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
                "Activate Unified Bed Leveling.",
                typeof(bool),
                true),
            new CommandParameter(
                "B",
                "Business Card mode.",
                typeof(bool),
                true),
            new CommandParameter(
                "C",
                "",
                true),
            new CommandParameter(
                "D",
                "Disable Unified Bed Leveling.",
                typeof(bool),
                true),
            new CommandParameter(
                "E",
                "Stow probe after probing each point.",
                typeof(bool),
                true),
            new CommandParameter(
                "F",
                "Fade height.",
                typeof(float),
                true),
            new CommandParameter(
                "H",
                "Height.",
                typeof(float),
                true),
            new CommandParameter(
                "I",
                "Invalidate a number of mesh points.",
                typeof(int),
                true),
            new CommandParameter(
                "J",
                "Grid (or 3-Point) leveling.",
                typeof(int),
                true),
            new CommandParameter(
                "K",
                "Subtract (diff) the stored mesh with the given index from the current mesh.",
                typeof(int),
                true),
            new CommandParameter(
                "L",
                "Load a mesh. If no index is given, load the previously-activated mesh.",
                typeof(int),
                true),
            new CommandParameter(
                "P",
                "Phase mode",
                typeof(int),
                true),
            new CommandParameter(
                "Q",
                "Test Pattern. This command is intended for developers and is not required for everyday bed leveling.",
                typeof(int),
                true),
            new CommandParameter(
                "R",
                "Repeat count.",
                typeof(int),
                true),
            new CommandParameter(
                "S",
                "Save the mesh to EEPROM in the given slot.",
                typeof(int),
                true),
            new CommandParameter(
                "T",
                "Topology.",
                typeof(int),
                true),
            new CommandParameter(
                "U",
                "Unlevel.",
                typeof(bool),
                true),
            new CommandParameter(
                "W",
                "Display valuable UBL data.",
                typeof(bool),
                true),
            new CommandParameter(
                "X",
                "X position.",
                typeof(float),
                true),
            new CommandParameter(
                "Y",
                "Y position.",
                typeof(float),
                true)
        };

        public override bool Process(PrinterCommand command)
        {
            return false;
        }
    }
}
