using MarlinToolset.Core.Exceptions;
using MarlinToolset.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarlinToolset.Core.CommandProcessors
{
    public class CommandProcessorBase : ICommandProcessorService
    {
        public virtual string Key { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }

        public virtual string Description { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }

        public virtual string Url { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }

        public virtual IList<CommandParameter> Parameters { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }

        public virtual bool Process(PrinterCommand command)
        {
            throw new System.NotImplementedException();
        }

        public void Validate(string commandText)
        {
            var commandParts = commandText.Split(' ');
            if (commandParts[0] != Key) throw new CommandKeyIncorrectException(commandParts[0], Key);

            var referencedParameters = new List<CommandParameter>();
            for (int curPart = 1; curPart < commandParts.Length; curPart++)
            {
                var token = commandParts[curPart][0].ToString().ToUpper();
                var parameter = Parameters.SingleOrDefault(x => x.Token == token);
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

            var unreferencedRequired = Parameters
                .Where(x => !x.Optional && !referencedParameters.Contains(x));

            if (unreferencedRequired.Any())
            {
                throw new UnreferencedRequiredCommandParameterException(unreferencedRequired);
            }
        }
    }
}
