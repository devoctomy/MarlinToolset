using MarlinToolset.Domain.Model;
using System;
using System.Text;

namespace MarlinToolset.Core.Services
{
    public interface IPrinterControllerService
    {
        event EventHandler<PrinterControllerReceivedDataEventArgs> ReceivedData;

        PrinterConfigurationModel Printer { get; }
        bool IsConnected { get; }
        void Connect(PrinterConfigurationModel printer);
        void Disconnect();
        void ClearCommandStack();

        PrinterCommand Write(
            string data,
            Encoding encoding);

        PrinterCommand Write(
            byte[] data,
            int offset,
            int count);
    }
}
