using MarlinToolset.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MarlinToolset.UnitTests.Services
{
    public class WindowsStoragePathServiceTests
    {
        [Fact]
        public void GivenCurrentUser_WhenSystemAppDataPath_ThenCorrectPathReturned()
        {
            // Arrange
            var sut = new WindowsStoragePathService();

            // Act
            var path = sut.SystemAppDataPath;

            // Assert
            Assert.Equal($"C:\\ProgramData", path);
        }

        [Fact]
        public void GivenCurrentUser_WhenUserAppDataPath_ThenCorrectPathReturned()
        {
            // Arrange
            var sut = new WindowsStoragePathService();

            // Act
            var path = sut.UserAppDataPath;

            // Assert
            Assert.Equal($"C:\\Users\\{Environment.UserName}\\AppData\\Local", path);
        }

        [Fact]
        public void GivenCurrentUser_WhenUserAppConfigPath_ThenCorrectPathReturned()
        {
            // Arrange
            var sut = new WindowsStoragePathService();

            // Act
            var path = sut.UserAppConfigPath;

            // Assert
            Assert.Equal($"C:\\Users\\{Environment.UserName}\\AppData\\Local\\MarlinToolset\\Config\\", path);
        }

        [Fact]
        public void GivenCurrentUser_WhenUserAppConfigPrinterConfigurationsFilePath_ThenCorrectPathReturned()
        {
            // Arrange
            var sut = new WindowsStoragePathService();

            // Act
            var path = sut.UserAppConfigPrinterConfigurationsFilePath;

            // Assert
            Assert.Equal($"C:\\Users\\{Environment.UserName}\\AppData\\Local\\MarlinToolset\\Config\\printers.json", path);
        }

    }
}
