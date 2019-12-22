using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MarlinToolset.Model
{
    [DataContract]
    public class PrintersConfigurationModel
    {
        [DataMember]
        public IList<PrinterConfigurationModel> Printers { get; set; } = new List<PrinterConfigurationModel>();
    }
}
