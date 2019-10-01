using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;
using Potestas.Observations.Comparers.OrderComparers;

namespace Potestas.Tests.ObservationTests.ComparerTests.OrderComparersTests
{
    public class ObservationTimeComparerTests
    {
        [Fact]
        public void ComparerTest_FirstObservationIsNull_ResultIsMinus1()
        {
            // Arange
            var sut = new ObservationTimeComparer();
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
            var sut = new ObservationTimeComparer();
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
            var sut = new ObservationTimeComparer();
            var energyMock1 = new Mock<IEnergyObservation>();
            var energyMock2 = energyMock1;

            // Act
            var actualResult = sut.Compare(energyMock1.Object, energyMock2.Object);

            // Assert
            Assert.Equal(0, actualResult);
        }

        [Theory]
        [InlineData(2019, 10, 1, 2019, 10, 1, 0)]
        [InlineData(2019, 9, 12, 2019, 9, 13, -1)]
        [InlineData(2019, 9, 12, 2019, 8, 12, 1)]
        public void CompareTest_PassTwoEnergyObservation_Return1OrMinus1Or0  
                                (int year1, int month1, int day1,
                                 int year2, int month2, int day2, int expectedResult)
        {
            // Arange
            var sut = new ObservationTimeComparer();
            var energyMock1 = GetEnergyObservationMock(year1, month1, day1);
            var energyMock2 = GetEnergyObservationMock(year2, month2, day2);


            // Act
            var actualResult = sut.Compare(energyMock1.Object, energyMock2.Object);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        private Mock<IEnergyObservation> GetEnergyObservationMock(int year, int month, int day)
        {
            var energyObservationMock = new Mock<IEnergyObservation>();

            energyObservationMock.SetupGet(m => m.ObservationTime).Returns(new DateTime(year, month, day));

            return energyObservationMock;
        }
    }
}
