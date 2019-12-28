using System;

namespace MarlinToolset.Core.Services
{
    public interface IPrinterPacketParser
    {
        event EventHandler<PrinterPacketParserPacketCompleteEventArgs> PacketComplete;
        void ReceiveData(string data);
    }
}
