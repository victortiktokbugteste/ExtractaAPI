using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public static class InsuranceCalculator
    {
        private const decimal MARGEM_SEGURANCA = 0.03m; // 3%
        private const decimal LUCRO = 0.05m; // 5%

        public static decimal CalculateInsuranceValue(decimal vehicleValue)
        {
            decimal taxaDeRisco = ((vehicleValue * 5) / (2 * vehicleValue)) / 100;

            decimal premioDeRisco = taxaDeRisco * vehicleValue;

            decimal premioPuro = premioDeRisco * (1 + MARGEM_SEGURANCA);

            decimal premioComercial = premioPuro * (1 + LUCRO);

            return Math.Round(premioComercial, 2);
        }
    }
} 