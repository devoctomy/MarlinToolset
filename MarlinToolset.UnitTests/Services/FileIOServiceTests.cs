using MarlinToolset.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MarlinToolset.UnitTests.Services
{
    public class FileIOServiceTests : IDisposable
    {
        private List<string> _tempFiles;

        public FileIOServiceTests()
        {
            _tempFiles = new List<string>();
        }

        public void Dispose()
        {
            foreach(string curTempFile in _tempFiles)
            {
                if(File.Exists(curTempFile))
                {
                    File.Delete(curTempFile);
                }
            }
        }

        [Fact]
        public void GivenPath_AndContent_AndPathNotExists_WhenWriteAllText_ThenFileCreatedAtPathContainingContent()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var path = $"{id}.txt";
            var contents = id;
            var sut = new FileIOService();

            // Act
            sut.WriteAllText(
                path,
                contents);

            // Assert
            Assert.True(File.Exists(path));
            _tempFiles.Add(path);
            Assert.Equal(contents, File.ReadAllText(path));
        }

        [Fact]
        public void GivenPath_AndContent_AndPathExists_WhenWriteAllText_ThenFileOverwrittenAtPathWithNewContent()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var path = $"{id}.txt";
            var contents1 = $"{id}.1";
            var contents2 = $"{id}.1";
            File.WriteAllText(
                path,
                contents1);
            _tempFiles.Add(path);
            var sut = new FileIOService();

            // Act
            sut.WriteAllText(
                path,
                contents2);

            // Assert
            Assert.True(File.Exists(path));
            Assert.Equal(contents2, File.ReadAllText(path));
        }

        [Fact]
        public void GivenPath_AndContentExists_WhenReadAllText_ThenFileContentsReturned()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var path = $"{id}.txt";
            var contents = id;
            File.WriteAllText(
                path,
                contents);
            _tempFiles.Add(path);
            var sut = new FileIOService();

            // Act
            var result = sut.ReadAllText(path);

            // Assert
            Assert.Equal(contents, result);
        }

    }
}
