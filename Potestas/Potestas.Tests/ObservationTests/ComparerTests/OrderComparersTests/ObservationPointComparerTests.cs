using System.Collections.Generic;
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
            var sut = new ObservationPointComparer<IEnergyObservation>();
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
            var sut = new ObservationPointComparer<IEnergyObservation>();
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
            var sut = new ObservationPointComparer<IEnergyObservation>();
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
            var sut = new ObservationPointComparer<IEnergyObservation>();
            var energyMock1 = GetEnergyObservationMock(xCoordinate1, yCoordinate1);
            var energyMock2 = GetEnergyObservationMock(xCoordinate2, yCoordinate2);


            // Act
            var actualResult = sut.Compare(energyMock1.Object, energyMock2.Object);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void CompareTest_SortCollectionAcordingObservationPointComparer_RightOrderOnCollection()
        {
            // Arange
            var sutComparer = new ObservationPointComparer<IEnergyObservation>();

            var energyMock1 = GetEnergyObservationMock(2, 2);
            var energyMock2 = GetEnergyObservationMock(1, 1);
            var energyMock3 = GetEnergyObservationMock(-0.01, 1);
            var energyMock4 = GetEnergyObservationMock(-0.1, 1);

            var listActual = new List<IEnergyObservation>();
            var listExpected = new List<IEnergyObservation>();

            listActual.Add(energyMock1.Object);
            listActual.Add(energyMock2.Object);
            listActual.Add(energyMock3.Object);
            listActual.Add(energyMock4.Object);

            listExpected.Add(energyMock4.Object);
            listExpected.Add(energyMock3.Object);
            listExpected.Add(energyMock2.Object);
            listExpected.Add(energyMock1.Object);

            // Act
            listActual.Sort(sutComparer);

            // Assert
            Assert.Equal(listExpected, listActual);
        }

        private Mock<IEnergyObservation> GetEnergyObservationMock(double xCoordinate, double yCoordinate)
        {
            var energyObservationMock = new Mock<IEnergyObservation>();

            energyObservationMock.SetupGet(m => m.ObservationPoint).Returns(new Coordinates(xCoordinate, yCoordinate));

            return energyObservationMock;
        }
    }
}
