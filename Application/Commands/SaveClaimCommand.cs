using Application.Commands.Response;
using Application.Dtos;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class SaveClaimCommand : IRequest<bool>
    {
        public PersonDto? Insured { get; set; }
        public VehicleDto? Vehicle { get; set; }
    }
}
