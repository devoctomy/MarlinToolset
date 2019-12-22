using MarlinToolset.Services;
using MarlinToolset.Views;
using System.Windows;

namespace MarlinToolset
{
    public partial class MainWindow : Window
    {
        private IPrinterConfigurationManagerService _printerConfigurationManagerService;

        public MainWindow(IPrinterConfigurationManagerService printerConfigurationManagerService)
        {
            InitializeComponent();

            _printerConfigurationManagerService = printerConfigurationManagerService;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var printersConfigurationView = new PrintersConfigurationView(); // _printerConfigurationManagerService);
            var result = printersConfigurationView.ShowDialog();
            if (result.HasValue)
            {

            }
        }
    }
}
