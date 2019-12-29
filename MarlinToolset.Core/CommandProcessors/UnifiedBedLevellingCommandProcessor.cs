using MarlinToolset.Core.Exceptions;
using MarlinToolset.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace MarlinToolset.Core.CommandProcessors
{
    public class UnifiedBedLevellingCommandProcessor : CommandProcessorBase
    {
        public override string Key => "G29";

        public override string Description => "The Unified Bed Leveling System (UBL) provides a comprehensive set of resources to produce the best bed leveling results possible.";

        public override string Url => "http://marlinfw.org/docs/gcode/G029-ubl.html";

        public override IList<CommandParameter> Parameters => new[]
        {
            new CommandParameter(
                "A",
                "Activate Unified Bed Leveling",
                true)
        };

        public override bool Process(PrinterCommand command)
        {
            return false;
        }
    }
}
