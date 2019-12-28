using System;
using System.Collections.Generic;

namespace MarlinToolset.Core.Services
{
    public class PrinterCommand
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime SentAt { get; } = DateTime.UtcNow;
        public IList<byte> Data { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
        public bool Acknowledged { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public TimeSpan? Elapsed => (AcknowledgedAt.HasValue ? AcknowledgedAt - SentAt : null);
        public Queue<PrinterPacket> ResponsePacketQueue { get; } = new Queue<PrinterPacket>();
    }
}
