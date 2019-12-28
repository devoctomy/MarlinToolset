using MarlinToolset.Domain.Model;
using System;

namespace MarlinToolset.Core.Services
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
