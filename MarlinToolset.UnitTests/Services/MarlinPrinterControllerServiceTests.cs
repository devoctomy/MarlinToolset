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
        public void GivenConnectedPrinterControllerService_AndString_WhenWrite_ThenStringWrittenToPrinter()
        {
            // Arrange
            var printer = new PrinterConfigurationModel();
            var testableSerialPortAdapter = new TestableSerialPortAdapter();
            var mockPrinterPacketParser = new Mock<IPrinterPacketParser>();
            var sut = new MarlinPrinterControllerService(
                testableSerialPortAdapter,
                mockPrinterPacketParser.Object);
            var expecetdData = "Hello World!";
            var expectedEncoding = Encoding.ASCII;
            sut.Connect(printer);

            // Act
            sut.Write(
                expecetdData,
                expectedEncoding);

            // Assert
            Assert.Single(testableSerialPortAdapter.WrittenStringData);
            Assert.Equal(expecetdData, testableSerialPortAdapter.WrittenStringData[0].Data);
            Assert.Equal(expectedEncoding, testableSerialPortAdapter.WrittenStringData[0].Encoding);
        }

        [Fact]
        public void GivenConnectedPrinterControllerService_AndByteArray_WhenWrite_ThenByteArrayWrittenToPrinter()
        {
            // Arrange
            var printer = new PrinterConfigurationModel();
            var mockPrinterPacketParser = new Mock<IPrinterPacketParser>();
            var testableSerialPortAdapter = new TestableSerialPortAdapter();
            var sut = new MarlinPrinterControllerService(
                testableSerialPortAdapter,
                mockPrinterPacketParser.Object);
            var expectedDataString = "Hello World!";
            var expecetdDataBytes = Encoding.ASCII.GetBytes(expectedDataString);
            var expectedOffset = 1;
            var expectedCount = 2;
            sut.Connect(printer);

            // Act
            sut.Write(
                expecetdDataBytes,
                expectedOffset,
                expectedCount);

            // Assert
            Assert.Single(testableSerialPortAdapter.WrittenBinaryData);
            Assert.Equal(expecetdDataBytes, testableSerialPortAdapter.WrittenBinaryData[0].Data);
            Assert.Equal(expectedOffset, testableSerialPortAdapter.WrittenBinaryData[0].Offset);
            Assert.Equal(expectedCount, testableSerialPortAdapter.WrittenBinaryData[0].Count);
        }
    }
}
