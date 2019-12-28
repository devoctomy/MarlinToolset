using System;
using System.Collections.Generic;

namespace MarlinToolset.Core.Services
{
    public class PrinterControllerReceivedDataEventArgs : EventArgs
    {
        public PrinterPacket Packet { get; set; }
    }
}
