using Microsoft.Extensions.DependencyInjection;
using System;
using ReactiveUI;
using MarlinToolset.ViewModels;
using System.Reactive.Disposables;
using System.Diagnostics.CodeAnalysis;

namespace MarlinToolset
{
    [ExcludeFromCodeCoverage]
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

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.Packets,
                    view => view.TerminalListBox.ItemsSource)
                .DisposeWith(disposableRegistration);

                this.Bind(ViewModel,
                    viewModel => viewModel.CommandText,
                    view => view.CommandTextBox.Text)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    this.ViewModel,
                    viewModel => viewModel.Send,
                    view => view.SendButton,
                    vm => vm.CommandText)
                .DisposeWith(disposableRegistration);

                ViewModel.TerminalListBox = TerminalListBox;
            });
        }

        private void CommandTextBox_KeyDown(
            object sender,
            System.Windows.Input.KeyEventArgs e)
        {
            switch(e.Key)
            {
                case System.Windows.Input.Key.Enter:
                    {
                        ViewModel.Send.Execute().Subscribe();
                        break;
                    }
            }
        }

        private void CommandTextBox_PreviewKeyDown(
            object sender,
            System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Up:
                    {
                        ViewModel.PreviousCommand.Execute().Subscribe();
                        break;
                    }
                case System.Windows.Input.Key.Down:
                    {
                        ViewModel.NextCommand.Execute().Subscribe();
                        break;
                    }
            }
        }
    }
}
