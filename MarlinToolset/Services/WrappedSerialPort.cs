using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;

namespace MarlinToolset.Services
{
    public class WrappedSerialPort : ISerialPort
    {
        public event SerialDataReceivedEventHandler DataReceived;

        public bool IsOpen { get; private set; }
        public string PortName { get; private set; }
        public int BaudRate { get; private set; }
        public SerialPort InnerPort { get; private set; }

        private bool _disposed;

        public WrappedSerialPort(
            string portName,
            int baudRate)
        {
            PortName = portName;
            BaudRate = baudRate;
            InnerPort = new SerialPort(
                portName,
                baudRate);
            InnerPort.DataReceived += DataReceived;
        }

        ~WrappedSerialPort()
        {
            Dispose(false);
        }

        [ExcludeFromCodeCoverage]
        public void Close()
        {
            InnerPort.Close();
            IsOpen = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if(disposing)
            {
                if(InnerPort != null)
                {
                    InnerPort.Dispose();
                    InnerPort = null;
                }
            }

            _disposed = true;
        }

        [ExcludeFromCodeCoverage]
        public void Open()
        {
            InnerPort.Open();
            IsOpen = true;
        }

        [ExcludeFromCodeCoverage]
        public string ReadExisting()
        {
            return InnerPort.ReadExisting();
        }

        [ExcludeFromCodeCoverage]
        public void Write(
            byte[] data,
            int offset,
            int count)
        {
            InnerPort.Write(
                data,
                offset,
                count);
        }
    }
}
