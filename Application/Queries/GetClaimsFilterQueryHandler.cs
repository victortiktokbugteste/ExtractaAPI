using Application.Queries.Response;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public class GetClaimsFilterQueryHandler : IRequestHandler<GetClaimsFilterQuery, IEnumerable<GetClaimResponse>>
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IMapper _mapper;

        public GetClaimsFilterQueryHandler(IClaimRepository claimRepository, IMapper mapper)
        {
            _claimRepository = claimRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetClaimResponse>> Handle(GetClaimsFilterQuery request, CancellationToken cancellationToken)
        {
            var claims = await _claimRepository.GetAllClaims(request.InsuredName, request.InsuredCpf, request.Branch, request.Model);
            return _mapper.Map<IEnumerable<GetClaimResponse>>(claims);
        }
    }
}
