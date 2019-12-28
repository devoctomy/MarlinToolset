using MarlinToolset.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarlinToolset.Services
{
    public class MarlinPrinterControllerService : IPrinterControllerService
    {
        

        public event EventHandler<PrinterControllerReceivedDataEventArgs> ReceivedData;

        public PrinterConfigurationModel Printer { get; private set; }
        public SerialPortAdapterRef SerialPortAdapterRef { get; private set; }
        public bool IsConnected => (Printer != null);

        private readonly ISerialPortAdapter _serialPortAdapter;
        private readonly IPrinterPacketParser _printerPacketParser;
        private readonly Stack<PrinterCommand> _commandStack;

        public MarlinPrinterControllerService(
            ISerialPortAdapter serialPortAdapter,
            IPrinterPacketParser printerPacketParser)
        {
            _serialPortAdapter = serialPortAdapter;
            _printerPacketParser = printerPacketParser;
            _printerPacketParser.PacketComplete += PacketParser_PacketComplete;
            _commandStack = new Stack<PrinterCommand>();
        }

        private void PacketParser_PacketComplete(object sender, PrinterPacketParserPacketCompleteEventArgs e)
        {
            if(_commandStack.Count > 0 && !_commandStack.Peek().Acknowledged)
            {
                _commandStack.Peek().CommandQueue.Enqueue(e.Packet);
                if (e.Packet.IsAck)
                {
                    _commandStack.Peek().Acknowledged = true;
                }
            }
            ReceivedData?.Invoke(this, new PrinterControllerReceivedDataEventArgs() { Packet = e.Packet });
        }

        public void Connect(PrinterConfigurationModel printer)
        {
            if(SerialPortAdapterRef == null)
            {
                SerialPortAdapterRef = _serialPortAdapter.Connect(
                    printer,
                    new Action<SerialPortAdapterRef, string>(DataReceivedCallback));
                Printer = printer;
            }
        }

        public void Disconnect()
        {
            if (SerialPortAdapterRef != null)
            {
                _serialPortAdapter.Disconnect(SerialPortAdapterRef);
                SerialPortAdapterRef = null;
                Printer = null;
            }
        }

        private void DataReceivedCallback(
            SerialPortAdapterRef portRef,
            string data)
        {
            _printerPacketParser.ReceiveData(data);
        }

        public PrinterCommand Write(
            string data,
            Encoding encoding)
        {
            if (!WaitingOnPreviousCommand() && SerialPortAdapterRef != null)
            {
                var dataBytes = encoding.GetBytes(data);
                return OnWrite(
                    dataBytes,
                    0,
                    dataBytes.Length);
            }
            else
            {
                return null;
            }
        }

        public PrinterCommand Write(
            byte[] data,
            int offset,
            int count)
        {
            if (!WaitingOnPreviousCommand() && SerialPortAdapterRef != null)
            {
                return OnWrite(
                    data,
                    offset,
                    count);
            }
            else
            {
                return null;
            }
        }

        private PrinterCommand OnWrite(
            byte[] data,
            int offset,
            int count)
        {
            _serialPortAdapter.Write(
                SerialPortAdapterRef,
                data,
                offset,
                count);

            var command = new PrinterCommand()
            {
                Data = data,
                Offset = offset,
                Count = count,
                Acknowledged = false
            };
            
            _commandStack.Push(command);
            return command;
        }

        private bool WaitingOnPreviousCommand()
        {
            if(_commandStack.Count > 0)
            {
                return !_commandStack.Peek().Acknowledged;
            }

            return false;
        }

        public void ClearCommandStack()
        {
            _commandStack.Clear();
        }
    }
}
