using MarlinToolset.Core.Exceptions;
using MarlinToolset.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MarlinToolset.Core.UnitTests.CommandProcessors
{
    public class CommandValidatorServiceTests
    {
        [Fact]
        public void GivenCommandWithRequiredParameters_AndValidCommandText_WhenValidate_ThenNoExceptionThrown_AndParametersParsed()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter()
                {
                    Token = "A",
                    Description = "Parameter 1",
                    Optional = false
                },
                new CommandParameter()
                {
                    Token = "B",
                    Description = "Parameter 2",
                    Optional = false,
                    ValueType = "int"
                },
                new CommandParameter()
                {
                    Token = "C",
                    Description = "Parameter 3",
                    Optional = false,
                    ValueType = "float"
                }
            };
            var definition = new CommandDefinition()
            {
                Key = "G10",
                Description = "This is some command",
                Url = "http://www.somewebsite.com/docs/commands/g10",
                Parameters = parameters
            };
            var sut = new CommandValidatorService();
            var commandText = "G10 A B1 C2.3";

            // Act / Assert
            sut.Validate(
                definition,
                commandText);
            Assert.Equal(true, definition.Parameters.Single(x => x.Token == "A").Value);
            Assert.Equal(1, definition.Parameters.Single(x => x.Token == "B").Value);
            Assert.Equal(2.3f, definition.Parameters.Single(x => x.Token == "C").Value);
        }

        [Fact]
        public void GivenCommandWithRequiredParameters_AndMissingParameter_WhenValidate_ThenExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter()
                {
                    Token = "A",
                    Description = "Parameter 1",
                    Optional = false
                },
                new CommandParameter()
                {
                    Token = "B",
                    Description = "Parameter 2",
                    Optional = false,
                    ValueType = "int"
                },
                new CommandParameter()
                {
                    Token = "C",
                    Description = "Parameter 3",
                    Optional = false,
                    ValueType = "float"
                }
            };
            var definition = new CommandDefinition()
            {
                Key = "G10",
                Description = "This is some command",
                Url = "http://www.somewebsite.com/docs/commands/g10",
                Parameters = parameters
            };
            var sut = new CommandValidatorService();
            var commandText = "G10 B1 C2.3";

            // Act / Assert
            Assert.ThrowsAny<UnreferencedRequiredCommandParametersException>(() =>
            {
                sut.Validate(
                    definition,
                    commandText);
            });
        }

        [Fact]
        public void GivenCommandWithRequiredParameters_AndParameterWrongType_WhenValidate_ThenExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter()
                {
                    Token = "A",
                    Description = "Parameter 1",
                    Optional = false
                },
                new CommandParameter()
                {
                    Token = "B",
                    Description = "Parameter 2",
                    Optional = false,
                    ValueType = "int"
                },
                new CommandParameter()
                {
                    Token = "C",
                    Description = "Parameter 3",
                    Optional = false,
                    ValueType = "float"
                }
            };
            var definition = new CommandDefinition()
            {
                Key = "G10",
                Description = "This is some command",
                Url = "http://www.somewebsite.com/docs/commands/g10",
                Parameters = parameters
            };
            var sut = new CommandValidatorService();
            var commandText = "G10 A B1.5 C2.3";

            // Act / Assert
            Assert.ThrowsAny<InvalidCommandParameterException>(() =>
            {
                sut.Validate(
                    definition,
                    commandText);
            });
        }

        [Fact]
        public void GivenCommandWithRequiredParameters_AndParameterUnknown_WhenValidate_ThenExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter()
                {
                    Token = "A",
                    Description = "Parameter 1",
                    Optional = false
                },
                new CommandParameter()
                {
                    Token = "B",
                    Description = "Parameter 2",
                    Optional = false,
                    ValueType = "int"
                },
                new CommandParameter()
                {
                    Token = "C",
                    Description = "Parameter 3",
                    Optional = false,
                    ValueType = "float"
                }
            };
            var definition = new CommandDefinition()
            {
                Key = "G10",
                Description = "This is some command",
                Url = "http://www.somewebsite.com/docs/commands/g10",
                Parameters = parameters
            };
            var sut = new CommandValidatorService();
            var commandText = "G10 A B1 C2.3 D1";

            // Act / Assert
            Assert.ThrowsAny<UnknownCommandParameterException>(() =>
            {
                sut.Validate(
                    definition,
                    commandText);
            });
        }

        [Fact]
        public void GivenCommandWithRequiredParameters_AndDuplicateParameter_WhenValidate_ThenExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter()
                {
                    Token = "A",
                    Description = "Parameter 1",
                    Optional = false
                },
                new CommandParameter()
                {
                    Token = "B",
                    Description = "Parameter 2",
                    Optional = false,
                    ValueType = "int",
                },
                new CommandParameter()
                {
                    Token = "C",
                    Description = "Parameter 3",
                    Optional = false,
                    ValueType = "float"
                }
            };
            var definition = new CommandDefinition()
            {
                Key = "G10",
                Description = "This is some command",
                Url = "http://www.somewebsite.com/docs/commands/g10",
                Parameters = parameters
            };
            var sut = new CommandValidatorService();
            var commandText = "G10 A B1 C2.3 B1";

            // Act / Assert
            Assert.ThrowsAny<DuplicateCommandParameterException>(() =>
            {
                sut.Validate(
                    definition,
                    commandText);
            });
        }

        [Fact]
        public void GivenCommand_AndIncorrectCommandKey_WhenValidate_ThenExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter()
                {
                    Token = "A",
                    Description = "Parameter 1",
                    Optional = false
                }
            };
            var definition = new CommandDefinition()
            {
                Key = "G10",
                Description = "This is some command",
                Url = "http://www.somewebsite.com/docs/commands/g10",
                Parameters = parameters
            };
            var sut = new CommandValidatorService();
            var commandText = "G11 A";

            // Act / Assert
            Assert.Throws<CommandKeyIncorrectException>(() =>
            {
                sut.Validate(
                    definition,
                    commandText);
            });
        }

        [Fact]
        public void GivenCommand_AndParameterRequiresAnother_AndMissingRequiredParameter_WhenValidate_ThenExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter()
                {
                    Token = "A",
                    Description = "Parameter 1",
                    Optional = true
                },
                new CommandParameter()
                {
                    Token = "B",
                    Description = "Parameter 2",
                    Optional = true,
                    Requires = "A"
                }
            };
            var definition = new CommandDefinition()
            {
                Key = "G10",
                Description = "This is some command",
                Url = "http://www.somewebsite.com/docs/commands/g10",
                Parameters = parameters
            };
            var sut = new CommandValidatorService();
            var commandText = "G10 B";

            // Act / Assert
            Assert.Throws<UnreferencedRequiredCommandParametersException>(() =>
            {
                sut.Validate(
                    definition,
                    commandText);
            });
        }

        [Fact]
        public void GivenCommand_AndParameterInChoices_WhenValidate_ThenNoExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter()
                {
                    Token = "A",
                    Description = "Parameter 1",
                    Optional = false,
                    ValueType = "int",
                    Choices = "1,2,3,4"
                },
            };
            var definition = new CommandDefinition()
            {
                Key = "G10",
                Description = "This is some command",
                Url = "http://www.somewebsite.com/docs/commands/g10",
                Parameters = parameters
            };
            var sut = new CommandValidatorService();
            var commandText = "G10 A3";

            // Act / Assert
            sut.Validate(
                definition,
                commandText);
        }

        [Fact]
        public void GivenCommand_AndParameterNotInChoices_WhenValidate_ThenExceptionThrown()
        {
            // Arrange
            var parameters = new[]
            {
                new CommandParameter()
                {
                    Token = "A",
                    Description = "Parameter 1",
                    Optional = false,
                    ValueType = "int",
                    Choices = "1,2,3,4"
                },
            };
            var definition = new CommandDefinition()
            {
                Key = "G10",
                Description = "This is some command",
                Url = "http://www.somewebsite.com/docs/commands/g10",
                Parameters = parameters
            };
            var sut = new CommandValidatorService();
            var commandText = "G10 A5";

            // Act / Assert
            Assert.ThrowsAny<InvalidParameterChoiceException>(() => sut.Validate(
               definition,
               commandText));
        }
    }
}
