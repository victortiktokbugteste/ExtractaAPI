using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class VehicleDto
    {
        [SwaggerSchema(Description = "Valor do veículo")]
        public decimal? Value { get; set; }

        [SwaggerSchema(Description = "Marca do veículo")]
        public string? Branch { get; set; }

        [SwaggerSchema(Description = "Modelo do veículo")]
        public string? Model { get; set; }
    }
}
