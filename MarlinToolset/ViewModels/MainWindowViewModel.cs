using Microsoft.Extensions.DependencyInjection;
using MarlinToolset.Services;
using MarlinToolset.Views;
using ReactiveUI;
using ReactiveUI.Validation.Contexts;
using System;
using System.Reactive;
using ReactiveUI.Validation.Abstractions;
using MarlinToolset.Model;
using System.Windows;
using System.Linq;

namespace MarlinToolset.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IValidatableViewModel
    {
        public ValidationContext ValidationContext { get; }
        public ReactiveCommand<Unit, Unit> ConfigurePrinters { get; }
        public ReactiveCommand<Unit, Unit> ConnectToggle { get; }
        public PrinterConfigurationModel SelectedPrinter { get; set; }
        public bool IsConnected { get; private set; }
        public IPrinterConfigurationManagerService PrinterConfigurationManagerService { get; set; }

        private readonly IServiceProvider _serviceProvider;
        private readonly IPrinterControllerService _printerControllerService;

        public MainWindowViewModel(
            IServiceProvider serviceProvider,
            IPrinterConfigurationManagerService printerConfigurationManagerService,
            IPrinterControllerService printerControllerService)
        {
            _serviceProvider = serviceProvider;
            _printerControllerService = printerControllerService;
            PrinterConfigurationManagerService = printerConfigurationManagerService;
            ValidationContext = new ValidationContext();

            printerControllerService.ReceivedData += PrinterControllerService_ReceivedData;

            ConfigurePrinters = ReactiveCommand.Create(new Action(OnConfigurePrinters));
            ConnectToggle = ReactiveCommand.Create(new Action(OnConnectToggle));
        }

        private void PrinterControllerService_ReceivedData(object sender, PrinterControllerReceivedDataEventArgs e)
        {
            // output the data to the screen
        }

        private void OnConfigurePrinters()
        {
            var printersConfigurationView = _serviceProvider.GetService<PrintersConfigurationView>();
            printersConfigurationView.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var result = printersConfigurationView.ShowDialog();
            if (result.HasValue && result.Value)
            {
                if(!PrinterConfigurationManagerService.Config.Printers.Contains(SelectedPrinter))
                {
                    SelectedPrinter = null;
                }
            }
        }

        private void OnConnectToggle()
        {
            if(SelectedPrinter != null)
            {
                if(_printerControllerService.IsConnected)
                {
                    _printerControllerService.Disconnect();
                }
                else
                {
                    _printerControllerService.Connect(SelectedPrinter);
                }
            }
        }
    }
}
