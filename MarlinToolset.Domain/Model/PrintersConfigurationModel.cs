using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MarlinToolset.Domain.Model
{
    [DataContract]
    public class PrintersConfigurationModel
    {
        [DataMember]
        public ObservableCollection<PrinterConfigurationModel> Printers { get; set; } = new ObservableCollection<PrinterConfigurationModel>();
    }
}
