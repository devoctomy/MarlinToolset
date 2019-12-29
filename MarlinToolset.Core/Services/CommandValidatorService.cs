using MarlinToolset.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarlinToolset.Core.Services
{
    public class CommandValidatorService : ICommandValidatorService
    {
        public void Validate(
            CommandDefinition commandDefinition,
            string commandText)
        {
            var commandParts = commandText.Split(' ');
            if (commandParts[0] != commandDefinition.Key) throw new CommandKeyIncorrectException(commandParts[0], commandDefinition.Key);

            var referencedParameters = new List<CommandParameter>();
            for (int curPart = 1; curPart < commandParts.Length; curPart++)
            {
                var token = commandParts[curPart][0].ToString().ToUpper();
                var value = commandParts[curPart].Length > 1 ? commandParts[curPart].Substring(1) : null;
                var parameter = commandDefinition.Parameters.SingleOrDefault(x => x.Token == token);
                if (parameter == null)
                {
                    throw new UnknownCommandParameterException(token);
                }
                else
                {
                    if (referencedParameters.Contains(parameter))
                    {
                        throw new DuplicateCommandParameterException(parameter);
                    }
                    else
                    {
                        if(!string.IsNullOrWhiteSpace(parameter.Requires))
                        {
                            var requires = parameter.Requires.Split(',');
                            foreach(var curRequired in requires)
                            {
                                var match = commandParts.SingleOrDefault(x => x.StartsWith(curRequired));
                                if(match == null)
                                {
                                    throw new UnreferencedRequiredCommandParametersException();
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(parameter.Choices))
                        {
                            var choices = parameter.Choices.Split(',');
                            var match = choices.SingleOrDefault(x => x == commandParts[curPart].Substring(1));
                            if (match == null)
                            {
                                throw new InvalidParameterChoiceException(
                                   token,
                                   value,
                                   choices);
                            }
                        }

                        try
                        {
                            parameter.SetValue(!string.IsNullOrEmpty(value) ? value : "true");
                        }
                        catch (FormatException)
                        {
                            throw new InvalidCommandParameterException(token);
                        }
                        referencedParameters.Add(parameter);
                    }
                }
            }

            var unreferencedRequired = commandDefinition.Parameters
                .Where(x => !x.Optional && !referencedParameters.Contains(x));

            if (unreferencedRequired.Any())
            {
                throw new UnreferencedRequiredCommandParametersException(unreferencedRequired);
            }
        }
    }
}
