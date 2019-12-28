using System;
using System.Text;

namespace MarlinToolset.Services
{
    public class MarlinPrinterPacketParser : IPrinterPacketParser
    {
        private const string MARLIN_COMMAND_ACK = "ok";

        public event EventHandler<PrinterPacketParserPacketCompleteEventArgs> PacketComplete;

        private readonly MarlinPrinterPacketParserOptions _options;
        private readonly StringBuilder _buffer;

        public MarlinPrinterPacketParser(MarlinPrinterPacketParserOptions options)
        {
            _options = options;
            _buffer = new StringBuilder();
        }

        public void ReceiveData(string data)
        {
            if(data.Contains("\n"))
            {
                foreach(var curChar in data)
                {
                    if(curChar != '\n')
                    {
                        _buffer.Append(curChar);
                    }
                    else
                    {
                        var completePacketData = _buffer.ToString();
                        _buffer.Clear();

                        PacketComplete?.Invoke(this, new PrinterPacketParserPacketCompleteEventArgs()
                        { 
                            Packet = PreProcess(completePacketData)
                        });
                    }
                }
            }
            else
            {
                _buffer.Append(data);
            }
        }

        private PrinterPacket PreProcess(string completePacketData)
        {
            var preProcessedPacketData = completePacketData;

            if (preProcessedPacketData.StartsWith("echo:"))
            {
                preProcessedPacketData = preProcessedPacketData.Substring(5);
            }

            return new PrinterPacket()
            {
                RawData = completePacketData,
                PreProcessedData = preProcessedPacketData,
                IsAck = preProcessedPacketData.Equals(MARLIN_COMMAND_ACK)
            };
        }

    }
}
