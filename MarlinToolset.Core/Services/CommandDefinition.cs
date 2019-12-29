using System;
using System.Collections.Generic;
using System.Text;

namespace MarlinToolset.Core.Services
{
    public class CommandDefinition
    {
        public string Key { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public IList<CommandParameter> Parameters { get; set; }
    }
}
