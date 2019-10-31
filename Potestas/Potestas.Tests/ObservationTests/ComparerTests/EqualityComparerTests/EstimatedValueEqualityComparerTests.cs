using Moq;
using Xunit;
using Potestas.Observations.Comparers.EqualityComparers;

namespace Potestas.Tests.ObservationTests.ComparerTests.EqualityComparerTests
{
    public class EstimatedValueEqualityComparerTests
    {
        [Fact]
        public void EqualsTest_FirstObservationIsNullSecondObservationIsNotNull_ResultIsFalse()
        {
            // Arange
            var sut = new EstimatedValueEqualityComparer<IEnergyObservation>();
            var energyMock = new Mock<IEnergyObservation>();

            // Act
            var actualResult = sut.Equals(null, energyMock.Object);

            // Assert
            Assert.False(actualResult);
        }

        [Fact]
        public void EqualsTest_FirstObservationIsNotNullSecondObservationIsNull_ResultIsFalse()
        {
            // Arange
            var sut = new EstimatedValueEqualityComparer<IEnergyObservation>();
            var energyMock = new Mock<IEnergyObservation>();

            // Act
            var actualResult = sut.Equals(energyMock.Object, null);

            // Assert
            Assert.False(actualResult);
        }


        [Fact]
        public void EqualsTest_ObservationReferencesAreEqual_ResultIsTrue()
        {
            // Arange
            var sut = new EstimatedValueEqualityComparer<IEnergyObservation>();
            var energyMock1 = new Mock<IEnergyObservation>();
            var energyMock2 = energyMock1;

            // Act
            var actualResult = sut.Equals(energyMock1.Object, energyMock2.Object);

            // Assert
            Assert.True(actualResult);
        }

        [Theory]
        [InlineData(double.NaN, double.NaN, true)]
        [InlineData(double.MinValue, double.MinValue, true)]
        [InlineData(double.MaxValue, double.MaxValue, true)]
        [InlineData(double.PositiveInfinity, double.PositiveInfinity, true)]
        [InlineData(double.NegativeInfinity, double.NegativeInfinity, true)]
        [InlineData(0.001, 0.001, true)]
        [InlineData(-0.001, -0.001, true)]
        [InlineData(0.001, 0.0011, true)]
        [InlineData(0.01, 0.00001, false)]
        [InlineData(-0.001, 0.001, false)]
        [InlineData(0.01, 0.0000001, false)]
        public void EqualsTest_PassTwoEnergyObservation_ReturnTrueOrFalse(double estimatedValue1, double estimatedValue2, bool expectedResult)
        {
            // Arange
            var sut = new EstimatedValueEqualityComparer<IEnergyObservation>();
            var energyMock1 = GetEnergyObservationMock(estimatedValue1);
            var energyMock2 = GetEnergyObservationMock(estimatedValue2);


            // Act
            var actualResult = sut.Equals(energyMock1.Object, energyMock2.Object);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(double.NaN, double.NaN)]
        [InlineData(double.MinValue, double.MinValue)]
        [InlineData(double.MaxValue, double.MaxValue)]
        [InlineData(double.PositiveInfinity, double.PositiveInfinity)]
        [InlineData(0.001, 0.001)]
        [InlineData(-0.001, -0.001)]
        [InlineData(0.001, 0.0011)]
        [InlineData(0.001, 0.00122)]
        public void GetHashCodeTest_PassTwoEqualEnergyObservationWithThousandPrecision_HashCodesAreEqual(double estimatedValue1, double estimatedValue2)
        {
            // Arange
            var sut = new EstimatedValueEqualityComparer<IEnergyObservation>();
            var energyMock1 = GetEnergyObservationMock(estimatedValue1);
            var energyMock2 = GetEnergyObservationMock(estimatedValue2);


            // Act
            var hashCode1 = sut.GetHashCode(energyMock1.Object);
            var hashCode2 = sut.GetHashCode(energyMock2.Object);

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        [Theory]
        [InlineData(double.NaN, double.MinValue)]
        [InlineData(double.MaxValue, double.MinValue)]
        [InlineData(0.01, 0.001)]
        [InlineData(-0.01, -0.001)]
        [InlineData(0.01, 0.0011)]
        [InlineData(0.001, 0.0122)]
        public void GetHashCodeTest_PassTwoNotEqualEnergyObservationWithThousandPrecision_HashCodeAreNotEqual(double estimatedValue1, double estimatedValue2)
        {
            // Arange
            var sut = new EstimatedValueEqualityComparer<IEnergyObservation>();
            var energyMock1 = GetEnergyObservationMock(estimatedValue1);
            var energyMock2 = GetEnergyObservationMock(estimatedValue2);


            // Act
            var hashCode1 = sut.GetHashCode(energyMock1.Object);
            var hashCode2 = sut.GetHashCode(energyMock2.Object);

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }

        private Mock<IEnergyObservation> GetEnergyObservationMock(double estimatedValue)
        {
            var energyObservationMock = new Mock<IEnergyObservation>();

            energyObservationMock.SetupGet(m => m.EstimatedValue).Returns(estimatedValue);

            return energyObservationMock;
        }
    }
}
