using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MarlinToolset.Model
{
    [DataContract]
    public class PrintersConfigurationModel
    {
        [DataMember]
        public ObservableCollection<PrinterConfigurationModel> Printers { get; set; } = new ObservableCollection<PrinterConfigurationModel>();
    }
}
