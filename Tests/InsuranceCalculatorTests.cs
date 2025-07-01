using Application.Utils;
using FluentAssertions;
using System;
using Xunit;

namespace Tests
{
    public class InsuranceCalculatorTests
    {
        [Fact]
        public void CalculateInsuranceValue_ShouldReturnCorrectValue()
        {           
            decimal vehicleValue = 50000m;        
            decimal result = InsuranceCalculator.CalculateInsuranceValue(vehicleValue);         
            decimal expected = 0.025m * vehicleValue * 1.03m * 1.05m;
            expected = Math.Round(expected, 2);
            
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(10000, 135.19)]
        [InlineData(25000, 337.97)]
        [InlineData(50000, 675.94)]
        [InlineData(100000, 1351.88)]
        public void CalculateInsuranceValue_WithDifferentValues_ShouldReturnExpectedResults(decimal vehicleValue, decimal expectedValue)
        {          
            decimal result = InsuranceCalculator.CalculateInsuranceValue(vehicleValue);
            result.Should().Be(expectedValue);
        }

    }
} 