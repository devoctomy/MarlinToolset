using Microsoft.Extensions.DependencyInjection;
using MarlinToolset.Services;
using MarlinToolset.Views;
using ReactiveUI;
using ReactiveUI.Validation.Contexts;
using System;
using System.Reactive;
using ReactiveUI.Validation.Abstractions;
using MarlinToolset.Model;

namespace MarlinToolset.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IValidatableViewModel
    {
        public ValidationContext ValidationContext { get; }
        public ReactiveCommand<Unit, Unit> ConfigurePrinters { get; }
        public PrinterConfigurationModel SelectedPrinter { get; set; }
        public IPrinterConfigurationManagerService PrinterConfigurationManagerService { get; set; }

        private readonly IServiceProvider _serviceProvider;

        public MainWindowViewModel(
            IServiceProvider serviceProvider,
            IPrinterConfigurationManagerService printerConfigurationManagerService)
        {
            _serviceProvider = serviceProvider;
            PrinterConfigurationManagerService = printerConfigurationManagerService;
            ValidationContext = new ValidationContext();

            ConfigurePrinters = ReactiveCommand.Create(new Action(OnConfigurePrinters));
        }

        private void OnConfigurePrinters()
        {
            var printersConfigurationView = _serviceProvider.GetService<PrintersConfigurationView>();
            var result = printersConfigurationView.ShowDialog();
            if (result.HasValue)
            {
                if(!PrinterConfigurationManagerService.Config.Printers.Contains(SelectedPrinter))
                {
                    SelectedPrinter = null;
                }
            }
        }
    }
}
