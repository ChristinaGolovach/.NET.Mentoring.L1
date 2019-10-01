using Moq;
using Xunit;
using Potestas.Observations.Comparers.OrderComparers;

namespace Potestas.Tests.ObservationTests.ComparerTests.OrderComparersTests
{
    public class ObservationPointComparerTests
    {
        [Fact]
        public void ComparerTest_FirstObservationIsNull_ResultIsMinus1()
        {
            // Arange
            var sut = new ObservationPointComparer();
            var energyMock = new Mock<IEnergyObservation>();

            // Act
            var actualResult = sut.Compare(null, energyMock.Object);

            // Assert
            Assert.Equal(-1, actualResult);
        }

        [Fact]
        public void ComparerTest_SecondObservationIsNull_ResultIs1()
        {
            // Arange
            var sut = new ObservationPointComparer();
            var energyMock = new Mock<IEnergyObservation>();

            // Act
            var actualResult = sut.Compare(energyMock.Object, null);

            // Assert
            Assert.Equal(1, actualResult);
        }


        [Fact]
        public void ComparerTest_ObservationReferencesAreEqual_ResultIs0()
        {
            // Arange
            var sut = new ObservationPointComparer();
            var energyMock1 = new Mock<IEnergyObservation>();
            var energyMock2 = energyMock1;

            // Act
            var actualResult = sut.Compare(energyMock1.Object, energyMock2.Object);

            // Assert
            Assert.Equal(0, actualResult);
        }

        [Theory]
        [InlineData(double.NaN, double.NaN, double.NaN, double.NaN, 0)]
        [InlineData(double.MinValue, double.MaxValue, double.MinValue, double.MaxValue, 0)]
        [InlineData(0.001, 0.001, 0.001, 0.001, 0)]
        [InlineData(0, 1, 0, 1, 0)]
        [InlineData(1, 1, 1, 2, -1)]
        [InlineData(1, 1, 2, 1, -1)]
        [InlineData(3, 1, 1, 2, 1)]
        [InlineData(3, 1, 3, 0, 1)]
        public void CompareTest_PassTwoEnergyObservation_Return1OrMinus1Or0
                                (double xCoordinate1, double yCoordinate1,
                                 double xCoordinate2, double yCoordinate2, int expectedResult)
        {
            // Arange
            var sut = new ObservationPointComparer();
            var energyMock1 = GetEnergyObservationMock(xCoordinate1, yCoordinate1);
            var energyMock2 = GetEnergyObservationMock(xCoordinate2, yCoordinate2);


            // Act
            var actualResult = sut.Compare(energyMock1.Object, energyMock2.Object);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        private Mock<IEnergyObservation> GetEnergyObservationMock(double xCoordinate, double yCoordinate)
        {
            var energyObservationMock = new Mock<IEnergyObservation>();

            energyObservationMock.SetupGet(m => m.ObservationPoint).Returns(new Coordinates() { X = xCoordinate, Y = yCoordinate });

            return energyObservationMock;
        }
    }
}
