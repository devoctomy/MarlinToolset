using MarlinToolset.Model;
using Newtonsoft.Json;
using System.IO;

namespace MarlinToolset.Services
{
    public class PrinterConfigurationManagerService : IPrinterConfigurationManagerService
    {
        public PrintersConfigurationModel Config { get; set; }
        private readonly IStoragePathService _storagePathService;

        public PrinterConfigurationManagerService(IStoragePathService storagePathService)
        {
            _storagePathService = storagePathService;
            Config = new PrintersConfigurationModel();
            Load();
        }

        public void Add(PrinterConfigurationModel printerConfigurationModel)
        {
            Config.Printers.Add(printerConfigurationModel);
            Save();
        }

        public void Remove(PrinterConfigurationModel printerConfigurationModel)
        {
            Config.Printers.Remove(printerConfigurationModel);
            Save();
        }

        public void Clear()
        {
            Config.Printers.Clear();
            Save();
        }

        private void Load()
        {
            var configFilePath = _storagePathService.UserAppConfigPrinterConfigurationsFilePath;
            if(File.Exists(configFilePath))
            {
                var configData = File.ReadAllText(configFilePath);
                Config = JsonConvert.DeserializeObject<PrintersConfigurationModel>(configData);
            }
        }

        private void Save()
        {
            var configFilePath = _storagePathService.UserAppConfigPrinterConfigurationsFilePath;
            var configData = JsonConvert.SerializeObject(Config);
            var configFile = new FileInfo(configFilePath);
            configFile.Directory.Create();
            File.WriteAllText(configFilePath, configData);
        }
    }
}
