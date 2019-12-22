using MarlinToolset.Services;
using MarlinToolset.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace MarlinToolset.Views
{
    public partial class PrintersConfigurationView : ReactiveWindow<PrintersConfigurationViewModel>
    {
        public PrintersConfigurationView()
        {
            InitializeComponent();
            ViewModel = new PrintersConfigurationViewModel(
                (IPrinterConfigurationManagerService)App.ServiceProvider.GetService(typeof(IPrinterConfigurationManagerService)),
                new Action(OnSave));

            this.WhenActivated(disposableRegistration =>
            {

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
