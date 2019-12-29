using System.Collections.Generic;
using System.Linq;

namespace MarlinToolset.Core.Services
{
    public class CommandsDefinition
    {
        public IList<CommandDefinition> Commands { get; set; }

        public CommandDefinition GetFromCommandText(string commandText)
        {
            var commandParts = commandText.Trim().Split(' ');
            return Commands.SingleOrDefault(x => x.Key == commandParts[0].ToUpper());
        }
    }
}
