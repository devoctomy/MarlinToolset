using Microsoft.Extensions.DependencyInjection;
using MarlinToolset.Model;
using MarlinToolset.Services;
using MarlinToolset.Views;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using System;
using System.Linq;
using System.Reactive;
using System.Windows;

namespace MarlinToolset.ViewModels
{
    public class PrintersConfigurationViewModel : ReactiveObject, IValidatableViewModel
    {
        public event EventHandler<EventArgs> Saved;
        public event EventHandler<EventArgs> Cancelled;

        public ValidationContext ValidationContext { get; }
        public ReactiveCommand<Unit, Unit> Add { get; }
        public ReactiveCommand<Unit, Unit> Remove { get; }
        public ReactiveCommand<Unit, Unit> Clear { get; }
        public ReactiveCommand<Unit, Unit> Save { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public PrinterConfigurationModel SelectedPrinter { get; set; }
        public IPrinterConfigurationManagerService PrinterConfigurationManagerService { get; set; }

        private readonly IServiceProvider _serviceProvider;

        public PrintersConfigurationViewModel(
            IServiceProvider serviceProvider,
            IPrinterConfigurationManagerService printerConfigurationManagerService)
        {
            _serviceProvider = serviceProvider;
            PrinterConfigurationManagerService = printerConfigurationManagerService;
            ValidationContext = new ValidationContext();

            Add = ReactiveCommand.Create(new Action(OnAdd));
            Remove = ReactiveCommand.Create(new Action(OnRemove));
            Clear = ReactiveCommand.Create(new Action(OnClear));
            Save = ReactiveCommand.Create(new Action(OnSave), this.IsValid());
            Cancel = ReactiveCommand.Create(new Action(OnCancel));
        }

        private void OnAdd()
        {
            var printerConfigurationView = _serviceProvider.GetService<PrinterConfigurationView>();
            printerConfigurationView.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var result = printerConfigurationView.ShowDialog();
            if (result.HasValue && result.Value)
            {
                PrinterConfigurationManagerService.Add(printerConfigurationView.ViewModel.Model);
            }
        }

        private void OnRemove()
        {
            if (SelectedPrinter != null)
            {
                if(MessageBox.Show(
                    Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive),
                    $"Are you sure you want to remove the printer '{SelectedPrinter.Name}'?",
                    "Remove Printer",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    PrinterConfigurationManagerService.Remove(SelectedPrinter);
                    SelectedPrinter = null;
                }
            }
        }

        private void OnClear()
        {
            PrinterConfigurationManagerService.Clear();
        }

        private void OnSave()
        {
            PrinterConfigurationManagerService.Save();
            Saved?.Invoke(this, EventArgs.Empty);
        }

        private void OnCancel()
        {
            Cancelled?.Invoke(this, EventArgs.Empty);
        }
    }
}
