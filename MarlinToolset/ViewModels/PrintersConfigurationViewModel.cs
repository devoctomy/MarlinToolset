using MarlinToolset.Services;
using MarlinToolset.Views;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using System;
using System.Reactive;

namespace MarlinToolset.ViewModels
{
    public class PrintersConfigurationViewModel : ReactiveObject, IValidatableViewModel
    {
        public ValidationContext ValidationContext { get; }
        public ReactiveCommand<Unit, Unit> Save { get; }
        public Action OnSave { get; }
        public ReactiveCommand<Unit, Unit> Add { get; }
        public ReactiveCommand<Unit, Unit> Remove { get; }
        public ReactiveCommand<Unit, Unit> Clear { get; }

        private readonly IPrinterConfigurationManagerService _printerConfigurationManagerService;

        public PrintersConfigurationViewModel(
            IPrinterConfigurationManagerService printerConfigurationManagerService,
            Action onSave)
        {
            _printerConfigurationManagerService = printerConfigurationManagerService;
            OnSave = onSave;
            ValidationContext = new ValidationContext();

            // validation logic here

            Add = ReactiveCommand.Create(new Action(OnAdd));
            Remove = ReactiveCommand.Create(new Action(OnRemove));
            Clear = ReactiveCommand.Create(new Action(OnClear));
            Save = ReactiveCommand.Create(OnSave, this.IsValid());
        }

        private void OnAdd()
        {
            var printerConfigurationView = new PrinterConfigurationView();
            var result = printerConfigurationView.ShowDialog();
            if (result.HasValue)
            {
                _printerConfigurationManagerService.Add(printerConfigurationView.ViewModel.Model);
            }
        }

        private void OnRemove()
        {

        }

        private void OnClear()
        {

        }
    }
}
