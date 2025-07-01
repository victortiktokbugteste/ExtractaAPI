using Application.Commands.Response;
using Application.Utils;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Commands
{
    public class SaveClaimCommandHandler : IRequestHandler<SaveClaimCommand, bool>
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<Person> _personRepository;

        private readonly IValidator<Claim> _claimValidator;
        private readonly IValidator<Vehicle> _vehicleValidator;
        private readonly IValidator<Person> _personValidator;

        public SaveClaimCommandHandler(IClaimRepository claimRepository, 
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

        public async Task<bool> Handle(SaveClaimCommand command, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var vehicle = new Vehicle(null, command.Vehicle?.Value, command.Vehicle?.Branch, command.Vehicle?.Model);

                //Faz a validação do veículo.
                var vehicleValidations = await _vehicleValidator.ValidateAsync(vehicle, cancellationToken);
                if (!vehicleValidations.IsValid)
                {
                    throw new FluentValidation.ValidationException(vehicleValidations.Errors);
                }

                int vehicleId = await _vehicleRepository.AddAsync(vehicle);

                var insured = new Person(null, command.Insured?.Name, command.Insured?.Cpf, command.Insured?.DateOfBirth);

                //Faz a validação do Segurado.
                var personValidations = await _personValidator.ValidateAsync(insured, cancellationToken);
                if (!personValidations.IsValid)
                {
                    throw new FluentValidation.ValidationException(personValidations.Errors);
                }

                int insuredId = await _personRepository.AddAsync(insured);

                //Fazer o cálculo do Risco.
                decimal claimValue = 0;
                
                if (vehicle.Value.HasValue)
                {
                    claimValue = InsuranceCalculator.CalculateInsuranceValue(vehicle.Value.Value);
                }

                var claim = new Claim(insuredId, vehicleId, claimValue, null);

                //Faz a validação do Sinistro.
                var claimValidations = await _claimValidator.ValidateAsync(claim, cancellationToken);
                if (!claimValidations.IsValid)
                {
                    throw new FluentValidation.ValidationException(claimValidations.Errors);
                }

                var claimId = await _claimRepository.AddAsync(claim);
                scope.Complete();

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
