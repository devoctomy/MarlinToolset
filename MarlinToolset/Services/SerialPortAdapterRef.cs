using MarlinToolset.Model;
using System;

namespace MarlinToolset.Services
{
    public class SerialPortAdapterRef
    {
        public PrinterConfigurationModel Config { get; }
        public Action<SerialPortAdapterRef, string> DataReceivedCallback { get; }

        public SerialPortAdapterRef(
            PrinterConfigurationModel config,
            Action<SerialPortAdapterRef, string> dataReceivedCalllback)
        {
            Config = config;
            DataReceivedCallback = dataReceivedCalllback;
        }
    }
}
