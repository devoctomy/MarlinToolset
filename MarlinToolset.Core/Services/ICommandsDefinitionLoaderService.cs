using System;
using System.Collections.Generic;

namespace MarlinToolset.Core.Services
{
    public interface ICommandsDefinitionLoaderService
    {
        CommandsDefinition CommandDefinitions { get; }
        void Load(string commandsFile);
    }
}
