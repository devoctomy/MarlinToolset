using MarlinToolset.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MarlinToolset.UnitTests.Services
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
            Assert.Null(sut.InnerPort);
        }
    }
}
