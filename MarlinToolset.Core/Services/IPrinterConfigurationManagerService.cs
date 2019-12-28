using MarlinToolset.Domain.Model;

namespace MarlinToolset.Core.Services
{
    public interface IPrinterConfigurationManagerService
    {
        PrintersConfigurationModel Config { get; }
        void Add(PrinterConfigurationModel printerConfigurationModel);
        void Remove(PrinterConfigurationModel printerConfigurationModel);
        void Clear();
        bool Load();
        void Save();
    }
}
