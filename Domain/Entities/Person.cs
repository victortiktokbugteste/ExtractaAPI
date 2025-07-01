using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Person
    {
        public Person(int? id, string? name, string? cpf, DateTime? dateOfBirth)
        {
            this.Id = id;
            this.Name = name;
            this.Cpf = cpf;
            this.DateOfBirth = dateOfBirth;
        }
        public int? Id { get; set; }
        public string? Name { get; private set; }
        public string? Cpf { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
    }
}
