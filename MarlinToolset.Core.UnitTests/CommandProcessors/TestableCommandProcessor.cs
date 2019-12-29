using MarlinToolset.Core.CommandProcessors;
using MarlinToolset.Core.Services;
using System.Collections.Generic;

namespace MarlinToolset.Core.UnitTests.CommandProcessors
{
    public class TestableCommandProcessor : CommandProcessorBase
    {
        public override string Key { get; protected set; }

        public override string Description { get; protected set; }

        public override string Url { get; protected set; }

        public override IList<CommandParameter> Parameters { get; protected set;}

        public TestableCommandProcessor(
            string key,
            string description,
            string url,
            params CommandParameter[] parameters)
        {
            Key = key;
            Description = description;
            Url = url;
            Parameters = parameters;
        }

        public override bool Process(PrinterCommand command)
        {
            return false;
        }
    }
}
