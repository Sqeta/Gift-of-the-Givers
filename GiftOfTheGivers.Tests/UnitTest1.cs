namespace GiftOfTheGivers.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void BasicTest_ShouldPass()
        {
            // Arrange
            int expected = 10;

            // Act
            int actual = 5 + 5;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}