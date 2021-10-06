using Domain.Services;
using Xunit;

namespace Domain.UnitTests.Services
{
    public class Calculator_Should
    {
        [Theory]
        [InlineData(3, 4, 7)]
        [InlineData(31, 43, 74)]
        [InlineData(12, 13, 25)]
        [InlineData(int.MinValue, int.MaxValue, -1)]
        [InlineData(int.MaxValue, 1, -2147483648)]
        [InlineData(int.MinValue, -1, 2147483647)]
        public void Add_TwoNumbers(int a, int b, int expectedResult)
        {
            // Arrange
            var sut = new Calculator();
            
            // Act
            var result = sut.Add(a, b);
            
            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}