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
    public class UpdateClaimCommand : IRequest<bool>
    {
        [SwaggerSchema(Description = "ID do Sinistro")]
        public int Id { get; set; }
        public PersonDto? Insured { get; set; }
        public VehicleDto? Vehicle { get; set; }
    }
} 