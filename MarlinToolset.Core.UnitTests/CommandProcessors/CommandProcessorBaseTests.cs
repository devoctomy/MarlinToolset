﻿using MarlinToolset.Core.Exceptions;
using MarlinToolset.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MarlinToolset.Core.UnitTests.CommandProcessors
{
    public class CommandProcessorBaseTests
    {
        [Fact]
        public void GivenCommandWithRequiredParameters_AndValidCommandText_WhenValidate_ThenNoExceptionThrown_AndParametersParsed()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter(
                    "A",
                    "Parameter 1",
                    typeof(bool),
                    false),
                new CommandParameter(
                    "B",
                    "Parameter 2",
                    typeof(int),
                    false),
                new CommandParameter(
                    "C",
                    "Parameter 3",
                    typeof(float),
                    false),
            };
            var sut = new TestableCommandProcessor(
                "G10",
                "This is some command",
                "http://www.somewebsite.com/docs/commands/g10",
                parameters);
            var commandText = "G10 A B1 C2.3";

            // Act / Assert
            sut.Validate(commandText);
            Assert.Equal(true, sut.Parameters.Single(x => x.Token == "A").Value);
            Assert.Equal(1, sut.Parameters.Single(x => x.Token == "B").Value);
            Assert.Equal(2.3f, sut.Parameters.Single(x => x.Token == "C").Value);
        }

        [Fact]
        public void GivenCommandWithRequiredParameters_AndMissingParameter_WhenValidate_ThenExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter(
                    "A",
                    "Parameter 1",
                    typeof(bool),
                    false),
                new CommandParameter(
                    "B",
                    "Parameter 2",
                    typeof(int),
                    false),
                new CommandParameter(
                    "C",
                    "Parameter 3",
                    typeof(float),
                    false),
            };
            var sut = new TestableCommandProcessor(
                "G10",
                "This is some command",
                "http://www.somewebsite.com/docs/commands/g10",
                parameters);
            var commandText = "G10 B1 C2.3";

            // Act / Assert
            Assert.ThrowsAny<UnreferencedRequiredCommandParameterException>(() =>
            {
                sut.Validate(commandText);
            });
        }

        [Fact]
        public void GivenCommandWithRequiredParameters_AndParameterWrongType_WhenValidate_ThenExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter(
                    "A",
                    "Parameter 1",
                    typeof(bool),
                    false),
                new CommandParameter(
                    "B",
                    "Parameter 2",
                    typeof(int),
                    false),
                new CommandParameter(
                    "C",
                    "Parameter 3",
                    typeof(float),
                    false),
            };
            var sut = new TestableCommandProcessor(
                "G10",
                "This is some command",
                "http://www.somewebsite.com/docs/commands/g10",
                parameters);
            var commandText = "G10 A B1.5 C2.3";

            // Act / Assert
            Assert.ThrowsAny<InvalidCommandParameterException>(() =>
            {
                sut.Validate(commandText);
            });
        }

        [Fact]
        public void GivenCommandWithRequiredParameters_AndParameterUnknown_WhenValidate_ThenExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter(
                    "A",
                    "Parameter 1",
                    typeof(bool),
                    false),
                new CommandParameter(
                    "B",
                    "Parameter 2",
                    typeof(int),
                    false),
                new CommandParameter(
                    "C",
                    "Parameter 3",
                    typeof(float),
                    false),
            };
            var sut = new TestableCommandProcessor(
                "G10",
                "This is some command",
                "http://www.somewebsite.com/docs/commands/g10",
                parameters);
            var commandText = "G10 A B1 C2.3 D1";

            // Act / Assert
            Assert.ThrowsAny<UnknownCommandParameterException>(() =>
            {
                sut.Validate(commandText);
            });
        }

        [Fact]
        public void GivenCommandWithRequiredParameters_AndDuplicateParameter_WhenValidate_ThenExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter(
                    "A",
                    "Parameter 1",
                    typeof(bool),
                    false),
                new CommandParameter(
                    "B",
                    "Parameter 2",
                    typeof(int),
                    false),
                new CommandParameter(
                    "C",
                    "Parameter 3",
                    typeof(float),
                    false),
            };
            var sut = new TestableCommandProcessor(
                "G10",
                "This is some command",
                "http://www.somewebsite.com/docs/commands/g10",
                parameters);
            var commandText = "G10 A B1 C2.3 B1";

            // Act / Assert
            Assert.ThrowsAny<DuplicateCommandParameterException>(() =>
            {
                sut.Validate(commandText);
            });
        }

        [Fact]
        public void GivenCommand_AndIncorrectCommandKey_WhenValidate_ThenExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter(
                    "A",
                    "Parameter 1",
                    typeof(bool),
                    false),
            };
            var sut = new TestableCommandProcessor(
                "G10",
                "This is some command",
                "http://www.somewebsite.com/docs/commands/g10",
                parameters);
            var commandText = "G11 A";

            // Act / Assert
            Assert.Throws<CommandKeyIncorrectException>(() =>
            {
                sut.Validate(commandText);
            });
        }
    }
}
