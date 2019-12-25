using MarlinToolset.Model;
using MarlinToolset.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarlinToolset.UnitTests.Services
{
    public class TestableSerialPortAdapter : ISerialPortAdapter
    {
        public IReadOnlyList<SerialPortAdapterRef> PortRefs => null;
        public List<SerialPortAdapterWriteStringData> WrittenStringData { get; private set; }
        public List<SerialPortAdapterWriteBinaryData> WrittenBinaryData { get; private set; }
        public PrinterConfigurationModel Config { get; private set; }
        public Action<SerialPortAdapterRef, string> Callback { get; private set; }
        public SerialPortAdapterRef SerialPortAdapterRef { get; private set; }

        public ISerialPort GetSerialPort(SerialPortAdapterRef portRef)
        {
            return null;
        }

        public SerialPortAdapterRef Connect(
            PrinterConfigurationModel config,
            Action<SerialPortAdapterRef, string> dataReceivedCallback)
        {
            WrittenStringData = new List<SerialPortAdapterWriteStringData>();
            WrittenBinaryData = new List<SerialPortAdapterWriteBinaryData>();
            Config = config;
            Callback = dataReceivedCallback;
            SerialPortAdapterRef = new SerialPortAdapterRef(
                config,
                dataReceivedCallback);
            return SerialPortAdapterRef;
        }

        public void Disconnect(SerialPortAdapterRef portRef)
        {
            if(portRef == SerialPortAdapterRef)
            {
                Config = null;
                Callback = null;
                SerialPortAdapterRef = null;
            }
        }

        public void Write(
            SerialPortAdapterRef portRef,
            string data,
            Encoding encoding)
        {
            if(portRef == SerialPortAdapterRef)
            {
                WrittenStringData.Add(new SerialPortAdapterWriteStringData()
                {
                    Data = data,
                    Encoding = encoding
                });
            }
        }

        public void Write(
            SerialPortAdapterRef portRef,
            byte[] data,
            int offset,
            int count)
        {
            if (portRef == SerialPortAdapterRef)
            {
                WrittenBinaryData.Add(new SerialPortAdapterWriteBinaryData()
                {
                    Data = data,
                    Offset = offset,
                    Count = count
                });
            }
        }

        public void FakeReceiveData(
            SerialPortAdapterRef portRef,
            string data)
        {
            if (portRef == SerialPortAdapterRef)
            {
                Callback(
                    portRef,
                    data);
            }
        }
    }
}
