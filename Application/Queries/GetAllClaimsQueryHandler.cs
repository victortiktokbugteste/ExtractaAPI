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
    public class GetAllClaimsQueryHandler : IRequestHandler<GetAllClaimsQuery, IEnumerable<GetClaimResponse>>
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IMapper _mapper;

        public GetAllClaimsQueryHandler(IClaimRepository claimRepository, IMapper mapper)
        {
            _claimRepository = claimRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetClaimResponse>> Handle(GetAllClaimsQuery request, CancellationToken cancellationToken)
        {
            var claims = await _claimRepository.GetAllClaims();
            return _mapper.Map<IEnumerable<GetClaimResponse>>(claims);
        }
    }
} 