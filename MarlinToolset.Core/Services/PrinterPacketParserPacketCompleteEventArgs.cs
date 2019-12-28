using System;

namespace MarlinToolset.Core.Services
{
    public class PrinterPacketParserPacketCompleteEventArgs : EventArgs
    {
        public PrinterPacket Packet { get; set; }
    }
}
