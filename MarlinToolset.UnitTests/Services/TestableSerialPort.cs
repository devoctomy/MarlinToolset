using MarlinToolset.Services;
using System;
using System.IO.Ports;

namespace MarlinToolset.UnitTests.Services
{
    public class TestableSerialPort : ISerialPort
    {
        public event SerialDataReceivedEventHandler DataReceived;
        public bool IsOpen { get; private set; }
        public string PortName { get; private set; }
        public int BaudRate { get; private set; }
        public bool Disposed { get; private set; }
        public Action<byte[], int, int> WriteCallback { get; set; }

        private bool _disposed;
        private string _pendingData = string.Empty;

        public TestableSerialPort(
            string portName,
            int baudRate)
        {
            PortName = portName;
            BaudRate = baudRate;
        }

        ~TestableSerialPort()
        {
            Dispose(false);
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
                Disposed = true;
            }

            _disposed = true;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public void Open()
        {
            IsOpen = true;
        }

        public string ReadExisting()
        {
            var existing = _pendingData;
            _pendingData = string.Empty;
            return existing;
        }

        public void Write(
            byte[] data,
            int offset,
            int count)
        {
            WriteCallback(
                data,
                offset,
                count);
        }

        public void FakeReceiveData(string data)
        {
            _pendingData += data;
            DataReceived?.Invoke(
                this,
                null);
        }
    }
}
