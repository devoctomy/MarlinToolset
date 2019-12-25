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
        public void GivenConnectedPortRef_WhenDisconnect_ThenPortClosed_AndTrueReturned()
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
            var result = sut.Disconnect(portRef);

            // Assert
            Assert.True(result);
            Assert.False(port.IsOpen);
        }

        [Fact]
        public void GivenUnknownPortRef_WhenDisconnect_ThenFalseReturned()
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
            var result = sut.Disconnect(new SerialPortAdapterRef(null, null));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GivenConnectedPortRef_AndString_AndEncoding_WhenWrite_ThenBytesWrittenToPort_AndTrueReturned()
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
            var result = sut.Write(
                portRef,
                "Hello World",
                Encoding.ASCII);

            // Assert
            Assert.True(result);
            Assert.Equal(Encoding.ASCII.GetBytes("Hello World"), actualData);
            Assert.Equal(0, actualOffset);
            Assert.Equal("Hello World".Length, actualCount);
        }

        [Fact]
        public void GivenUnknownPortRef_AndString_AndEncoding_WhenWrite_ThenFalseReturned()
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
            var result = sut.Write(
                new SerialPortAdapterRef(null, null),
                "Hello World",
                Encoding.ASCII);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GivenConnectedPortRef_AndBytes_AndOffset_AndCount_WhenWrite_ThenBytesWrittenToPort_AndTrueReturned()
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
            var result = sut.Write(
                portRef,
                expectedData,
                expectedOffset,
                expectedCount);

            // Assert
            Assert.True(result);
            Assert.Equal(expectedData, actualData);
            Assert.Equal(expectedOffset, actualOffset);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void GivenUnknownPortRef_AndBytes_AndOffset_AndCount_WhenWrite_ThenFalseReturned()
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
            var result = sut.Write(
                new SerialPortAdapterRef(null, null),
                null,
                0,
                0);

            // Assert
            Assert.False(false);
        }

        [Fact]
        public void GivenConnected_AndPortRef_WhenGetSerialPort_ThenCorrectSerialPortReturned()
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

            // Act
            var port = sut.GetSerialPort(portRef);

            // Assert
            Assert.Equal(config.Port, port.PortName);
            Assert.Equal(config.BaudRate, port.BaudRate);
        }

        [Fact]
        public void GivenUnknownPortRef_WhenGetSerialPort_ThenNullReturned()
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
            var port = sut.GetSerialPort(new SerialPortAdapterRef(null, null));

            // Assert
            Assert.Null(port);
        }

        [Fact]
        public void GivenConnected_WhenDispose_ThenAllPortsDisposed()
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

            // Act
            sut.Dispose();

            // Assert
            Assert.Empty(sut.PortRefs);
            Assert.True(port.Disposed);
        }
    }
}
