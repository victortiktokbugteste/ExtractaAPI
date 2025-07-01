using Application.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public class GetClaimsFilterQuery : IRequest<IEnumerable<GetClaimResponse>>
    {
        public string InsuredName { get; set; }
        public string InsuredCpf { get; set; }
        public string Branch { get; set; }
        public string Model { get; set; }
    }
}
