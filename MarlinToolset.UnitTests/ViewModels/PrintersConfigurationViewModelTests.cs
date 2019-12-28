using MarlinToolset.Core.Services;
using MarlinToolset.Domain.Model;
using MarlinToolset.ViewModels;
using MarlinToolset.Views;
using Moq;
using System;
using Xunit;

namespace MarlinToolset.UnitTests.ViewModels
{
    public class PrintersConfigurationViewModelTests
    {
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Mock<IPrinterConfigurationView> _mockPrinterConfigurationView;
        private readonly Mock<IPrinterConfigurationManagerService> _mockPrinterConfigurationManagerService;

        public PrintersConfigurationViewModelTests()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockPrinterConfigurationView = new Mock<IPrinterConfigurationView>();
            _mockPrinterConfigurationManagerService = new Mock<IPrinterConfigurationManagerService>();
        }

        [Fact]
        public void GivenOk_WhenAdd_ThenDialogShown_AndPrinterAdded()
        {
            // Arrange
            var model = new PrinterConfigurationModel();
            var sut = new PrintersConfigurationViewModel(
                _mockServiceProvider.Object,
                _mockPrinterConfigurationManagerService.Object);

            _mockPrinterConfigurationView.Setup(x => x.ShowDialog())
                .Returns(true);

            _mockServiceProvider.Setup(x => x.GetService(typeof(IPrinterConfigurationView)))
                .Returns(_mockPrinterConfigurationView.Object);

            _mockPrinterConfigurationView.SetupGet<PrinterConfigurationViewModel>(x => x.ViewModel)
                .Returns(new PrinterConfigurationViewModel() { Model = model });

            // Act
            sut.Add.Execute().Subscribe();

            // Assert
            _mockPrinterConfigurationManagerService.Verify(x => x.Add(
                It.Is<PrinterConfigurationModel>(x => x == model)), Times.Once);
        }

        [Fact]
        public void GivenCancel_WhenAdd_ThenDialogShown_AndPrinterNotAdded()
        {
            // Arrange
            var model = new PrinterConfigurationModel();
            var sut = new PrintersConfigurationViewModel(
                _mockServiceProvider.Object,
                _mockPrinterConfigurationManagerService.Object);

            _mockPrinterConfigurationView.Setup(x => x.ShowDialog())
                .Returns(false);

            _mockServiceProvider.Setup(x => x.GetService(typeof(IPrinterConfigurationView)))
                .Returns(_mockPrinterConfigurationView.Object);

            _mockPrinterConfigurationView.SetupGet<PrinterConfigurationViewModel>(x => x.ViewModel)
                .Returns(new PrinterConfigurationViewModel() { Model = model });

            // Act
            sut.Add.Execute().Subscribe();

            // Assert
            _mockPrinterConfigurationManagerService.Verify(x => x.Add(
                It.Is<PrinterConfigurationModel>(x => x == model)), Times.Never);
        }

        [Fact]
        public void GivenInstance_WhenClear_ThenPrintersCleared()
        {
            // Arrange
            var sut = new PrintersConfigurationViewModel(
                null,
                _mockPrinterConfigurationManagerService.Object);

            // Act
            sut.Clear.Execute().Subscribe();

            // Assert
            _mockPrinterConfigurationManagerService.Verify(x => x.Clear(), Times.Once);
        }

        [Fact]
        public void GivenInstance_WhenSave_ThenPrintersSaved_AndSavedEventInvoked()
        {
            // Arrange
            var sut = new PrintersConfigurationViewModel(
                null,
                _mockPrinterConfigurationManagerService.Object);
            var saveEventInvoked = false;
            sut.Saved += delegate
            {
                saveEventInvoked = true;
            };

            // Act
            sut.Save.Execute().Subscribe();

            // Assert
            _mockPrinterConfigurationManagerService.Verify(x => x.Save(), Times.Once);
            Assert.True(saveEventInvoked);
        }

        [Fact]
        public void GivenInstance_WhenCancel_ThenCancelEventInvoked()
        {
            // Arrange
            var sut = new PrintersConfigurationViewModel(
                null,
                _mockPrinterConfigurationManagerService.Object);
            var cancelEventInvoked = false;
            sut.Cancelled += delegate
            {
                cancelEventInvoked = true;
            };

            // Act
            sut.Cancel.Execute().Subscribe();

            // Assert
            Assert.True(cancelEventInvoked);
        }

        [Fact]
        public void GivenInstance_AndNoSelectedPrinter_WhenRemove_ThenNoPrinterRemoved()
        {
            // Arrange
            var sut = new PrintersConfigurationViewModel(
                null,
                _mockPrinterConfigurationManagerService.Object);

            // Act
            sut.Remove.Execute().Subscribe();

            // Assert
            _mockPrinterConfigurationManagerService.Verify(x => x.Remove(
                It.IsAny<PrinterConfigurationModel>()), Times.Never);
        }

        [Fact]
        public void GivenInstance_AndSelectedPrinter_WhenRemove_ThenPrinterRemoved_AndPrinterDeselected()
        {
            // Arrange
            var printer = new PrinterConfigurationModel();
            var sut = new PrintersConfigurationViewModel(
                null,
                _mockPrinterConfigurationManagerService.Object)
            {
                SelectedPrinter = printer
            };

            // Act
            sut.Remove.Execute().Subscribe();

            // Assert
            _mockPrinterConfigurationManagerService.Verify(x => x.Remove(
                It.Is<PrinterConfigurationModel>(x => x == printer)), Times.Once);
            Assert.Null(sut.SelectedPrinter);
        }
    }
}
