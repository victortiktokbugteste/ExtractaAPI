using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EntitiesValidators
{
    public class ClaimValidator : AbstractValidator<Claim>
    {
        public ClaimValidator()
        {
            RuleFor(x => x.InsuredId)
                .NotEmpty()
                .WithMessage("O ID do segurado é obrigatório")
                .GreaterThan(0)
                .WithMessage("O ID do segurado deve ser maior que zero");

            RuleFor(x => x.VehicleId)
                .NotEmpty()
                .WithMessage("O ID do veículo é obrigatório")
                .GreaterThan(0)
                .WithMessage("O ID do veículo deve ser maior que zero");
                
            RuleFor(x => x.ClaimValue)
                .GreaterThan(0)
                .When(x => x.ClaimValue.HasValue)
                .WithMessage("O valor do sinistro deve ser maior que zero");
        }
    }
}
