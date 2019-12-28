using MarlinToolset.Core.Services;
using System.Collections.Generic;

namespace MarlinToolset.CommandProcessors
{
    public class UnifiedBedLevellingCommandProcessor : ICommandProcessorService
    {
        public string Command => "G29";

        public string Description => "The Unified Bed Leveling System (UBL) provides a comprehensive set of resources to produce the best bed leveling results possible.";

        public string Url => "http://marlinfw.org/docs/gcode/G029-ubl.html";

        public IList<CommandParameter> Parameters => new[]
        {
            new CommandParameter(
                "A",
                "Activate Unified Bed Leveling",
                true)
        };

        public bool Process(PrinterCommand command)
        {
            return false;
        }
    }
}
