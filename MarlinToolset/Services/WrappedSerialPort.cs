using System;
using System.IO.Ports;

namespace MarlinToolset.Services
{
    public class WrappedSerialPort : ISerialPort
    {
        public event SerialDataReceivedEventHandler DataReceived;

        public bool IsOpen { get; private set; }

        private SerialPort _serialPort;
        private bool _disposed;

        public WrappedSerialPort(
            string port,
            int baudRate)
        {
            _serialPort = new SerialPort(
                port,
                baudRate);
            _serialPort.DataReceived += DataReceived;
        }

        ~WrappedSerialPort()
        {
            Dispose(false);
        }

        public void Close()
        {
            _serialPort.Close();
            IsOpen = false;
        }

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if(disposing)
            {
                if(_serialPort != null)
                {
                    _serialPort.Dispose();
                    _serialPort = null;
                }
            }

            _disposed = true;
        }

        public void Open()
        {
            _serialPort.Open();
            IsOpen = true;
        }

        public string ReadExisting()
        {
            return _serialPort.ReadExisting();
        }

        public void Write(
            byte[] data,
            int offset,
            int count)
        {
            _serialPort.Write(
                data,
                offset,
                count);
        }
    }
}
