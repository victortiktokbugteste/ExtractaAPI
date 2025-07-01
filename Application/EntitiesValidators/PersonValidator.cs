using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EntitiesValidators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome é obrigatório")
                .MaximumLength(100)
                .WithMessage("O nome não pode ter mais de 100 caracteres");

            RuleFor(x => x.Cpf)
                .NotEmpty()
                .WithMessage("O CPF é obrigatório")
                .Length(11)
                .WithMessage("O CPF deve ter 11 dígitos")
                .Matches(@"^\d{11}$")
                .WithMessage("O CPF deve conter apenas números");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("A data de nascimento é obrigatória")
                .LessThan(DateTime.Now)
                .WithMessage("A data de nascimento não pode ser no futuro");
        }
    }
}
