﻿using System;
using System.Collections.Generic;

namespace MarlinToolset.Services
{
    public class PrinterControllerReceivedDataEventArgs : EventArgs
    {
        public PrinterPacket Packet { get; set; }
    }
}
