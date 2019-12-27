using MarlinToolset.Model;
using System;
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

        public MarlinPrinterControllerService(
            ISerialPortAdapter serialPortAdapter,
            IPrinterPacketParser printerPacketParser)
        {
            _serialPortAdapter = serialPortAdapter;
            _printerPacketParser = printerPacketParser;

            _printerPacketParser.PacketComplete += PacketParser_PacketComplete;
        }

        private void PacketParser_PacketComplete(object sender, PrinterPacketParserPacketCompleteEventArgs e)
        {
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

        public void Write(
            string data,
            Encoding encoding)
        {
            if (SerialPortAdapterRef != null)
            {
                _serialPortAdapter.Write(
                    SerialPortAdapterRef,
                    data,
                    encoding);
            }
        }

        public void Write(
            byte[] data,
            int offset,
            int count)
        {
            if (SerialPortAdapterRef != null)
            {
                _serialPortAdapter.Write(
                    SerialPortAdapterRef,
                    data,
                    offset,
                    count);
            }
        }
    }
}
