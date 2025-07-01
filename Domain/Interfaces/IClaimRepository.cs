using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IClaimRepository : IRepository<Claim>
    {
        Task<IEnumerable<Claim>> GetAllClaims(string? InsuredName = "", string? InsuredCpf = "", string? Branch = "", string? Model = "");
    }
}
