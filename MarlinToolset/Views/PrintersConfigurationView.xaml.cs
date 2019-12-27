using Microsoft.Extensions.DependencyInjection;
using MarlinToolset.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Diagnostics.CodeAnalysis;

namespace MarlinToolset.Views
{
    [ExcludeFromCodeCoverage]
    public partial class PrintersConfigurationView : ReactiveWindow<PrintersConfigurationViewModel>, IPrintersConfigurationView
    {
        private readonly IServiceProvider _serviceProvider;
        public PrintersConfigurationView(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            ViewModel = _serviceProvider.GetService<PrintersConfigurationViewModel>();
            ViewModel.Saved += ViewModel_Saved;
            ViewModel.Cancelled += ViewModel_Cancelled;

            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.PrinterConfigurationManagerService.Config.Printers,
                    view => view.PrintersListBox.ItemsSource)
                .DisposeWith(disposableRegistration);

                this.Bind(ViewModel,
                    viewModel => viewModel.SelectedPrinter,
                    view => view.PrintersListBox.SelectedItem)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    this.ViewModel,
                    viewModel => viewModel.Add,
                    view => view.AddButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    this.ViewModel,
                    viewModel => viewModel.Remove,
                    view => view.RemoveButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    this.ViewModel,
                    viewModel => viewModel.Clear,
                    view => view.ClearButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    this.ViewModel,
                    viewModel => viewModel.Save,
                    view => view.OkButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    this.ViewModel,
                    viewModel => viewModel.Cancel,
                    view => view.CancelButton)
                .DisposeWith(disposableRegistration);
            });
        }

        private void ViewModel_Cancelled(object sender, EventArgs e)
        {
            Close();
        }

        private void ViewModel_Saved(object sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
