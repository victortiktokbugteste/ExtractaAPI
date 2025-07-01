using Application.Queries.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Queries
{
    public class GetClaimByIdQueryHandler : IRequestHandler<GetClaimByIdQuery, GetClaimResponse>
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IMapper _mapper;

        public GetClaimByIdQueryHandler(IClaimRepository claimRepository, IMapper mapper)
        {
            _claimRepository = claimRepository;
            _mapper = mapper;
        }

        public async Task<GetClaimResponse> Handle(GetClaimByIdQuery request, CancellationToken cancellationToken)
        {
            var claim = await _claimRepository.GetById(request.Id);
            
            return _mapper.Map<GetClaimResponse>(claim);
        }
    }
}
