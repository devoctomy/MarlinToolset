using MarlinToolset.Model;
using System;
using System.Text;

namespace MarlinToolset.Services
{
    public interface IPrinterControllerService
    {
        event EventHandler<PrinterControllerReceivedDataEventArgs> ReceivedData;

        PrinterConfigurationModel Printer { get; }
        bool IsConnected { get; }
        void Connect(PrinterConfigurationModel printer);
        void Disconnect();

        void Write(
            string data,
            Encoding encoding);

        void Write(
            byte[] data,
            int offset,
            int count);
    }
}
