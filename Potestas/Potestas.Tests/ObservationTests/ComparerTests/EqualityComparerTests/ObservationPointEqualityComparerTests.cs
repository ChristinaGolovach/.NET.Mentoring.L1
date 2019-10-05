using Moq;
using Xunit;
using Potestas.Observations.Comparers.EqualityComparers;

namespace Potestas.Tests.ObservationTests.ComparerTests.EqualityComparerTests
{
    public class ObservationPointEqualityComparerTests
    {
        [Fact]
        public void EqualsTest_FirstObservationIsNullSecondObservationIsNotNull_ResultIsFalse()
        {
            // Arange
            var sut = new ObservationPointEqualityComparer<IEnergyObservation>();
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
            var sut = new ObservationPointEqualityComparer<IEnergyObservation>();
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
            var sut = new ObservationPointEqualityComparer<IEnergyObservation>();
            var energyMock1 = new Mock<IEnergyObservation>();
            var energyMock2 = energyMock1;

            // Act
            var actualResult = sut.Equals(energyMock1.Object, energyMock2.Object);

            // Assert
            Assert.True(actualResult);
        }

        [Theory]
        [InlineData(double.NaN, double.NaN, double.NaN, double.NaN, true)]
        [InlineData(double.MinValue, double.MaxValue, double.MinValue, double.MaxValue, true)]
        [InlineData(0.001, 0.001, 0.001, 0.001, true)]
        [InlineData(0.001, 0.0011, 0.001, 0.001, true)]
        [InlineData(0, 1, 0, 1, true)]
        [InlineData(0, 1, 1, 0, false)]
        [InlineData(1, 1, 1, 2, false)]
        [InlineData(1, 1, 2, 1, false)]
        [InlineData(3, 1, 1, 2, false)]
        [InlineData(3, 1, 3, 0, false)]
        public void EqualsTest_PassTwoEnergyObservation_ReturnTrueOrFalse
                               (double xCoordinate1, double yCoordinate1,
                                double xCoordinate2, double yCoordinate2, bool expectedResult)
        {
            // Arange
            var sut = new ObservationPointEqualityComparer<IEnergyObservation>();
            var energyMock1 = GetEnergyObservationMock(xCoordinate1, yCoordinate1);
            var energyMock2 = GetEnergyObservationMock(xCoordinate2, yCoordinate2);


            // Act
            var actualResult = sut.Equals(energyMock1.Object, energyMock2.Object);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        private Mock<IEnergyObservation> GetEnergyObservationMock(double xCoordinate, double yCoordinate)
        {
            var energyObservationMock = new Mock<IEnergyObservation>();

            energyObservationMock.SetupGet(m => m.ObservationPoint).Returns(new Coordinates(xCoordinate, yCoordinate));

            return energyObservationMock;
        }
    }
}
