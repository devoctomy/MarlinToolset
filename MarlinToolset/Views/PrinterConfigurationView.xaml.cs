using MarlinToolset.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace MarlinToolset.Views
{
    public partial class PrinterConfigurationView : ReactiveWindow<PrinterConfigurationViewModel>
    {
        public PrinterConfigurationView()
        {
            InitializeComponent();
            ViewModel = new PrinterConfigurationViewModel(new Action(OnSave));

            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.AvailableSerialPorts,
                    view => view.AvailableSerialPorts.ItemsSource)
                .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.BaudRates,
                    view => view.BaudRates.ItemsSource)
                .DisposeWith(disposableRegistration);

                this.Bind(ViewModel,
                    viewModel => viewModel.Model.Name,
                    view => view.PrinterName.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Model.Port,
                    view => view.AvailableSerialPorts.SelectedItem)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Model.BaudRate,
                    view => view.BaudRates.SelectedItem)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Model.BedXSize,
                    view => view.BedXSize.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Model.BedYSize,
                    view => view.BedYSize.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Model.BedZSize,
                    view => view.BedZSize.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Model.PrintableAreaMarginBack,
                    view => view.PrintableAreaMarginBack.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Model.PrintableAreaMarginFront,
                    view => view.PrintableAreaMarginFront.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Model.PrintableAreaMarginLeft,
                    view => view.PrintableAreaMarginLeft.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Model.PrintableAreaMarginRight,
                    view => view.PrintableAreaMarginRight.Text)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    this.ViewModel,
                    viewModel => viewModel.Save,
                    view => view.OkButton)
                .DisposeWith(disposableRegistration);
            });
        }

        private void OnSave()
        {
            DialogResult = true;
            Close();
        }
    }
}
