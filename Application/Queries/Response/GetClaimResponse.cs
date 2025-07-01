using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Response
{
    public class GetClaimResponse
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int InsuredId { get; set; }
        public Person? Insured { get; set; }
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public decimal ClaimValue { get; set; }
    }
}
