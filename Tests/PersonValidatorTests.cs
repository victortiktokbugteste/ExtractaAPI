using Application.EntitiesValidators;
using Domain.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Tests
{
    public class PersonValidatorTests
    {
        private readonly PersonValidator _validator;

        public PersonValidatorTests()
        {
            _validator = new PersonValidator();
        }

        [Fact]
        public void Validate_WithValidPerson_ShouldNotHaveValidationErrors()
        {
            
            var person = new Person(null, "João da Silva", "12345678901", new DateTime(1980, 1, 1));        
            var result = _validator.TestValidate(person);          
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WithNullName_ShouldHaveValidationError()
        {
            
            var person = new Person(null, null, "12345678901", new DateTime(1980, 1, 1));
            var result = _validator.TestValidate(person);
            result.ShouldHaveValidationErrorFor(p => p.Name);
        }

        [Fact]
        public void Validate_WithNullCpf_ShouldHaveValidationError()
        {
            
            var person = new Person(null, "João da Silva", null, new DateTime(1980, 1, 1));
            var result = _validator.TestValidate(person);
            result.ShouldHaveValidationErrorFor(p => p.Cpf);
        }

        [Fact]
        public void Validate_WithInvalidCpfLength_ShouldHaveValidationError()
        {
            
            var person = new Person(null, "João da Silva", "123456", new DateTime(1980, 1, 1));
            var result = _validator.TestValidate(person);
            result.ShouldHaveValidationErrorFor(p => p.Cpf);
        }

        [Fact]
        public void Validate_WithCpfContainingLetters_ShouldHaveValidationError()
        {
            
            var person = new Person(null, "João da Silva", "1234567890A", new DateTime(1980, 1, 1));
            var result = _validator.TestValidate(person);
            result.ShouldHaveValidationErrorFor(p => p.Cpf);
        }

        [Fact]
        public void Validate_WithNullDateOfBirth_ShouldHaveValidationError()
        {
            
            var person = new Person(null, "João da Silva", "12345678901", null);
            var result = _validator.TestValidate(person);
            result.ShouldHaveValidationErrorFor(p => p.DateOfBirth);
        }

    }
} 