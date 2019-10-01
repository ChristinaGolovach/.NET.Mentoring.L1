using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;
using Potestas.Observations.Comparers.EqualityComparers;

namespace Potestas.Tests.ObservationTests.ComparerTests.EqualityComparerTests
{
    public class ObservationTimeEqualityComparerTests
    {
        [Fact]
        public void Equals_Test_FirstObservationIsNullSecondObservationIsNotNull_ResultIsFalse()
        {
            // Arange
            var sut = new ObservationTimeEqualityComparer();
            var energyMock = new Mock<IEnergyObservation>();

            // Act
            var actualResult = sut.Equals(null, energyMock.Object);

            // Assert
            Assert.False(actualResult);
        }

        [Fact]
        public void Equals_FirstObservationIsNotNullSecondObservationIsNull_ResultIsFalse()
        {
            // Arange
            var sut = new ObservationTimeEqualityComparer();
            var energyMock = new Mock<IEnergyObservation>();

            // Act
            var actualResult = sut.Equals(energyMock.Object, null);

            // Assert
            Assert.False(actualResult);
        }

        [Fact]
        public void Equals_ObservationReferencesAreEqual_ResultIsTrue()
        {
            // Arange
            var sut = new ObservationTimeEqualityComparer();
            var energyMock1 = new Mock<IEnergyObservation>();
            var energyMock2 = energyMock1;

            // Act
            var actualResult = sut.Equals(energyMock1.Object, energyMock2.Object);

            // Assert
            Assert.True(actualResult);
        }

        [Theory]
        [InlineData(2019, 10, 1, 2019, 10, 1, true)]
        [InlineData(2019, 9, 12, 2019, 9, 13, false)]
        [InlineData(2019, 9, 12, 2019, 8, 12, false)]
        public void EqualsTest_PassTwoEnergyObservation_ReturnTrueOrFalse
                                (int year1, int month1, int day1,
                                 int year2, int month2, int day2, bool expectedResult)
        {
            // Arange
            var sut = new ObservationTimeEqualityComparer();
            var energyMock1 = GetEnergyObservationMock(year1, month1, day1);
            var energyMock2 = GetEnergyObservationMock(year2, month2, day2);


            // Act
            var actualResult = sut.Equals(energyMock1.Object, energyMock2.Object);

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
