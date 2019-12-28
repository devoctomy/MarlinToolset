using MarlinToolset.Core.Services;
using System.Collections.Generic;
using Xunit;

namespace MarlinToolset.Core.UnitTests.Services
{
    public class MarlinPrinterPacketParserTests
    {
        [Fact]
        public void GivenSplitPacketData_WhenReceiveData_ThenCorrectCompletePacketsReceived()
        {
            // Arrange
            var options = new MarlinPrinterPacketParserOptions();
            var sut = new MarlinPrinterPacketParser(options);
            var actualPackets = new List<PrinterPacket>();

            var packetData = new[]
            {
                "Hel",
                "lo World!\n",
                "Hello World!\n",
                "Hello",
                " World!\n"
            };

            sut.PacketComplete += (object s, PrinterPacketParserPacketCompleteEventArgs e) =>
            {
                actualPackets.Add(e.Packet);
            };

            // Act
            foreach(var curDataLine in packetData)
            {
                sut.ReceiveData(curDataLine);
            }

            // Assert
            Assert.Equal(3, actualPackets.Count);
            Assert.Equal("Hello World!", actualPackets[0].RawData);
            Assert.Equal("Hello World!", actualPackets[1].RawData);
            Assert.Equal("Hello World!", actualPackets[2].RawData);
        }

        [Fact]
        public void GivenCompletePacketData_WhenReceiveData_ThenEchoRemovedFromPreProcessedData()
        {
            // Arrange
            var options = new MarlinPrinterPacketParserOptions();
            var sut = new MarlinPrinterPacketParser(options);
            var actualPackets = new List<PrinterPacket>();

            var packetData = new[]
            {
                "echo:Hello World!\n",
                "echo:Hello World!\n",
                "echo:Hello World!\n"
            };

            sut.PacketComplete += (object s, PrinterPacketParserPacketCompleteEventArgs e) =>
            {
                actualPackets.Add(e.Packet);
            };

            // Act
            foreach (var curDataLine in packetData)
            {
                sut.ReceiveData(curDataLine);
            }

            // Assert
            Assert.Equal(3, actualPackets.Count);
            Assert.Equal("echo:Hello World!", actualPackets[0].RawData);
            Assert.Equal("echo:Hello World!", actualPackets[1].RawData);
            Assert.Equal("echo:Hello World!", actualPackets[2].RawData);
            Assert.Equal("Hello World!", actualPackets[0].PreProcessedData);
            Assert.Equal("Hello World!", actualPackets[1].PreProcessedData);
            Assert.Equal("Hello World!", actualPackets[2].PreProcessedData);
        }
    }
}
