using Application.EntitiesValidators;
using Domain.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Tests
{
    public class ClaimValidatorTests
    {
        private readonly ClaimValidator _validator;

        public ClaimValidatorTests()
        {
            _validator = new ClaimValidator();
        }

        [Fact]
        public void Validate_WithValidClaim_ShouldNotHaveValidationErrors()
        {          
            var claim = new Claim(1, 1, 5000m, null);            
            var result = _validator.TestValidate(claim);            
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WithZeroInsuredId_ShouldHaveValidationError()
        {          
            var claim = new Claim(0, 1, 5000m, null);           
            var result = _validator.TestValidate(claim);           
            result.ShouldHaveValidationErrorFor(c => c.InsuredId);
        }

        [Fact]
        public void Validate_WithZeroVehicleId_ShouldHaveValidationError()
        {           
            var claim = new Claim(1, 0, 5000m, null);            
            var result = _validator.TestValidate(claim);          
            result.ShouldHaveValidationErrorFor(c => c.VehicleId);
        }
        

        [Fact]
        public void Validate_WithNullClaimValue_ShouldNotHaveValidationError()
        {           
            var claim = new Claim(1, 1, null, null);
            var result = _validator.TestValidate(claim);       
            result.ShouldNotHaveValidationErrorFor(c => c.ClaimValue);
        }      
       
    }
} 