using Moq;
using Potestas.Analizers;
using Potestas.Observations;
using System;
using System.Collections.Generic;
using Xunit;

namespace Potestas.Tests.AnalizerTests
{
    public class LINQAnalizerTests
    {
        [Theory]
        [InlineData(74.8)]
        public void GetAverageEnergyTest_ReturnAverageEnergy(double excpectedAvarageEnergy)
        {
            // Arange
            var fakeStorage = GetStorageMock().Object;
            var sut = new LINQAnalizer(fakeStorage);

            // Act
            var actualResult = sut.GetAverageEnergy();

            // Assert
            Assert.Equal(excpectedAvarageEnergy, actualResult, 1);
        }

        [Theory]
        [InlineData(2019, 09, 01, 2019, 11, 01, 82)]
        [InlineData(2018, 10, 01, 2019, 11, 01, 62)]
        [InlineData(2018, 08, 01, 2019, 11, 01, 74.8)]
        public void GetAverageEnergyTest_PassDateTimeRange_ReturnAverageEnergy(int yearStart, int monthStart, int dayStart,
                                                                               int yearEnd, int monthEnd, int dayEnd,
                                                                               double excpectedAvarageEnergy)
        {
            // Arange
            var fakeStorage = GetStorageMock().Object;
            var dateStart = new DateTime(yearStart, monthStart, dayStart);
            var dateEnd = new DateTime(yearEnd, monthEnd, dayEnd);
            var sut = new LINQAnalizer(fakeStorage);

            // Act
            var actualResult = sut.GetAverageEnergy(dateStart, dateEnd);

            // Assert
            Assert.Equal(excpectedAvarageEnergy, actualResult, 1);
        }

        [Theory]
        [InlineData(242)]
        public void GetMaxEnergyTest_ReturnAverageEnergy(double excpectedMaxEnergy)
        {
            // Arange
            var fakeStorage = GetStorageMock().Object;
            var sut = new LINQAnalizer(fakeStorage);

            // Act
            var actualResult = sut.GetMaxEnergy();

            // Assert
            Assert.Equal(excpectedMaxEnergy, actualResult, 1);
        }


        [Theory]
        [InlineData(2019, 10, 15, 242)]
        [InlineData(2019, 09, 01, 2)]
        [InlineData(2018, 08, 15, 126)]
        public void GetMaxEnergyTest_PassDateTimeRange_ReturnAverageEnergy(int yearStart, int monthStart, int dayStart,
                                                                           double excpectedMaxEnergy)
        {
            // Arange
            var fakeStorage = GetStorageMock().Object;
            var dateObservation = new DateTime(yearStart, monthStart, dayStart);
            var sut = new LINQAnalizer(fakeStorage);

            // Act
            var actualResult = sut.GetMaxEnergy(dateObservation);

            // Assert
            Assert.Equal(excpectedMaxEnergy, actualResult, 1);
        }

        private Mock<IEnergyObservationStorage<IEnergyObservation>> GetStorageMock()
        {
            var fakeList = new List<IEnergyObservation>();

            fakeList.Add(new FlashObservation(1, 2, new Coordinates(11, 22), new DateTime(2019, 10, 15)));
            fakeList.Add(new FlashObservation(11, 22, new Coordinates(11, 22), new DateTime(2019, 10, 15)));
            fakeList.Add(new FlashObservation(1, 2, new Coordinates(1, 2), new DateTime(2019, 09, 01)));
            fakeList.Add(new FlashObservation(1, 2, new Coordinates(14, 14), new DateTime(2018, 10, 15)));
            fakeList.Add(new FlashObservation(9, 14, new Coordinates(14, 14), new DateTime(2018, 08, 15)));

            var storageMock = new Mock<IEnergyObservationStorage<IEnergyObservation>>();
            storageMock.Setup(storage => storage.GetEnumerator()).Returns(() => fakeList.GetEnumerator());

            return storageMock;
        }
    }
}
