using MarlinToolset.Model;
using MarlinToolset.Services;
using MarlinToolset.ViewModels;
using MarlinToolset.Views;
using Moq;
using System;
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
                _mockPrinterControllerSevice.Object);

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
                _mockPrinterControllerSevice.Object)
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
                _mockPrinterControllerSevice.Object)
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
                _mockPrinterControllerSevice.Object)
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
                _mockPrinterControllerSevice.Object)
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
    }
}
