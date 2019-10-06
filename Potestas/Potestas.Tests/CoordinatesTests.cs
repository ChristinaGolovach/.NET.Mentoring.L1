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

        [Theory]
        [InlineData(0.001, 0.001, 0.02, 0.02, 0.021, 0.021)]
        [InlineData(-0.001, 0, 0.02, 1.11, 0.019, 1.11)]
        [InlineData( 23.00, 23.00, 23.00, 23.00, 46.00, 46.00)]
        public void OperatorPlusTests_TwoPoints_TheSumOfPoints(double p1XCoordinate, double p1YCoordinate, 
                                                               double p2XCoordinate, double p2YCoordinate,
                                                               double resultXCoordinate, double resultYCoordinate)
        {
            // Arange
            int precision = 3;
            var point1 = new Coordinates(p1XCoordinate, p1YCoordinate);
            var point2 = new Coordinates(p2XCoordinate, p2YCoordinate);

            // Act
            var actualPoint = point1 + point2;

            // Assert
            Assert.Equal(actualPoint.X, resultXCoordinate, precision);
            Assert.Equal(actualPoint.Y, resultYCoordinate, precision);
        }

        [Theory]
        [InlineData(0.222, 0.222, 0.02, 0.02, 0.202, 0.202)]
        [InlineData(-0.001, 23.12, 0.02, 1.11, -0.021, 22.01)]
        [InlineData(23.000, 23.000, -23.000, 23.000, 46.000, 0.000)]
        public void OperatorMinusTests_TwoPoints_TheSumOfPoints(double p1XCoordinate, double p1YCoordinate,
                                                       double p2XCoordinate, double p2YCoordinate,
                                                       double resultXCoordinate, double resultYCoordinate)
        {
            // Arange
            int precision = 3;
            var point1 = new Coordinates(p1XCoordinate, p1YCoordinate);
            var point2 = new Coordinates(p2XCoordinate, p2YCoordinate);

            // Act
            var actualPoint = point1 - point2;

            // Assert
            Assert.Equal(actualPoint.X, resultXCoordinate, precision);
            Assert.Equal(actualPoint.Y, resultYCoordinate, precision);
        }


        [Theory]
        [InlineData(0.222, 0.222, 0.02, 0.02, false)]
        [InlineData(0.221, 0.222, 0.02, 0.02, false)]
        [InlineData(0.0011, 23.122, 0.0012, 23.1223, true)]
        [InlineData(23.0001, 23.0003, 23.0007, 23.0009, true)]
        public void OperatorEqualityTests_TwoPoints_EqualtyResult(double p1XCoordinate, double p1YCoordinate,
                                                                   double p2XCoordinate, double p2YCoordinate,
                                                                   bool expectedResult)
        {
            // Arange
            var point1 = new Coordinates(p1XCoordinate, p1YCoordinate);
            var point2 = new Coordinates(p2XCoordinate, p2YCoordinate);

            // Act
            var actualResult = point1 == point2;

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(0.222, 0.222, 0.02, 0.02, true)]
        [InlineData(0.221, 0.222, 0.02, 0.02, true)]
        [InlineData(0.0011, 23.122, 0.0012, 23.1223, false)]
        [InlineData(23.0001, 23.0003, 23.0007, 23.0009, false)]
        public void OperatorInequalityTests_TwoPoints_EqualtyResult(double p1XCoordinate, double p1YCoordinate,
                                                                    double p2XCoordinate, double p2YCoordinate,
                                                                    bool expectedResult)
        {
            // Arange
            var point1 = new Coordinates(p1XCoordinate, p1YCoordinate);
            var point2 = new Coordinates(p2XCoordinate, p2YCoordinate);

            // Act
            var actualResult = point1 != point2;

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(0.222, 0.222, 0.02, 0.02, false)]
        [InlineData(0.221, 0.222, 0.02, 0.02, false)]
        [InlineData(0.0011, 23.122, 0.0012, 23.1223, true)]
        [InlineData(23.0001, 23.0003, 23.0007, 23.0009, true)]
        public void EqualsTests_TwoPoints_EqualtyResult(double p1XCoordinate, double p1YCoordinate,
                                                        double p2XCoordinate, double p2YCoordinate,
                                                        bool expectedResult)
        {
            // Arange
            var point1 = new Coordinates(p1XCoordinate, p1YCoordinate);
            var point2 = new Coordinates(p2XCoordinate, p2YCoordinate);

            // Act
            var actualResult = point1.Equals(point2);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(0.0011, 23.122, 0.0012, 23.1223)]
        [InlineData(23.0001, 23.0003, 23.0007, 23.0009)]
        public void GetHashCodeTests_TwoPointsEqualWithThousandPrecision_HashCodesAreEqual(double p1XCoordinate, double p1YCoordinate,
                                                                                           double p2XCoordinate, double p2YCoordinate)
        {
            // Arange
            var point1 = new Coordinates(p1XCoordinate, p1YCoordinate);
            var point2 = new Coordinates(p2XCoordinate, p2YCoordinate);

            // Act
            var point1HashCode = point1.GetHashCode();
            var point2HashCode = point2.GetHashCode();

            // Assert
            Assert.Equal(point1HashCode, point2HashCode);
        }

        [Theory]
        [InlineData(0.222, 0.222, 0.02, 0.02)]
        [InlineData(0.221, 0.222, 0.02, 0.02)]
        public void GetHashCodeTests_TwoPointsNotEqualWithThousandPrecision_HashCodesAreNotEqual(double p1XCoordinate, double p1YCoordinate,
                                                                                                 double p2XCoordinate, double p2YCoordinate)
        {
            // Arange
            var point1 = new Coordinates(p1XCoordinate, p1YCoordinate);
            var point2 = new Coordinates(p2XCoordinate, p2YCoordinate);

            // Act
            var point1HashCode = point1.GetHashCode();
            var point2HashCode = point2.GetHashCode();

            // Assert
            Assert.NotEqual(point1HashCode, point2HashCode);
        }
    }
}
