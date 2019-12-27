using System;

namespace MarlinToolset.Services
{
    public class PrinterPacketParserPacketCompleteEventArgs : EventArgs
    {
        public PrinterPacket Packet { get; set; }
    }
}
