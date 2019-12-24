using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Runtime.Serialization;

namespace MarlinToolset.Model
{
    [DataContract]
    public class PrinterConfigurationModel : ReactiveObject
    {
        [Reactive]
        [DataMember]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Reactive]
        [DataMember]
        public string Name { get; set; } = "New Printer";

        [Reactive]
        [DataMember]
        public string Port { get; set; }

        [Reactive]
        [DataMember] 
        public int BaudRate { get; set; }

        [Reactive]
        [DataMember] 
        public float BedXSize { get; set; } = 235.0f;

        [Reactive]
        [DataMember] 
        public float BedYSize { get; set; } = 235.0f;

        [Reactive]
        [DataMember] 
        public float BedZSize { get; set; } = 235.0f;

        [Reactive]
        [DataMember] 
        public float PrintableAreaMarginBack { get; set; }

        [Reactive]
        [DataMember] 
        public float PrintableAreaMarginFront { get; set; }

        [Reactive]
        [DataMember] 
        public float PrintableAreaMarginLeft { get; set; }

        [Reactive]
        [DataMember] 
        public float PrintableAreaMarginRight { get; set; }
    }
}
