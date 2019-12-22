using MarlinToolset.Services;
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
            //test printers configuration view
        }
    }
}
