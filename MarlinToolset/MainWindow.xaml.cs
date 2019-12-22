using MarlinToolset.Services;
using MarlinToolset.Views;
using System.Windows;

namespace MarlinToolset
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            var printerConfigurationView = new PrinterConfigurationView();
            var result = printerConfigurationView.ShowDialog();
            if(result.HasValue)
            {
                _printerConfigurationManagerService.Add(printerConfigurationView.ViewModel.Model);
            }
        }
    }
}
