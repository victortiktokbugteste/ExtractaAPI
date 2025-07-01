using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Vehicle
    {
        public Vehicle(int? id, decimal? value, string? branch, string? model)
        {
            this.Id = id;
            this.Value = value;
            this.Branch = branch;
            this.Model = model;
        }
        public int? Id { get; set; }
        public decimal? Value { get; private set; }
        public string? Branch { get; private set; }
        public string? Model { get; private set; }
    }
}
