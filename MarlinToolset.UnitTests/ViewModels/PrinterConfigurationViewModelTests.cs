using MarlinToolset.ViewModels;
using System;
using System.Linq;
using Xunit;

namespace MarlinToolset.UnitTests.ViewModels
{
    public class PrinterConfigurationViewModelTests
    {
        [Fact]
        public void GivenInstance_WhenSave_ThenCancelEventInvoked()
        {
            // Arrange
            var sut = new PrinterConfigurationViewModel();
            var saveEventInvoked = false;
            sut.Saved += delegate
            {
                saveEventInvoked = true;
            };

            // Act
            sut.Save.Execute().Subscribe();

            // Assert
            Assert.True(saveEventInvoked);
        }

        [Fact]
        public void GivenInstance_WhenCancel_ThenCancelEventInvoked()
        {
            // Arrange
            var sut = new PrinterConfigurationViewModel();
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
        public void GivenInstance_WhenBaudRates_ThenCorrectBaudRatesReturned()
        {
            // Arrange
            var expectedBaudRates = new[]
            {
                300,
                600,
                1200,
                2400,
                4800,
                9600,
                19200,
                38400,
                57600,
                115200,
                230400,
                460800,
                921600
            };
            var sut = new PrinterConfigurationViewModel();

            // Act
            var actualBaudRates = sut.BaudRates;

            // Assert
            Assert.True(expectedBaudRates.SequenceEqual(actualBaudRates));
        }
    }
}
