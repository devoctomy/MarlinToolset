using MarlinToolset.Model;
using System;

namespace MarlinToolset.Services
{
    public interface IPrinterControllerService
    {
        event EventHandler<PrinterControllerReceivedDataEventArgs> ReceivedData;

        PrinterConfigurationModel Printer { get; }
        bool IsConnected { get; }

        void Connect(PrinterConfigurationModel printer);
        void Disconnect();
    }
}
