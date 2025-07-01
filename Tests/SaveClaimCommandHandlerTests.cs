using Application.Commands;
using Application.Dtos;
using Application.Utils;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class SaveClaimCommandHandlerTests
    {
        private readonly Mock<IClaimRepository> _mockClaimRepository;
        private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
        private readonly Mock<IRepository<Person>> _mockPersonRepository;
        private readonly Mock<IValidator<Claim>> _mockClaimValidator;
        private readonly Mock<IValidator<Vehicle>> _mockVehicleValidator;
        private readonly Mock<IValidator<Person>> _mockPersonValidator;
        private readonly SaveClaimCommandHandler _handler;

        public SaveClaimCommandHandlerTests()
        {
            _mockClaimRepository = new Mock<IClaimRepository>();
            _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
            _mockPersonRepository = new Mock<IRepository<Person>>();
            _mockClaimValidator = new Mock<IValidator<Claim>>();
            _mockVehicleValidator = new Mock<IValidator<Vehicle>>();
            _mockPersonValidator = new Mock<IValidator<Person>>();

            _handler = new SaveClaimCommandHandler(
                _mockClaimRepository.Object,
                _mockVehicleRepository.Object,
                _mockPersonRepository.Object,
                _mockClaimValidator.Object,
                _mockVehicleValidator.Object,
                _mockPersonValidator.Object
            );

            // Configuração padrão para os validadores
            _mockVehicleValidator.Setup(v => v.ValidateAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockPersonValidator.Setup(v => v.ValidateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockClaimValidator.Setup(v => v.ValidateAsync(It.IsAny<Claim>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
        }

        [Fact]
        public async Task Handle_ShouldCalculateCorrectClaimValue()
        {
            
            decimal vehicleValue = 50000m;
            decimal expectedClaimValue = InsuranceCalculator.CalculateInsuranceValue(vehicleValue);

            var command = new SaveClaimCommand
            {
                Insured = new PersonDto
                {
                    Name = "João da Silva",
                    Cpf = "12345678901",
                    DateOfBirth = new DateTime(1980, 1, 1)
                },
                Vehicle = new VehicleDto
                {
                    Value = vehicleValue,
                    Branch = "Toyota",
                    Model = "Corolla"
                }
            };

            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>()))
                .ReturnsAsync(1);
            _mockPersonRepository.Setup(r => r.AddAsync(It.IsAny<Person>()))
                .ReturnsAsync(1);
            _mockClaimRepository.Setup(r => r.AddAsync(It.Is<Claim>(c => c.ClaimValue == expectedClaimValue)))
                .ReturnsAsync(1);

            
            var result = await _handler.Handle(command, CancellationToken.None);

            
            result.Should().BeTrue();
            _mockClaimRepository.Verify(r => r.AddAsync(It.Is<Claim>(c => c.ClaimValue == expectedClaimValue)), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidVehicle_ShouldThrowValidationException()
        {
            
            var command = new SaveClaimCommand
            {
                Insured = new PersonDto
                {
                    Name = "João da Silva",
                    Cpf = "12345678901",
                    DateOfBirth = new DateTime(1980, 1, 1)
                },
                Vehicle = new VehicleDto
                {
                    Value = -1000m, // Valor inválido
                    Branch = "Toyota",
                    Model = "Corolla"
                }
            };

            var validationFailure = new ValidationFailure("Value", "O valor do veículo deve ser maior que zero");
            var validationResult = new ValidationResult(new[] { validationFailure });

            _mockVehicleValidator.Setup(v => v.ValidateAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            _mockVehicleRepository.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Never);
            _mockPersonRepository.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Never);
            _mockClaimRepository.Verify(r => r.AddAsync(It.IsAny<Claim>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WithInvalidPerson_ShouldThrowValidationException()
        {
            
            var command = new SaveClaimCommand
            {
                Insured = new PersonDto
                {
                    Name = "", // Nome inválido
                    Cpf = "12345678901",
                    DateOfBirth = new DateTime(1980, 1, 1)
                },
                Vehicle = new VehicleDto
                {
                    Value = 50000m,
                    Branch = "Toyota",
                    Model = "Corolla"
                }
            };

            _mockVehicleValidator.Setup(v => v.ValidateAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var validationFailure = new ValidationFailure("Name", "O nome é obrigatório");
            var validationResult = new ValidationResult(new[] { validationFailure });

            _mockPersonValidator.Setup(v => v.ValidateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);


            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            _mockVehicleRepository.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Once);
            _mockPersonRepository.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Never);
            _mockClaimRepository.Verify(r => r.AddAsync(It.IsAny<Claim>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WithInvalidClaim_ShouldThrowValidationException()
        {
            
            var command = new SaveClaimCommand
            {
                Insured = new PersonDto
                {
                    Name = "João da Silva",
                    Cpf = "12345678901",
                    DateOfBirth = new DateTime(1980, 1, 1)
                },
                Vehicle = new VehicleDto
                {
                    Value = 50000m,
                    Branch = "Toyota",
                    Model = "Corolla"
                }
            };

            _mockVehicleValidator.Setup(v => v.ValidateAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockPersonValidator.Setup(v => v.ValidateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>()))
                .ReturnsAsync(1);
            _mockPersonRepository.Setup(r => r.AddAsync(It.IsAny<Person>()))
                .ReturnsAsync(1);

            var validationFailure = new ValidationFailure("InsuredId", "O ID do segurado deve ser maior que zero");
            var validationResult = new ValidationResult(new[] { validationFailure });

            _mockClaimValidator.Setup(v => v.ValidateAsync(It.IsAny<Claim>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);


            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            _mockVehicleRepository.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Once);
            _mockPersonRepository.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Once);
            _mockClaimRepository.Verify(r => r.AddAsync(It.IsAny<Claim>()), Times.Never);
        }
    }
} 