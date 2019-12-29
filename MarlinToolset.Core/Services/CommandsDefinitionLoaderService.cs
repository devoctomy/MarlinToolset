using Newtonsoft.Json;
using System.IO;

namespace MarlinToolset.Core.Services
{
    public class CommandsDefinitionLoaderService : ICommandsDefinitionLoaderService
    {
        public CommandsDefinition CommandDefinitions { get; private set; }

        public async void Load(string commandsFile)
        {
            var data = await File.ReadAllTextAsync(commandsFile);
            var commandDefinition = JsonConvert.DeserializeObject<CommandsDefinition>(data);
            CommandDefinitions = commandDefinition;
        }

    }
}
