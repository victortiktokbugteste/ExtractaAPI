using Application.Queries.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public class GetAllClaimsQuery : IRequest<IEnumerable<GetClaimResponse>>
    {
    }
} 