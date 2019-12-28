using System;

namespace MarlinToolset.Core.Services
{
    public class PrinterPacket
    {
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
        public string RawData { get; set; }
        public string PreProcessedData { get; set; } 
        public bool IsAck { get; set; }
    }
}
