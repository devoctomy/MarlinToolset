using System;
using System.Collections.Generic;

namespace MarlinToolset.Services
{
    public class PrinterControllerReceivedDataEventArgs : EventArgs
    {
        public IList<string> Lines { get; set; }
    }
}
