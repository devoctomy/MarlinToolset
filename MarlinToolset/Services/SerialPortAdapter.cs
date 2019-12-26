using MarlinToolset.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace MarlinToolset.Services
{
    public class SerialPortAdapter<SerialPortType> : ISerialPortAdapter, IDisposable where SerialPortType : ISerialPort
    {
        public IReadOnlyList<SerialPortAdapterRef> PortRefs => _portsByRef.Keys.ToList();

        private readonly ConcurrentDictionary<SerialPortAdapterRef, ISerialPort> _portsByRef;
        private readonly ConcurrentDictionary<ISerialPort, SerialPortAdapterRef> _refsByPort;
        private bool _disposed;

        public SerialPortAdapter()
        {
            _portsByRef = new ConcurrentDictionary<SerialPortAdapterRef, ISerialPort>();
            _refsByPort = new ConcurrentDictionary<ISerialPort, SerialPortAdapterRef>();
        }

        public ISerialPort GetSerialPort(SerialPortAdapterRef portRef)
        {
            if (_portsByRef.TryGetValue(portRef, out var serialPort))
            {
                return serialPort;
            }
            else
            {
                return null;
            }
        }

        public SerialPortAdapterRef Connect(
            PrinterConfigurationModel config,
            Action<SerialPortAdapterRef, string> dataReceivedCallback)
        {
            var portRef = new SerialPortAdapterRef(
                config,
                dataReceivedCallback);

            var serialPort = (SerialPortType)Activator.CreateInstance(typeof(SerialPortType),
                config.Port,
                config.BaudRate);
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();

            _portsByRef.TryAdd(portRef, serialPort);
            _refsByPort.TryAdd(serialPort, portRef);
            return portRef;
        }

        public bool Disconnect(SerialPortAdapterRef portRef)
        {
            if(_portsByRef.ContainsKey(portRef))
            {
                var serialPort = _portsByRef[portRef];
                serialPort.Close();

                _portsByRef.TryRemove(portRef, out var removedPort);
                _refsByPort.TryRemove(serialPort, out var removedRef);

                serialPort.Dispose();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Write(
            SerialPortAdapterRef portRef,
            string data,
            Encoding encoding)
        {
            var bytes = encoding.GetBytes(data);
            return Write(
                portRef,
                bytes,
                0,
                bytes.Length);
        }

        public bool Write(
            SerialPortAdapterRef portRef,
            byte[] data,
            int offset,
            int count)
        {
            if (_portsByRef.ContainsKey(portRef))
            {
                var serialPort = _portsByRef[portRef];
                serialPort.Write(
                    data,
                    offset,
                    count);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SerialPort_DataReceived(
            object sender,
            SerialDataReceivedEventArgs e)
        {
            if (_refsByPort.TryGetValue((SerialPortType)sender, out var portRef))
            {
                var serialPort = (ISerialPort)sender;
                portRef.DataReceivedCallback(
                    portRef,
                    serialPort.ReadExisting());
            }
            else
            {

            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                while (_portsByRef.Count > 0)
                {
                    var portRef = _portsByRef.Keys.First();
                    if (_portsByRef.TryRemove(portRef, out var removedPort))
                    {
                        removedPort.Dispose();
                    }
                }
                _portsByRef.Clear();
                _refsByPort.Clear();
            }

            _disposed = true;
        }

    }
}
