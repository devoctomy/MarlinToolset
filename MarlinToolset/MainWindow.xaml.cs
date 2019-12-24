using MarlinToolset.Services;
using System;
using ReactiveUI;
using MarlinToolset.ViewModels;
using System.Reactive.Disposables;

namespace MarlinToolset
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        private readonly IServiceProvider _serviceProvider;
        private IPrinterConfigurationManagerService _printerConfigurationManagerService;

        public MainWindow(
            IServiceProvider serviceProvider,
            IPrinterConfigurationManagerService printerConfigurationManagerService)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            _printerConfigurationManagerService = printerConfigurationManagerService;
            ViewModel = new MainWindowViewModel(
                _serviceProvider,
                _printerConfigurationManagerService);

            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.PrinterConfigurationManagerService.Config.Printers,
                    view => view.PrintersComboBox.ItemsSource)
                .DisposeWith(disposableRegistration);

                this.Bind(ViewModel,
                    viewModel => viewModel.SelectedPrinter,
                    view => view.PrintersComboBox.SelectedItem)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    this.ViewModel,
                    viewModel => viewModel.ConfigurePrinters,
                    view => view.ConfigurePrintersButton)
                .DisposeWith(disposableRegistration);
            });
        }
    }
}
