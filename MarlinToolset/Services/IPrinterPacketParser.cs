using System;

namespace MarlinToolset.Services
{
    public interface IPrinterPacketParser
    {
        event EventHandler<PrinterPacketParserPacketCompleteEventArgs> PacketComplete;
        void ReceiveData(string data);
    }
}
