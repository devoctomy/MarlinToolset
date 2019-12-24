using MarlinToolset.Model;
using MarlinToolset.Services;
using MarlinToolset.Views;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using System;
using System.Reactive;
using System.Windows;

namespace MarlinToolset.ViewModels
{
    public class PrintersConfigurationViewModel : ReactiveObject, IValidatableViewModel
    {
        public event EventHandler<EventArgs> Saved;

        public ValidationContext ValidationContext { get; }
        public ReactiveCommand<Unit, Unit> Save { get; }
        public ReactiveCommand<Unit, Unit> Add { get; }
        public ReactiveCommand<Unit, Unit> Remove { get; }
        public ReactiveCommand<Unit, Unit> Clear { get; }
        public PrinterConfigurationModel SelectedPrinter { get; set; }
        public IPrinterConfigurationManagerService PrinterConfigurationManagerService { get; set; }

        public PrintersConfigurationViewModel(IPrinterConfigurationManagerService printerConfigurationManagerService)
        {
            PrinterConfigurationManagerService = printerConfigurationManagerService;
            ValidationContext = new ValidationContext();

            Add = ReactiveCommand.Create(new Action(OnAdd));
            Remove = ReactiveCommand.Create(new Action(OnRemove));
            Clear = ReactiveCommand.Create(new Action(OnClear));
            Save = ReactiveCommand.Create(new Action(OnSave), this.IsValid());
        }

        private void OnAdd()
        {
            var printerConfigurationView = new PrinterConfigurationView();
            var result = printerConfigurationView.ShowDialog();
            if (result.HasValue)
            {
                PrinterConfigurationManagerService.Add(printerConfigurationView.ViewModel.Model);
            }
        }

        private void OnRemove()
        {
            if (SelectedPrinter != null)
            {
                if(MessageBox.Show(
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
    }
}
