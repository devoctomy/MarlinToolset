using MarlinToolset.Model;
using MarlinToolset.Services;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MarlinToolset.UnitTests.Services
{
    public class PrinterConfigurationManagerServiceTests
    {
        [Fact]
        public void GivenPrinterConfigurationModel_WhenAdd_ThenPrinterAddedToConfig()
        {
            // Arrange
            var mockFileIOService = new Mock<IFileIOService>();
            var config = new PrinterConfigurationModel()
            {
                Name = Guid.NewGuid().ToString(),
                Port = "com6",
                BaudRate = 100,
                BedXSize = 101,
                BedYSize = 102,
                BedZSize = 103,
                PrintableAreaMarginBack = 104,
                PrintableAreaMarginFront = 105,
                PrintableAreaMarginLeft = 106,
                PrintableAreaMarginRight = 107
            };

            mockFileIOService.Setup(x => x.ReadAllText(
                It.IsAny<string>()))
                .Returns("{}");

            var sut = new PrinterConfigurationManagerService(
                new WindowsStoragePathService(),
                mockFileIOService.Object);

            // Act
            sut.Add(config);

            // Assert
            Assert.Single(sut.Config.Printers);
            Assert.Equal(config, sut.Config.Printers.First());
        }

        [Fact]
        public void Given2Printers_WhenRemove_ThenPrinterRemovedFromConfig()
        {
            // Arrange
            var mockFileIOService = new Mock<IFileIOService>();

            mockFileIOService.Setup(x => x.ReadAllText(
                It.IsAny<string>()))
                .Returns("{\"Printers\":[{},{}]}");

            var sut = new PrinterConfigurationManagerService(
                new WindowsStoragePathService(),
                mockFileIOService.Object);

            // Act
            sut.Remove(sut.Config.Printers.First());

            // Assert
            Assert.Single(sut.Config.Printers);
        }


        [Fact]
        public void Given2Printers_WhenClear_ThenAllPrintersRemovedFromConfig()
        {
            // Arrange
            var mockFileIOService = new Mock<IFileIOService>();

            mockFileIOService.Setup(x => x.ReadAllText(
                It.IsAny<string>()))
                .Returns("{\"Printers\":[{},{}]}");

            var sut = new PrinterConfigurationManagerService(
                new WindowsStoragePathService(),
                mockFileIOService.Object);

            // Act
            sut.Clear();

            // Assert
            Assert.Empty(sut.Config.Printers);
        }

        [Fact]
        public void Given1Printer_WhenSave_ThenAllPrintersRemovedFromConfig()
        {
            // Arrange
            var windowsStoragePathService = new WindowsStoragePathService();
            var mockFileIOService = new Mock<IFileIOService>();

            mockFileIOService.Setup(x => x.ReadAllText(
                It.IsAny<string>()))
                .Returns("{\"Printers\":[{}]}");

            var sut = new PrinterConfigurationManagerService(
                windowsStoragePathService,
                mockFileIOService.Object);

            // Act
            sut.Save();

            // Assert
            var configJson = JsonConvert.SerializeObject(sut.Config);
            mockFileIOService.Verify(x => x.WriteAllText(
                It.Is<string>(x => x == windowsStoragePathService.UserAppConfigPrinterConfigurationsFilePath),
                It.Is<string>(x => x == configJson)), Times.Once);
        }
    }
}
