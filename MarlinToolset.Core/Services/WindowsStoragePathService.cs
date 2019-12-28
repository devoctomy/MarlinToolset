using System;

namespace MarlinToolset.Core.Services
{
    public class WindowsStoragePathService : IStoragePathService
    {
        public string UserAppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public string UserAppConfigPath
        {
            get
            {
                return System.IO.Path.Combine(UserAppDataPath, "MarlinToolset\\Config\\");
            }
        }

        public string UserAppConfigPrinterConfigurationsFilePath
        {
            get
            {
                return System.IO.Path.Combine(UserAppConfigPath, "printers.json");
            }
        }

        public string SystemAppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
    }
}
