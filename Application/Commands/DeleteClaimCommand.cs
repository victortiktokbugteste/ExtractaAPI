using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class DeleteClaimCommand : IRequest<bool>
    {
        [SwaggerSchema(Description = "ID do Sinistro")]
        public int Id { get; set; }

        public DeleteClaimCommand(int id)
        {
            Id = id;
        }
    }
} 