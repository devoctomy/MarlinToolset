using System.Collections.Generic;

namespace MarlinToolset.Core.Services
{
    public interface ICommandProcessorService
    {
        string Key { get; }
        string Description { get; }
        string Url { get; }
        IList<CommandParameter> Parameters { get; }

        bool Process(PrinterCommand command);
    }
}
