using MarlinToolset.Model;
using MarlinToolset.Services;
using System;
using System.Text;
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

        [Fact]
        public void GivenConnectedPortRef_AndString_AndEncoding_WhenWrite_ThenBytesWrittenToPort()
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
            var port = (TestableSerialPort)sut.GetSerialPort(portRef);
            var actualData = default(byte[]);
            var actualOffset = 0;
            var actualCount = 0;
            port.WriteCallback = delegate (byte[] data, int offset, int count)
            {
                actualData = data;
                actualOffset = offset;
                actualCount = count;
            };

            // Act
            sut.Write(
                portRef,
                "Hello World",
                Encoding.ASCII);

            // Assert
            Assert.Equal(Encoding.ASCII.GetBytes("Hello World"), actualData);
            Assert.Equal(0, actualOffset);
            Assert.Equal("Hello World".Length, actualCount);
        }

        [Fact]
        public void GivenConnectedPortRef_AndBytes_AndOffset_AndCount_WhenWrite_ThenBytesWrittenToPort()
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
            var port = (TestableSerialPort)sut.GetSerialPort(portRef);
            var expectedData = Encoding.ASCII.GetBytes("Hello World");
            int expectedOffset = 0;
            int expectedCount = expectedData.Length;
            var actualData = default(byte[]);
            var actualOffset = 0;
            var actualCount = 0;
            port.WriteCallback = delegate (byte[] data, int offset, int count)
            {
                actualData = data;
                actualOffset = offset;
                actualCount = count;
            };

            // Act
            sut.Write(
                portRef,
                expectedData,
                expectedOffset,
                expectedCount);

            // Assert
            Assert.Equal(expectedData, actualData);
            Assert.Equal(expectedOffset, actualOffset);
            Assert.Equal(expectedCount, actualCount);
        }
    }
}
