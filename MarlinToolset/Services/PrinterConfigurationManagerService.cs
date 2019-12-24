using MarlinToolset.Model;
using Newtonsoft.Json;
using System.IO;

namespace MarlinToolset.Services
{
    public class PrinterConfigurationManagerService : IPrinterConfigurationManagerService
    {
        public PrintersConfigurationModel Config { get; set; }
        private readonly IStoragePathService _storagePathService;
        private readonly IFileIOService _fileIOService;

        public PrinterConfigurationManagerService(
            IStoragePathService storagePathService,
            IFileIOService fileIOService,
            bool load)
        {
            _storagePathService = storagePathService;
            _fileIOService = fileIOService;
            Config = new PrintersConfigurationModel();
            if(load) Load();
        }

        public void Add(PrinterConfigurationModel printerConfigurationModel)
        {
            Config.Printers.Add(printerConfigurationModel);
        }

        public void Remove(PrinterConfigurationModel printerConfigurationModel)
        {
            Config.Printers.Remove(printerConfigurationModel);
        }

        public void Clear()
        {
            Config.Printers.Clear();
        }

        public bool Load()
        {
            var loaded = false;
            var configFilePath = _storagePathService.UserAppConfigPrinterConfigurationsFilePath;
            if(_fileIOService.Exists(configFilePath))
            {
                var configData = _fileIOService.ReadAllText(configFilePath);
                Config = JsonConvert.DeserializeObject<PrintersConfigurationModel>(configData);
                loaded = true;
            }

            return loaded;
        }

        public void Save()
        {
            var configFilePath = _storagePathService.UserAppConfigPrinterConfigurationsFilePath;
            var configData = JsonConvert.SerializeObject(Config);
            var configFile = new FileInfo(configFilePath);
            configFile.Directory.Create();
            _fileIOService.WriteAllText(
                configFilePath,
                configData);
        }
    }
}
