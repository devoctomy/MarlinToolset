using Microsoft.Extensions.DependencyInjection;
using MarlinToolset.Services;
using MarlinToolset.Views;
using System;
using System.Windows;

namespace MarlinToolset
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private IPrinterConfigurationManagerService _printerConfigurationManagerService;

        public MainWindow(
            IServiceProvider serviceProvider,
            IPrinterConfigurationManagerService printerConfigurationManagerService)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            _printerConfigurationManagerService = printerConfigurationManagerService;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var printersConfigurationView = _serviceProvider.GetService<PrintersConfigurationView>();
            var result = printersConfigurationView.ShowDialog();
            if (result.HasValue)
            {

            }
        }
    }
}
