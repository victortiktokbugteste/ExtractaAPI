using AutoMapper;
using Domain.Entities;
using Application.Queries.Response;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Claim, GetClaimResponse>();
        }
    }
} 