using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Commands
{
    public class DeleteClaimCommandHandler : IRequestHandler<DeleteClaimCommand, bool>
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<Person> _personRepository;

        public DeleteClaimCommandHandler(IClaimRepository claimRepository,
            IRepository<Vehicle> vehicleRepository,
            IRepository<Person> personRepository)
        {
            _claimRepository = claimRepository;
            _vehicleRepository = vehicleRepository;
            _personRepository = personRepository;
        }

        public async Task<bool> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
        {
            var claim = await _claimRepository.GetById(request.Id);
            
            if (claim == null)
                throw new SingleErrorException("Sinistro não encontrado!");

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                await _claimRepository.DeleteAsync(request.Id);
                await _vehicleRepository.DeleteAsync(claim.Vehicle.Id.Value);
                await _personRepository.DeleteAsync(claim.Insured.Id.Value);

                scope.Complete();

                return true;
            }
            catch(Exception ex)
            {
                throw;
            }


        }
    }
} 