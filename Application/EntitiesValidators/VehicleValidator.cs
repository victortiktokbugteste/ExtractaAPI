using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EntitiesValidators
{
    public class VehicleValidator : AbstractValidator<Vehicle>
    {
        public VehicleValidator()
        {
            RuleFor(x => x.Value)
                .NotEmpty()
                .WithMessage("O valor do veículo é obrigatório")
                .GreaterThan(0)
                .WithMessage("O valor do veículo deve ser maior que zero");

            RuleFor(x => x.Branch)
                .NotEmpty()
                .WithMessage("A marca do veículo é obrigatória")
                .MaximumLength(50)
                .WithMessage("A marca não pode ter mais de 50 caracteres");

            RuleFor(x => x.Model)
                .NotEmpty()
                .WithMessage("O modelo do veículo é obrigatório")
                .MaximumLength(100)
                .WithMessage("O modelo não pode ter mais de 100 caracteres");
        }
    }
}
