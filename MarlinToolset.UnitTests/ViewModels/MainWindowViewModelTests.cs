using MarlinToolset.Core.Services;
using MarlinToolset.Domain.Model;
using MarlinToolset.ViewModels;
using MarlinToolset.Views;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MarlinToolset.UnitTests.ViewModels
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Mock<IPrinterConfigurationManagerService> _mockPrinterConfigurationManagerService;
        private readonly Mock<IPrinterControllerService> _mockPrinterControllerSevice;

        public MainWindowViewModelTests()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockPrinterConfigurationManagerService = new Mock<IPrinterConfigurationManagerService>();
            _mockPrinterControllerSevice = new Mock<IPrinterControllerService>();
        }

        [Fact]
        public void GivenCancel_WhenConfigurePrinters_ThenDialogShown()
        {
            // Arrange
            var mockPrintersConfigurationView = new Mock<IPrintersConfigurationView>();
            var sut = new MainWindowViewModel(
                _mockServiceProvider.Object,
                _mockPrinterConfigurationManagerService.Object,
                _mockPrinterControllerSevice.Object,
                null,
                null);

            mockPrintersConfigurationView.Setup(x => x.ShowDialog())
                .Returns(false);

            _mockServiceProvider.Setup(x => x.GetService(typeof(IPrintersConfigurationView)))
                .Returns(mockPrintersConfigurationView.Object);

            // Act
            sut.ConfigurePrinters.Execute().Subscribe();

            // Assert
            mockPrintersConfigurationView.Verify(x => x.ShowDialog(), Times.Once);
        }

        [Fact]
        public void GivenPrinterSelected_AndPrinterRemoved_AndOk_WhenConfigurePrinters_ThenDialogShown_AndSelectedPrinterUnset()
        {
            // Arrange
            var mockPrintersConfigurationView = new Mock<IPrintersConfigurationView>();
            var sut = new MainWindowViewModel(
                _mockServiceProvider.Object,
                _mockPrinterConfigurationManagerService.Object,
                _mockPrinterControllerSevice.Object,
                null,
                null)
            {
                SelectedPrinter = new PrinterConfigurationModel()
            };

            _mockPrinterConfigurationManagerService.SetupGet<PrintersConfigurationModel>(x => x.Config)
                .Returns(new PrintersConfigurationModel());

            mockPrintersConfigurationView.Setup(x => x.ShowDialog())
                .Returns(true);

            _mockServiceProvider.Setup(x => x.GetService(typeof(IPrintersConfigurationView)))
                .Returns(mockPrintersConfigurationView.Object);

            // Act
            sut.ConfigurePrinters.Execute().Subscribe();

            // Assert
            mockPrintersConfigurationView.Verify(x => x.ShowDialog(), Times.Once);
            Assert.Null(sut.SelectedPrinter);
        }

        [Fact]
        public void GivenPrinterSelected_AndConnected_WhenConnectToggle_ThenPrinterDisconnected()
        {
            // Arrange
            var mockPrintersConfigurationView = new Mock<IPrintersConfigurationView>();
            var sut = new MainWindowViewModel(
                _mockServiceProvider.Object,
                _mockPrinterConfigurationManagerService.Object,
                _mockPrinterControllerSevice.Object,
                null,
                null)
            {
                SelectedPrinter = new PrinterConfigurationModel()
            };

            _mockPrinterControllerSevice.SetupGet<bool>(x => x.IsConnected)
                .Returns(true);

            // Act
            sut.ConnectToggle.Execute().Subscribe();

            // Assert
            _mockPrinterControllerSevice.Verify(x => x.Disconnect(), Times.Once);
        }

        [Fact]
        public void GivenPrinterSelected_AndNotConnected_WhenConnectToggle_ThenPrinterConnected()
        {
            // Arrange
            var mockPrintersConfigurationView = new Mock<IPrintersConfigurationView>();
            var sut = new MainWindowViewModel(
                _mockServiceProvider.Object,
                _mockPrinterConfigurationManagerService.Object,
                _mockPrinterControllerSevice.Object,
                null,
                null)
            {
                SelectedPrinter = new PrinterConfigurationModel()
            };

            _mockPrinterControllerSevice.SetupGet<bool>(x => x.IsConnected)
                .Returns(false);

            // Act
            sut.ConnectToggle.Execute().Subscribe();

            // Assert
            _mockPrinterControllerSevice.Verify(x => x.Connect(
                It.Is<PrinterConfigurationModel>(x => x == sut.SelectedPrinter)), Times.Once);
        }

        [StaFact]
        public void GivenPrinterControllerReceivedDataEventArgs_WhenEventHandled_ThenPacketAddedToList()
        {
            // Arrange
            var mockPrintersConfigurationView = new Mock<IPrintersConfigurationView>();
            var sut = new MainWindowViewModel(
                _mockServiceProvider.Object,
                _mockPrinterConfigurationManagerService.Object,
                _mockPrinterControllerSevice.Object,
                null,
                null)
            {
                SelectedPrinter = new PrinterConfigurationModel()
            };
            sut.TerminalListBox = new System.Windows.Controls.ListBox();

            var packets = new PrinterControllerReceivedDataEventArgs[]
            {
                new PrinterControllerReceivedDataEventArgs()
                {
                    Packet = new PrinterPacket()
                },
                new PrinterControllerReceivedDataEventArgs()
                {
                    Packet = new PrinterPacket()
                },
                new PrinterControllerReceivedDataEventArgs()
                {
                    Packet = new PrinterPacket()
                }
            };

            // Act
            foreach(var curPacket in packets)
            {
                _mockPrinterControllerSevice.Raise(
                    x => x.ReceivedData += null,
                    curPacket);
            }

            // Assert
            Equals(3, sut.Packets.Count);
        }

        [Fact]
        public void GivenSelectedConnectedPrinter_AndCommandText_WhenSend_ThenCommandSent_AndCommandAddedToHistory()
        {
            // Arrange
            var mockPrintersConfigurationView = new Mock<IPrintersConfigurationView>();
            var sut = new MainWindowViewModel(
                _mockServiceProvider.Object,
                _mockPrinterConfigurationManagerService.Object,
                _mockPrinterControllerSevice.Object,
                null,
                null)
            {
                SelectedPrinter = new PrinterConfigurationModel()
            };
            var expectedData = "Hello World!";
            sut.CommandText = expectedData;
            var actualData = string.Empty;
            var actualEncoding = default(Encoding);

            _mockPrinterControllerSevice.SetupGet<bool>(x => x.IsConnected)
                .Returns(true);

            _mockPrinterControllerSevice.Setup(x => x.Write(
                It.IsAny<string>(),
                It.IsAny<Encoding>()))
                .Callback((string data, Encoding encoding) =>
                {
                    actualData = data;
                    actualEncoding = encoding;
                });

            // Act
            sut.Send.Execute().Subscribe();

            // Assert
            Assert.Equal($"{expectedData}\n", actualData);
            Assert.Single(sut.CommandHistory);
        }

        [Fact]
        public void GivenCommandHistory_WhenPreviousCommand_ThenCommandTextSetToPreviousCommand()
        {
            // Arrange
            var mockPrintersConfigurationView = new Mock<IPrintersConfigurationView>();
            var sut = new MainWindowViewModel(
                _mockServiceProvider.Object,
                _mockPrinterConfigurationManagerService.Object,
                _mockPrinterControllerSevice.Object,
                null,
                null)
            {
                SelectedPrinter = new PrinterConfigurationModel()
            };
            _mockPrinterControllerSevice.SetupGet<bool>(x => x.IsConnected)
                .Returns(true);

            sut.CommandText = "1";
            sut.Send.Execute().Subscribe();
            sut.CommandText = "2";
            sut.Send.Execute().Subscribe();
            sut.CommandText = "3";
            sut.Send.Execute().Subscribe();

            // Act
            sut.PreviousCommand.Execute().Subscribe();
            var prev1 = sut.CommandText;
            sut.PreviousCommand.Execute().Subscribe();
            var prev2 = sut.CommandText;
            sut.PreviousCommand.Execute().Subscribe();
            var prev3 = sut.CommandText;

            // Assert
            Assert.Equal("3", prev1);
            Assert.Equal("2", prev2);
            Assert.Equal("1", prev3);
        }

        [Fact]
        public void GivenCommandHistory_AndAtTopOfHistory_WhenNextCommand_ThenCommandTextSetToNextCommand()
        {
            // Arrange
            var mockPrintersConfigurationView = new Mock<IPrintersConfigurationView>();
            var sut = new MainWindowViewModel(
                _mockServiceProvider.Object,
                _mockPrinterConfigurationManagerService.Object,
                _mockPrinterControllerSevice.Object,
                null,
                null)
            {
                SelectedPrinter = new PrinterConfigurationModel()
            };
            _mockPrinterControllerSevice.SetupGet<bool>(x => x.IsConnected)
                .Returns(true);

            sut.CommandText = "1";
            sut.Send.Execute().Subscribe();
            sut.CommandText = "2";
            sut.Send.Execute().Subscribe();
            sut.CommandText = "3";
            sut.Send.Execute().Subscribe();
            sut.PreviousCommand.Execute().Subscribe();
            sut.PreviousCommand.Execute().Subscribe();
            sut.PreviousCommand.Execute().Subscribe();

            // Act
            var cmd1 = sut.CommandText;
            sut.NextCommand.Execute().Subscribe();
            var cmd2 = sut.CommandText;
            sut.NextCommand.Execute().Subscribe();
            var cmd3 = sut.CommandText;

            // Assert
            Assert.Equal("1", cmd1);
            Assert.Equal("2", cmd2);
            Assert.Equal("3", cmd3);
        }



    }
}
