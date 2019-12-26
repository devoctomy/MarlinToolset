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

        public MarlinPrinterControllerService(ISerialPortAdapter serialPortAdapter)
        {
            _serialPortAdapter = serialPortAdapter;
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
            var lines = data.Split('\n').Select(x => x.StartsWith("echo:") ? x.Replace("echo:", string.Empty) : x);
            ReceivedData?.Invoke(this, new PrinterControllerReceivedDataEventArgs() { Lines = lines.ToList() });
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
