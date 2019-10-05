using System;
using Xunit;

namespace Potestas.Tests
{
    public class CoordinatesTests
    {
        [Theory]
        [InlineData(double.MinValue, double.MaxValue)]
        [InlineData(-91, -1)]
        [InlineData(-90.01, -0.0001)]
        [InlineData(91, 181)]
        [InlineData(90.00001, 180.001)]
        public void CtorTests_PassValueOutOfRange_ArgumentOutOfRangeException(double xCoordinate, double yCoordinate)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinates(xCoordinate, yCoordinate));
        }
    }
}
