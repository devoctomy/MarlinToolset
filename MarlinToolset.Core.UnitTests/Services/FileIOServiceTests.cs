using MarlinToolset.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MarlinToolset.Core.UnitTests.Services
{
    public class FileIOServiceTests : IDisposable
    {
        private readonly List<string> _tempFiles;
        private bool _disposed;

        public FileIOServiceTests()
        {
            _tempFiles = new List<string>();
        }

        ~FileIOServiceTests()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if(disposing)
            {
                foreach (string curTempFile in _tempFiles)
                {
                    if (File.Exists(curTempFile))
                    {
                        File.Delete(curTempFile);
                    }
                }
            }

            _disposed = true;
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

        [Fact]
        public void GivenPath_AndPathExists_WhenExists_ThenTrueReturned()
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
            var exists = sut.Exists(path);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void GivenPath_AndPathNotExists_WhenExists_ThenFalseReturned()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var path = $"{id}.txt";
            var sut = new FileIOService();

            // Act
            var exists = sut.Exists(path);

            // Assert
            Assert.False(exists);
        }

    }
}
