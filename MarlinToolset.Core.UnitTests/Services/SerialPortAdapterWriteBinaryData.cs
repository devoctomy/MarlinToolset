using System;
using System.Collections.Generic;
using System.Text;

namespace MarlinToolset.Core.UnitTests.Services
{
    public class SerialPortAdapterWriteBinaryData
    {
        public IList<byte> Data { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
    }
}
