using ReactiveUI;
using System.Collections.Generic;

namespace MarlinToolset.Model
{
    public class PrintersConfigurationModel
    {
        public IList<PrinterConfigurationModel> Printers { get; set; } = new List<PrinterConfigurationModel>();
    }
}
