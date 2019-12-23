using Microsoft.Extensions.DependencyInjection;
using MarlinToolset.Services;
using MarlinToolset.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Windows;

namespace MarlinToolset.Views
{
    public partial class PrintersConfigurationView : ReactiveWindow<PrintersConfigurationViewModel>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPrinterConfigurationManagerService _printerConfigurationManagerService;
        public PrintersConfigurationView(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _printerConfigurationManagerService = _serviceProvider.GetService<IPrinterConfigurationManagerService>();
            ViewModel = new PrintersConfigurationViewModel(
                _printerConfigurationManagerService,
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var printerConfigurationView = _serviceProvider.GetService<PrinterConfigurationView>();
            var result = printerConfigurationView.ShowDialog();
            if (result.HasValue)
            {
                
            }
        }
    }
}
