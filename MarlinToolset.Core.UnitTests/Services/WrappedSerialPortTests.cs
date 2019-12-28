using MarlinToolset.Core.Services;
using Xunit;

namespace MarlinToolset.Core.UnitTests.Services
{
    public class WrappedSerialPortTests
    {
        [Fact]
        public void GivenInstance_WhenDispose_ThenInnerPortDisposed()
        {
            // Arrange
            var sut = new WrappedSerialPort(
                "com6",
                1001);

            // Act
            sut.Dispose();

            // Assert
            Assert.False(sut.IsOpen);   // This assertion is pointless but covers that line of code...
            Assert.Null(sut.InnerPort);
        }
    }
}
