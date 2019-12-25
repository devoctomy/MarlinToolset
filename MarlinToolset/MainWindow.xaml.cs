using Microsoft.Extensions.DependencyInjection;
using MarlinToolset.Services;
using System;
using ReactiveUI;
using MarlinToolset.ViewModels;
using System.Reactive.Disposables;

namespace MarlinToolset
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            ViewModel = serviceProvider.GetService<MainWindowViewModel>();

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

                this.BindCommand(
                    this.ViewModel,
                    viewModel => viewModel.ConnectToggle,
                    view => view.ConnectToggleButton)
                .DisposeWith(disposableRegistration);
            });
        }
    }
}
