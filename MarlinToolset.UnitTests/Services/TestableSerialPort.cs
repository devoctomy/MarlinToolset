using MarlinToolset.Services;
using System.IO.Ports;

namespace MarlinToolset.UnitTests.Services
{
    public class TestableSerialPort : ISerialPort
    {
        public event SerialDataReceivedEventHandler DataReceived;
        public bool IsOpen { get; private set; }
        public bool Disposed { get; private set; }

        private string _portName;
        private int _baudRate;

        public TestableSerialPort(
            string portName,
            int baudRate)
        {
            _portName = portName;
            _baudRate = baudRate;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public void Dispose()
        {
            Disposed = true;
        }

        public void Open()
        {
            IsOpen = true;
        }

        public string ReadExisting()
        {
            return string.Empty;
        }

        public void Write(
            byte[] data,
            int offset,
            int count)
        {
        }
    }
}
