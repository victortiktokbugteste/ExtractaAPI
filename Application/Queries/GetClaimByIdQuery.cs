using Application.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public class GetClaimByIdQuery : IRequest<GetClaimResponse>
    {
        public int Id { get; set; }

        public GetClaimByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}
