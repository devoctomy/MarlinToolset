using MarlinToolset.Model;
using System;
using System.IO.Ports;
using System.Threading;

namespace MarlinToolset.Services
{
    public class MarlinPrinterControllerService : IPrinterControllerService
    {
        public PrinterConfigurationModel Printer { get; private set; }

        public bool IsConnected => (Printer != null);

        private SerialPort _serialPort;

        public event EventHandler<PrinterControllerReceivedDataEventArgs> ReceivedData;

        public MarlinPrinterControllerService()
        {
        }

        public void Connect(PrinterConfigurationModel printer)
        {
            if(_serialPort == null)
            {
                _serialPort = new SerialPort(
                    printer.Port,
                    printer.BaudRate);
                _serialPort.DataReceived += _serialPort_DataReceived;
                _serialPort.Open();
                Printer = printer;
            }
        }

        public void Disconnect()
        {
            if (_serialPort != null)
            {
                _serialPort.DataReceived -= _serialPort_DataReceived;
                _serialPort.Close();
                Printer = null;
            }
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string rx = _serialPort.ReadExisting();
            ReceivedData?.Invoke(this, new PrinterControllerReceivedDataEventArgs() { Data = rx });
        }
    }
}
