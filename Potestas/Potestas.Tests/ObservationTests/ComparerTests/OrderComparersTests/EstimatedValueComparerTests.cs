using Moq;
using Xunit;
using Potestas.Observations.Comparers.OrderComparers;

namespace Potestas.Tests.ObservationTests.ComparerTests.OrderComparersTests
{
    public class EstimatedValueComparerTests
    {
        [Fact]
        public void ComparerTest_FirstObservationIsNull_ResultIsMinus1()
        {
            // Arange
            var sut = new EstimatedValueComparer();
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
            var sut = new EstimatedValueComparer();
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
            var sut = new EstimatedValueComparer();
            var energyMock1 = new Mock<IEnergyObservation>();
            var energyMock2 = energyMock1;

            // Act
            var actualResult = sut.Compare(energyMock1.Object, energyMock2.Object);

            // Assert
            Assert.Equal(0, actualResult);
        }

        [Theory]
        [InlineData(double.NaN, double.NaN, 0)]
        [InlineData(double.MinValue, double.MaxValue, -1)]
        [InlineData(double.MaxValue, double.MinValue, 1)]
        [InlineData(0.001, 0.001, 0)]
        [InlineData(-0.001, -0.001, 0)]
        [InlineData(-0.001, 0.001, -1)]
        [InlineData(0.00001, 0.001, -1)]
        [InlineData(0.01, 0.0000001, 1)]
        public void CompareTest_PassTwoEnergyObservation_Return1OrMinus1Or0(double estimatedValue1, double estimatedValue2, int expectedResult)
        {
            // Arange
            var sut = new EstimatedValueComparer();
            var energyMock1 = GetEnergyObservationMock(estimatedValue1);
            var energyMock2 = GetEnergyObservationMock(estimatedValue2);


            // Act
            var actualResult = sut.Compare(energyMock1.Object, energyMock2.Object);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        //TODO move to the separate class all GetEnergyObservationMock methods from ComparerTests
        private Mock<IEnergyObservation> GetEnergyObservationMock(double estimatedValue)
        {
            var energyObservationMock = new Mock<IEnergyObservation>();

            energyObservationMock.SetupGet(m => m.EstimatedValue).Returns(estimatedValue);

            return energyObservationMock;
        }

        //TODO ClassData for 1-3 test
        // https://andrewlock.net/creating-strongly-typed-xunit-theory-test-data-with-theorydata/
    }
}
