using MarlinToolset.Model;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Reactive;

namespace MarlinToolset.ViewModels
{
    public class PrinterConfigurationViewModel : ReactiveObject, IValidatableViewModel
    {
        public event EventHandler<EventArgs> Saved;
        public event EventHandler<EventArgs> Cancelled;

        public ValidationContext ValidationContext { get; }

        public PrinterConfigurationModel Model { get; private set; }

        public IEnumerable<string> AvailableSerialPorts => SerialPort.GetPortNames();

        public IEnumerable<int> BaudRates => new[]
        {
            300,
            600,
            1200,
            2400,
            4800,
            9600,
            19200,
            38400,
            57600,
            115200,
            230400,
            460800,
            921600
        };

        public ReactiveCommand<Unit, Unit> Save { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public PrinterConfigurationViewModel()
        {
            Model = new PrinterConfigurationModel();
            ValidationContext = new ValidationContext();

            this.ValidationRule(
                viewModel => viewModel.Model.Name,
                value => !string.IsNullOrWhiteSpace(value),
                "You must specify a valid printer name.");

            this.ValidationRule(
                viewModel => viewModel.Model.Port,
                value => !string.IsNullOrWhiteSpace(value),
                "You must specify a valid serial port.");

            this.ValidationRule(
                viewModel => viewModel.Model.BaudRate,
                value => value > 0,
                "You must specify a valid serial port.");

            this.ValidationRule(
                viewModel => viewModel.Model.BedXSize,
                value => value > 0,
                "You must specify a valid X bed size.");

            this.ValidationRule(
                viewModel => viewModel.Model.BedYSize,
                value => value > 0,
                "You must specify a valid Y bed size.");

            this.ValidationRule(
                viewModel => viewModel.Model.BedZSize,
                value => value > 0,
                "You must specify a valid Z bed size.");

            this.ValidationRule(
                viewModel => viewModel.Model.PrintableAreaMarginBack,
                value => value >= 0,
                "You must specify a valid printable area back margin.");

            this.ValidationRule(
                viewModel => viewModel.Model.PrintableAreaMarginFront,
                value => value >= 0,
                "You must specify a valid printable area front margin.");

            this.ValidationRule(
                viewModel => viewModel.Model.PrintableAreaMarginLeft,
                value => value >= 0,
                "You must specify a valid printable area left margin.");

            this.ValidationRule(
                viewModel => viewModel.Model.PrintableAreaMarginRight,
                value => value >= 0,
                "You must specify a valid printable area right margin.");

            Save = ReactiveCommand.Create(OnSave, this.IsValid());
            Cancel = ReactiveCommand.Create(OnCancel);
        }

        private void OnSave()
        {
            Saved?.Invoke(this, EventArgs.Empty);
        }

        private void OnCancel()
        {
            Cancelled?.Invoke(this, EventArgs.Empty);
        }

    }
}
