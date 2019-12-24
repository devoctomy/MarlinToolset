using MarlinToolset.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarlinToolset.Services
{
    public interface IPrinterConfigurationManagerService
    {
        PrintersConfigurationModel Config { get; }
        void Add(PrinterConfigurationModel printerConfigurationModel);
        void Remove(PrinterConfigurationModel printerConfigurationModel);
        void Clear();
        void Save();
    }
}
