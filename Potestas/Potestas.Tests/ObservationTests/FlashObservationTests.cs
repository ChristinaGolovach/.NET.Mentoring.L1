using System;
using Xunit;
using Potestas.Observations;

namespace Potestas.Tests.ObservationTests
{
    public class FlashObservationTests
    {
        [Theory]
        [InlineData(10, -1)]
        [InlineData(10, -0.00001)]
        [InlineData(10, 2000000000.01)]
        public void CtorTests_PassIntensityOutOfRange_ArgumentOutOfRangeException(int durationMs, double intensity)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new FlashObservation(durationMs, intensity, new Coordinates()));
        }

        [Theory]
        [InlineData(10, 0.002)]
        [InlineData(1, 0.00001)]
        [InlineData(12, 23.2222)]
        public void EstimatedValueProperyTests_PassIntensityAndDurationMs_ReturnMultiplicationOfDurationAndIntensity(int durationMs, double intensity)
        {
            // Arange
            int precision = 3;
            var flashObservation = new FlashObservation(durationMs, intensity, new Coordinates());

            // Act
            var estimatedValue = flashObservation.EstimatedValue;

            // Assert
            Assert.Equal(estimatedValue, durationMs * intensity, precision);

        }

        [Theory]
        [InlineData(1, 11.1, 1, 11.1, 0.0011, 23.122, 0.0012, 23.1223)]
        [InlineData(2, 12.2, 2, 12.2, 23.0001, 23.0003, 23.0007, 23.0009)]
        public void OperatorEqualityTests_TwoEqualFlashObservation_ResultTrue(int durationMs1, double intensity1,
                                                                              int durationMs2, double intensity2,
                                                                              double p1XCoordinate, double p1YCoordinate,
                                                                              double p2XCoordinate, double p2YCoordinate)
        {
            // Arange
            var flashObservation1 = new FlashObservation(durationMs1, intensity1, new Coordinates(p1XCoordinate, p1YCoordinate), new DateTime(1));
            var flashObservation2 = new FlashObservation(durationMs2, intensity2, new Coordinates(p2XCoordinate, p2YCoordinate), new DateTime(1));

            // Act
            var actualResult = flashObservation1 == flashObservation2;

            // Assert
            Assert.True(actualResult);
        }

        [Theory]
        [InlineData(2333, 1)]
        [InlineData(2111, 2)]
        [InlineData(2, 3)]
        public void OperatorEqualityTests_PassDifferentDuration_ResulFalse(int durationMs1, int durationMs2)
        {
            // Arange
            var flashObservation1 = new FlashObservation(durationMs1, 11, new Coordinates(), new DateTime(1));
            var flashObservation2 = new FlashObservation(durationMs2, 11, new Coordinates(), new DateTime(1));

            // Act
            var actualResult = flashObservation1 == flashObservation2;

            // Assert
            Assert.False(actualResult);
        }

        [Theory]
        [InlineData(10.1, 11.1)]
        [InlineData(10.2, 12.2)]
        [InlineData(12.10, 12.001)]
        public void OperatorEqualityTests_PassDifferentIntensity_ResulFalse(double intensity1, double intensity2)
        {
            // Arange
            var flashObservation1 = new FlashObservation(1, intensity1, new Coordinates(), new DateTime(1));
            var flashObservation2 = new FlashObservation(1, intensity2, new Coordinates(), new DateTime(1));

            // Act
            var actualResult = flashObservation1 == flashObservation2;

            // Assert
            Assert.False(actualResult);
        }

        [Theory]
        [InlineData(23.1111, 23.122, 0.0012, 23.1223)]
        [InlineData(0.0001, 23.0003, 23.0007, 23.0009)]
        public void OperatorEqualityTests_PassDifferentPoints_ResulFalse(double p1XCoordinate, double p1YCoordinate,
                                                                         double p2XCoordinate, double p2YCoordinate)
        {
            // Arange
            var flashObservation1 = new FlashObservation(1, 1, new Coordinates(p1XCoordinate, p1YCoordinate), new DateTime(1));
            var flashObservation2 = new FlashObservation(1, 1, new Coordinates(p2XCoordinate, p2YCoordinate), new DateTime(1));

            // Act
            var actualResult = flashObservation1 == flashObservation2;

            // Assert
            Assert.False(actualResult);
        }


        [Theory]
        [InlineData(11, 12)]
        [InlineData(43, 45)]
        public void OperatorEqualityTests_PassDifferentTime_ResulFalse(long tiks1, long tiks2)
        {
            // Arange
            var flashObservation1 = new FlashObservation(1, 1, new Coordinates(), new DateTime(tiks1));
            var flashObservation2 = new FlashObservation(1, 1, new Coordinates(), new DateTime(tiks2));

            // Act
            var actualResult = flashObservation1 == flashObservation2;

            // Assert
            Assert.False(actualResult);
        }

        [Theory]
        [InlineData(2, 2, 11.1, 11.1, 0.0011, 23.122, 0.0012, 23.1223, 2, 2)]
        [InlineData(45, 45, 10.1, 10.1, 23.0001, 23.0003, 23.0007, 23.0009, 1, 1)]
        public void GetHashCodeTests_FlashObservationEqual_HashCodesAreEqual(int durationMs1, int durationMs2,
                                                                             double intensity1, double intensity2,
                                                                             double p1XCoordinate, double p1YCoordinate,
                                                                             double p2XCoordinate, double p2YCoordinate,
                                                                             long tiks1, long tiks2)
        {
            // Arange
            var flashObservation1 = new FlashObservation(durationMs1, intensity1, new Coordinates(p1XCoordinate, p1YCoordinate), new DateTime(tiks1));
            var flashObservation2 = new FlashObservation(durationMs2, intensity2, new Coordinates(p2XCoordinate, p2YCoordinate), new DateTime(tiks2));

            // Act
            var hashCode1 = flashObservation1.GetHashCode();
            var hashCode2 = flashObservation2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }
    }
}
