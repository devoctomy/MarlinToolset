using MarlinToolset.Model;
using MarlinToolset.Services;
using System;
using Xunit;

namespace MarlinToolset.UnitTests.Services
{
    public class SerialPortAdapterTests
    {
        [Fact]
        public void GivenConfig_AndCallback_WhenConnect_ThenPortCreated_AndPortOpened_AndRefReturned()
        {
            // Arrange
            var config = new PrinterConfigurationModel()
            {
                Port = "com9",
                BaudRate = 1001
            };
            Action<SerialPortAdapterRef, string> callback = delegate (SerialPortAdapterRef portRef, string data)
            {
            };
            var sut = new SerialPortAdapter<TestableSerialPort>();

            // Act
            var result = sut.Connect(
                config,
                callback);

            // Assert
            Assert.Equal(config, result.Config);
            Assert.Equal(callback, result.DataReceivedCallback);
            Assert.True(sut.GetSerialPort(result).IsOpen);
        }

        [Fact]
        public void GivenConnected_AndPortRef_WhenDisconnect_ThenPortClosed()
        {
            // Arrange
            var config = new PrinterConfigurationModel()
            {
                Port = "com9",
                BaudRate = 1001
            };
            Action<SerialPortAdapterRef, string> callback = delegate (SerialPortAdapterRef portRef, string data)
            {
            };
            var sut = new SerialPortAdapter<TestableSerialPort>();
            var portRef = sut.Connect(
                config,
                callback);
            var port = sut.GetSerialPort(portRef);

            // Act
            sut.Disconnect(portRef);

            // Assert
            Assert.False(port.IsOpen);
        }
    }
}
