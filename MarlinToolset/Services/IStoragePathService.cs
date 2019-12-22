namespace MarlinToolset.Services
{
    public interface IStoragePathService
    {
        string UserAppDataPath { get; }
        string UserAppConfigPath { get; }
        string UserAppConfigPrinterConfigurationsFilePath { get; }
        string SystemAppDataPath { get; }
    }
}
