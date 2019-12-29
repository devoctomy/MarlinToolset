using MarlinToolset.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MarlinToolset.Core.UnitTests.Services
{
    public class CommandsDefinitionLoaderServiceTests
    {
        [Fact]
        public void Given_When_Then()
        {
            // Arrange
            var sut = new CommandsDefinitionLoaderService();

            // Act
            sut.Load("Data\\TestCommands.json");

            // Assert
            Assert.Single(sut.CommandDefinitions.Commands);
            Assert.Equal("G10", sut.CommandDefinitions.Commands[0].Key);
            Assert.Equal("Description of the command.", sut.CommandDefinitions.Commands[0].Description);
            Assert.Equal("http://www.somewebsite.com/docs/commands/g10", sut.CommandDefinitions.Commands[0].Url);
            Assert.Equal(4, sut.CommandDefinitions.Commands[0].Parameters.Count);

            Assert.Equal("A", sut.CommandDefinitions.Commands[0].Parameters[0].Token);
            Assert.Equal("Parameter 1.", sut.CommandDefinitions.Commands[0].Parameters[0].Description);
            Assert.True(sut.CommandDefinitions.Commands[0].Parameters[0].Optional);

            Assert.Equal("B", sut.CommandDefinitions.Commands[0].Parameters[1].Token);
            Assert.Equal("Parameter 2.", sut.CommandDefinitions.Commands[0].Parameters[1].Description);
            Assert.False(sut.CommandDefinitions.Commands[0].Parameters[1].Optional);

            Assert.Equal("C", sut.CommandDefinitions.Commands[0].Parameters[2].Token);
            Assert.Equal("Parameter 3.", sut.CommandDefinitions.Commands[0].Parameters[2].Description);
            Assert.False(sut.CommandDefinitions.Commands[0].Parameters[2].Optional);
            Assert.Equal("[P1,P2,P3,Z]", sut.CommandDefinitions.Commands[0].Parameters[2].Requires);

            Assert.Equal("D", sut.CommandDefinitions.Commands[0].Parameters[3].Token);
            Assert.Equal("Parameter 4.", sut.CommandDefinitions.Commands[0].Parameters[3].Description);
            Assert.False(sut.CommandDefinitions.Commands[0].Parameters[3].Optional);
            Assert.Equal("1,2,3,4,5", sut.CommandDefinitions.Commands[0].Parameters[3].Choices);
        }
    }
}
