using Application.Dtos;
using Domain.Entities;

namespace Application.Commands.Response
{
    public class SaveClaimResponse
    {
        public DateTime CreateDate { get; set; }
        public PersonDto? Insured { get; set; }
        public VehicleDto? Vehicle { get; set; }

        public static SaveClaimResponse FromDomain(Claim claim) => 
            new SaveClaimResponse 
            { 
                CreateDate = claim.CreateDate, 
                Insured = new PersonDto() {
                    Name = claim.Insured?.Name,
                    Cpf = claim.Insured?.Cpf,
                    DateOfBirth = claim.Insured?.DateOfBirth
                },
                Vehicle = new VehicleDto()
                {
                    Value = claim.Vehicle?.Value,
                    Branch = claim.Vehicle?.Branch,
                    Model = claim.Vehicle?.Model
                }
            };
    }
}
