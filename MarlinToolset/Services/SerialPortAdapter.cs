using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace MarlinToolset.Services
{
    public class SerialPortAdapter : ISerialPortAdapter
    {
        private ConcurrentDictionary<SerialPortAdapterRef, SerialPort> _portsByRef;
        private ConcurrentDictionary<SerialPort, SerialPortAdapterRef> _refsByPort;

        public SerialPortAdapter()
        {
            _portsByRef = new ConcurrentDictionary<SerialPortAdapterRef, SerialPort>();
            _refsByPort = new ConcurrentDictionary<SerialPort, SerialPortAdapterRef>();
        }

        public SerialPortAdapterRef Connect(
            string port,
            int baudRate,
            Action<SerialPortAdapterRef, string> dataReceivedCallback)
        {
            var portRef = new SerialPortAdapterRef(
                port,
                baudRate,
                dataReceivedCallback);

            var serialPort = new SerialPort(
                port,
                baudRate);
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();

            _portsByRef.TryAdd(portRef, serialPort);
            _refsByPort.TryAdd(serialPort, portRef);
            return portRef;
        }

        public void Disconnect(SerialPortAdapterRef portRef)
        {
            if(_portsByRef.ContainsKey(portRef))
            {
                var serialPort = _portsByRef[portRef];
                serialPort.Close();

                _portsByRef.TryRemove(portRef, out var removedPort);
                _refsByPort.TryRemove(serialPort, out var removedRef);
            }
            else
            {
                //throw exception
            }
        }

        public void Write(
            SerialPortAdapterRef portRef,
            string data,
            Encoding encoding)
        {
            var bytes = encoding.GetBytes(data);
            Write(
                portRef,
                bytes,
                0,
                bytes.Length);
        }

        public void Write(
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
            }
            else
            {
                //throw exception
            }
        }

        private void SerialPort_DataReceived(
            object sender,
            SerialDataReceivedEventArgs e)
        {
            if (_refsByPort.TryGetValue((SerialPort)sender, out var portRef))
            {
                var serialPort = (SerialPort)sender;
                portRef.DataReceivedCallback(
                    portRef,
                    serialPort.ReadExisting());
            }
            else
            {

            }
        }

    }
}
