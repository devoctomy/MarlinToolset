using System;
using System.Collections.Generic;
using System.Text;

namespace MarlinToolset.Core.Services
{
    public class PrinterCommand
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public IList<byte> Data { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
        public bool Acknowledged { get; set; }
        public Queue<PrinterPacket> CommandQueue { get; } = new Queue<PrinterPacket>();
    }
}
