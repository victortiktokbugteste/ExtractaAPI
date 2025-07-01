using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class PersonDto
    {
        [SwaggerSchema(Description = "Nome da Pessoa")]
        public string? Name { get; set; }

        [SwaggerSchema(Description = "Cpf da Pessoa")]
        public string? Cpf { get; set; }

        [SwaggerSchema(Description = "Data de Nascimento")]
        public DateTime? DateOfBirth { get; set; }
    }
}
