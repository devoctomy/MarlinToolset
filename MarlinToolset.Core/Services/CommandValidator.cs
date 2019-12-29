using MarlinToolset.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarlinToolset.Core.Services
{
    public class CommandValidator : ICommandValidator
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
                        try
                        {
                            parameter.SetValue(commandParts[curPart].Length > 1 ? commandParts[curPart].Substring(1) : "true");
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
                throw new UnreferencedRequiredCommandParameterException(unreferencedRequired);
            }
        }
    }
}
