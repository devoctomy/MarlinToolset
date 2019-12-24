using System;

namespace MarlinToolset.Services
{
    public class PrinterControllerReceivedDataEventArgs : EventArgs
    {
        public string Data { get; set; }
    }
}
