using MarlinToolset.Model;
using System;

namespace MarlinToolset.Services
{
    public class MarlinPrinterControllerService : IPrinterControllerService
    {
        public PrinterConfigurationModel Printer { get; private set; }

        public bool IsConnected => (Printer != null);

        private ISerialPortAdapter _serialPortAdapter;
        private SerialPortAdapterRef _serialPortRef;

        public event EventHandler<PrinterControllerReceivedDataEventArgs> ReceivedData;

        public MarlinPrinterControllerService(ISerialPortAdapter serialPortAdapter)
        {
            _serialPortAdapter = serialPortAdapter;
        }

        public void Connect(PrinterConfigurationModel printer)
        {
            if(_serialPortRef == null)
            {
                _serialPortRef = _serialPortAdapter.Connect(
                    printer.Port,
                    printer.BaudRate,
                    new Action<SerialPortAdapterRef, string>(DataReceivedCallback));
                Printer = printer;
            }
        }

        public void Disconnect()
        {
            if (_serialPortRef != null)
            {
                _serialPortAdapter.Disconnect(_serialPortRef);
                Printer = null;
            }
        }

        private void DataReceivedCallback(
            SerialPortAdapterRef portRef,
            string data)
        {
            ReceivedData?.Invoke(this, new PrinterControllerReceivedDataEventArgs() { Data = data });
        }
    }
}
