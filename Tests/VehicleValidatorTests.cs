using Application.EntitiesValidators;
using Domain.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Tests
{
    public class VehicleValidatorTests
    {
        private readonly VehicleValidator _validator;

        public VehicleValidatorTests()
        {
            _validator = new VehicleValidator();
        }

        [Fact]
        public void Validate_WithValidVehicle_ShouldNotHaveValidationErrors()
        {         
            var vehicle = new Vehicle(null, 50000m, "Toyota", "Corolla");
            var result = _validator.TestValidate(vehicle);          
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WithNullValue_ShouldHaveValidationError()
        {         
            var vehicle = new Vehicle(null, null, "Toyota", "Corolla");
            var result = _validator.TestValidate(vehicle);
            result.ShouldHaveValidationErrorFor(v => v.Value);
        }

        [Fact]
        public void Validate_WithZeroValue_ShouldHaveValidationError()
        {      
            var vehicle = new Vehicle(null, 0m, "Toyota", "Corolla");
            var result = _validator.TestValidate(vehicle);
            result.ShouldHaveValidationErrorFor(v => v.Value);
        }

        [Fact]
        public void Validate_WithNullBranch_ShouldHaveValidationError()
        {          
            var vehicle = new Vehicle(null, 50000m, null, "Corolla");
            var result = _validator.TestValidate(vehicle);
            result.ShouldHaveValidationErrorFor(v => v.Branch);
        }

        [Fact]
        public void Validate_WithNullModel_ShouldHaveValidationError()
        {           
            var vehicle = new Vehicle(null, 50000m, "Toyota", null);
            var result = _validator.TestValidate(vehicle);   
            result.ShouldHaveValidationErrorFor(v => v.Model);
        }
    }
} 