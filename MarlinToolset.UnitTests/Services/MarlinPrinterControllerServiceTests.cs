using MarlinToolset.Model;
using MarlinToolset.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MarlinToolset.UnitTests.Services
{
    public class MarlinPrinterControllerServiceTests
    {
        [Fact]
        public void GivenPrinterConfigurationModel_AndConfigurationCorrect_WhenConnect_ThenSerialPortAdapterConnectCalled_AndPrinterSet_AndSerialPortAdapterRefSet()
        {
            // Arrange
            var printer = new PrinterConfigurationModel()
            {
                Name = "Some wizzy printer",
                Port = "com9",
                BaudRate = 1001
            };
            var mockSerialPortAdapter = new Mock<ISerialPortAdapter>();
            var mockPrinterPacketParser = new Mock<IPrinterPacketParser>();

            mockSerialPortAdapter.Setup(x => x.Connect(
                It.IsAny<PrinterConfigurationModel>(),
                It.IsAny<Action<SerialPortAdapterRef, string>>()))
                .Returns(new SerialPortAdapterRef(printer, null));

            var sut = new MarlinPrinterControllerService(
                mockSerialPortAdapter.Object,
                mockPrinterPacketParser.Object);

            // Act
            sut.Connect(printer);

            // Assert
            mockSerialPortAdapter.Verify(x => x.Connect(
                It.Is<PrinterConfigurationModel>(x => x == printer),
                It.IsAny<Action<SerialPortAdapterRef, string>>()), Times.Once);
            Assert.Equal(printer, sut.Printer);
            Assert.Equal(printer, sut.SerialPortAdapterRef.Config);
        }

        [Fact]
        public void GivenConnectedPrinterControllerService_WhenDisconnect_ThenSerialPortAdapterDisonnectCalled_AndPrinterUnSet_AndSerialPortAdapterRefUnSet()
        {
            // Arrange
            var printer = new PrinterConfigurationModel();
            var mockSerialPortAdapter = new Mock<ISerialPortAdapter>();
            var mockPrinterPacketParser = new Mock<IPrinterPacketParser>();

            mockSerialPortAdapter.Setup(x => x.Connect(
                It.IsAny<PrinterConfigurationModel>(),
                It.IsAny<Action<SerialPortAdapterRef, string>>()))
                .Returns(new SerialPortAdapterRef(printer, null));

            var sut = new MarlinPrinterControllerService(
                mockSerialPortAdapter.Object,
                mockPrinterPacketParser.Object);
            sut.Connect(printer);

            // Act
            sut.Disconnect();

            // Assert
            mockSerialPortAdapter.Verify(x => x.Disconnect(
                It.IsAny<SerialPortAdapterRef>()), Times.Once);
            Assert.Null(sut.Printer);
            Assert.Null(sut.SerialPortAdapterRef);
        }

        [Fact]
        public void GivenConnectedPrinterControllerService_WhenReceivedData_ThenEventRaisedWithReceivedData()
        {
            // Arrange
            var printer = new PrinterConfigurationModel();
            var testableSerialPortAdapter = new TestableSerialPortAdapter();
            var mockPrinterPacketParser = new Mock<IPrinterPacketParser>();
            var sut = new MarlinPrinterControllerService(
                testableSerialPortAdapter,
                mockPrinterPacketParser.Object);
            var receivedData = string.Empty;
            var expecetdData = "Hello World!";
            sut.Connect(printer);

            mockPrinterPacketParser.Setup(x => x.ReceiveData(
                It.IsAny<string>())).Callback((string data) =>
                {
                    var eventArgs = new PrinterPacketParserPacketCompleteEventArgs()
                    {
                        Packet = new PrinterPacket()
                        {
                            RawData = data
                        }
                    };
                    mockPrinterPacketParser.Raise(
                        x => x.PacketComplete += null,
                        eventArgs);
                });

            // Act
            sut.ReceivedData += new EventHandler<PrinterControllerReceivedDataEventArgs>(delegate (object s, PrinterControllerReceivedDataEventArgs ev)
            {
                receivedData = ev.Packet.RawData;
            });
            testableSerialPortAdapter.FakeReceiveData(
                testableSerialPortAdapter.SerialPortAdapterRef,
                expecetdData);

            // Assert
            Assert.Equal(expecetdData, receivedData);
        }

        [Fact]
        public void GivenConnectedPrinterControllerService_AndByteArray_AndOffset_AndCount_WhenWrite_ThenByteArrayWrittenToPrinter_AndPrinterCommandReturned()
        {
            // Arrange
            var printer = new PrinterConfigurationModel();
            var mockPrinterPacketParser = new Mock<IPrinterPacketParser>();
            var testableSerialPortAdapter = new TestableSerialPortAdapter();
            var sut = new MarlinPrinterControllerService(
                testableSerialPortAdapter,
                mockPrinterPacketParser.Object);
            var expectedDataString = "Hello World!";
            var expectedDataBytes = Encoding.ASCII.GetBytes(expectedDataString);
            var expectedOffset = 1;
            var expectedCount = 2;
            sut.Connect(printer);

            // Act
            var result = sut.Write(
                expectedDataBytes,
                expectedOffset,
                expectedCount);

            // Assert
            Assert.Equal(expectedDataBytes, result.Data);
            Assert.Equal(expectedOffset, result.Offset);
            Assert.Equal(expectedCount, result.Count);
            Assert.False(result.Acknowledged);
            Assert.Single(testableSerialPortAdapter.WrittenBinaryData);
            Assert.Equal(expectedDataBytes, testableSerialPortAdapter.WrittenBinaryData[0].Data);
            Assert.Equal(expectedOffset, testableSerialPortAdapter.WrittenBinaryData[0].Offset);
            Assert.Equal(expectedCount, testableSerialPortAdapter.WrittenBinaryData[0].Count);
        }

        [Fact]
        public void GivenConnectedPrinterControllerService_AndString_AndEncoding_WhenWrite_ThenByteArrayWrittenToPrinter_AndPrinterCommandReturned()
        {
            // Arrange
            var printer = new PrinterConfigurationModel();
            var mockPrinterPacketParser = new Mock<IPrinterPacketParser>();
            var testableSerialPortAdapter = new TestableSerialPortAdapter();
            var sut = new MarlinPrinterControllerService(
                testableSerialPortAdapter,
                mockPrinterPacketParser.Object);
            var expectedDataString = "Hello World!";
            var expectedDataBytes = Encoding.ASCII.GetBytes(expectedDataString);
            var expectedOffset = 0;
            var expectedCount = expectedDataString.Length;
            sut.Connect(printer);

            // Act
            var result = sut.Write(
                expectedDataString,
                Encoding.ASCII);

            // Assert
            Assert.Equal(expectedDataBytes, result.Data);
            Assert.Equal(expectedOffset, result.Offset);
            Assert.Equal(expectedCount, result.Count);
            Assert.False(result.Acknowledged);
            Assert.Single(testableSerialPortAdapter.WrittenBinaryData);
            Assert.Equal(expectedDataBytes, testableSerialPortAdapter.WrittenBinaryData[0].Data);
            Assert.Equal(expectedOffset, testableSerialPortAdapter.WrittenBinaryData[0].Offset);
            Assert.Equal(expectedCount, testableSerialPortAdapter.WrittenBinaryData[0].Count);
        }

        [Fact]
        public void GivenConnectedPrinterControllerService_AndPrevCommandWaitingAck_AndString_AndEncoding_WhenWrite_ThenNullReturned()
        {
            // Arrange
            var printer = new PrinterConfigurationModel();
            var mockPrinterPacketParser = new Mock<IPrinterPacketParser>();
            var testableSerialPortAdapter = new TestableSerialPortAdapter();
            var sut = new MarlinPrinterControllerService(
                testableSerialPortAdapter,
                mockPrinterPacketParser.Object);
            var expectedDataString = "Hello World!";
            var expectedDataBytes = Encoding.ASCII.GetBytes(expectedDataString);
            var expectedOffset = 0;
            var expectedCount = expectedDataString.Length;
            sut.Connect(printer);
            sut.Write(
                expectedDataString,
                Encoding.ASCII);

            // Act
            var result = sut.Write(
                "This won't get sent!",
                Encoding.ASCII);

            // Assert
            Assert.Null(result);
            Assert.Single(testableSerialPortAdapter.WrittenBinaryData);
            Assert.Equal(expectedDataBytes, testableSerialPortAdapter.WrittenBinaryData[0].Data);
            Assert.Equal(expectedOffset, testableSerialPortAdapter.WrittenBinaryData[0].Offset);
            Assert.Equal(expectedCount, testableSerialPortAdapter.WrittenBinaryData[0].Count);
        }

        [Fact]
        public void GivenConnectedPrinterControllerService_AndCommandAcked_AndString_AndEncoding_WhenWrite_ThenCommandSent()
        {
            // Arrange
            var printer = new PrinterConfigurationModel();
            var mockPrinterPacketParser = new Mock<IPrinterPacketParser>();
            var testableSerialPortAdapter = new TestableSerialPortAdapter();
            var sut = new MarlinPrinterControllerService(
                testableSerialPortAdapter,
                mockPrinterPacketParser.Object);
            var expectedDataString = "Hello World!";
            var expectedDataBytes = Encoding.ASCII.GetBytes(expectedDataString);
            var expectedOffset = 0;
            var expectedCount = expectedDataString.Length;
            sut.Connect(printer);
            sut.Write(
                expectedDataString,
                Encoding.ASCII);

            mockPrinterPacketParser.Setup(x => x.ReceiveData(
                It.Is<string>(x => x == "ok\n")))
                .Callback(() =>
                {
                    mockPrinterPacketParser.Raise(
                        x => x.PacketComplete += null,
                        new PrinterPacketParserPacketCompleteEventArgs()
                        {
                            Packet = new PrinterPacket()
                            {
                                IsAck = true
                            }
                        });
                });

            sut.SerialPortAdapterRef.DataReceivedCallback(
                sut.SerialPortAdapterRef,
                "ok\n");

            // Act
            var result = sut.Write(
                "This will get sent as the previous command was acknowledged!",
                Encoding.ASCII);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, testableSerialPortAdapter.WrittenBinaryData.Count);
        }

        [Fact]
        public void GivenConnectedPrinterControllerService_AndPrevCommandWaitingAck_AndCommandStackCleared_AndString_AndEncoding_WhenWrite_ThenCommandSent()
        {
            // Arrange
            var printer = new PrinterConfigurationModel();
            var mockPrinterPacketParser = new Mock<IPrinterPacketParser>();
            var testableSerialPortAdapter = new TestableSerialPortAdapter();
            var sut = new MarlinPrinterControllerService(
                testableSerialPortAdapter,
                mockPrinterPacketParser.Object);
            var expectedDataString = "Hello World!";
            var expectedDataBytes = Encoding.ASCII.GetBytes(expectedDataString);
            var expectedOffset = 0;
            var expectedCount = expectedDataString.Length;
            sut.Connect(printer);
            sut.Write(
                expectedDataString,
                Encoding.ASCII);
            sut.ClearCommandStack();

            // Act
            var result = sut.Write(
                "This will get sent as the command stack has been cleared!",
                Encoding.ASCII);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, testableSerialPortAdapter.WrittenBinaryData.Count);
        }
    }
}
