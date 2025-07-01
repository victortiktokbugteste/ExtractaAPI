using Application.Exceptions;
using Application.Utils;
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
    public class UpdateClaimCommandHandler : IRequestHandler<UpdateClaimCommand, bool>
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<Person> _personRepository;

        private readonly IValidator<Claim> _claimValidator;
        private readonly IValidator<Vehicle> _vehicleValidator;
        private readonly IValidator<Person> _personValidator;

        public UpdateClaimCommandHandler(IClaimRepository claimRepository, 
            IRepository<Vehicle> vehicleRepository, 
            IRepository<Person> personRepository, 
            IValidator<Claim> claimValidator, 
            IValidator<Vehicle> vehicleValidator, 
            IValidator<Person> personValidator)
        {
            _claimRepository = claimRepository;
            _vehicleRepository = vehicleRepository;
            _personRepository = personRepository;
            _claimValidator = claimValidator;
            _vehicleValidator = vehicleValidator;
            _personValidator = personValidator;
        }

        public async Task<bool> Handle(UpdateClaimCommand request, CancellationToken cancellationToken)
        {
            var claim = await _claimRepository.GetById(request.Id);
            
            if (claim == null)
                throw new SingleErrorException("Sinistro não encontrado!");

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var vehicle = new Vehicle(claim.Vehicle?.Id, request.Vehicle?.Value, request.Vehicle?.Branch, request.Vehicle?.Model);
                var vehicleValidations = await _vehicleValidator.ValidateAsync(vehicle, cancellationToken);
                if (!vehicleValidations.IsValid)
                    throw new ValidationException(vehicleValidations.Errors);

                await _vehicleRepository.UpdateAsync(vehicle);

                var insured = new Person(claim.Insured?.Id, request.Insured?.Name, request.Insured?.Cpf, request.Insured?.DateOfBirth);
                var personValidations = await _personValidator.ValidateAsync(insured, cancellationToken);
                if (!personValidations.IsValid)
                    throw new ValidationException(personValidations.Errors);

                await _personRepository.UpdateAsync(insured);

                //Fazer o cálculo do Risco.
                decimal claimValue = 0;
                
                if (vehicle.Value.HasValue)
                {
                    claimValue = InsuranceCalculator.CalculateInsuranceValue(vehicle.Value.Value);
                }

                var claimToUpdate = new Claim(insured.Id.Value, vehicle.Id.Value, claimValue, claim.Id);
                claimToUpdate.UpdateDate = DateTime.Now;

                await _claimRepository.UpdateAsync(claimToUpdate);

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