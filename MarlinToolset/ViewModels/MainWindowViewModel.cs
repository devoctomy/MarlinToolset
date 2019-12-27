using Microsoft.Extensions.DependencyInjection;
using MarlinToolset.Services;
using MarlinToolset.Views;
using ReactiveUI;
using ReactiveUI.Validation.Contexts;
using System;
using System.Reactive;
using ReactiveUI.Validation.Abstractions;
using MarlinToolset.Model;
using System.Windows;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;

namespace MarlinToolset.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IValidatableViewModel
    {
        public ValidationContext ValidationContext { get; }
        public ReactiveCommand<Unit, Unit> ConfigurePrinters { get; }
        public ReactiveCommand<Unit, Unit> ConnectToggle { get; }
        public PrinterConfigurationModel SelectedPrinter { get; set; }
        public ListBox TerminalListBox { get; set; }
        public bool IsConnected { get; private set; }
        public IPrinterConfigurationManagerService PrinterConfigurationManagerService { get; set; }

        public ObservableCollection<PrinterPacket> Packets { get; }

        private readonly IServiceProvider _serviceProvider;
        private readonly IPrinterControllerService _printerControllerService;

        public MainWindowViewModel(
            IServiceProvider serviceProvider,
            IPrinterConfigurationManagerService printerConfigurationManagerService,
            IPrinterControllerService printerControllerService)
        {
            _serviceProvider = serviceProvider;
            _printerControllerService = printerControllerService;
            PrinterConfigurationManagerService = printerConfigurationManagerService;
            ValidationContext = new ValidationContext();

            printerControllerService.ReceivedData += PrinterControllerService_ReceivedData;

            ConfigurePrinters = ReactiveCommand.Create(new Action(OnConfigurePrinters));
            ConnectToggle = ReactiveCommand.Create(new Action(OnConnectToggle));
            Packets = new ObservableCollection<PrinterPacket>();
        }

        private void PrinterControllerService_ReceivedData(
            object sender,
            PrinterControllerReceivedDataEventArgs e)
        {
            TerminalListBox.Dispatcher.Invoke(() =>
            {
                Packets.Add(e.Packet);

                Border border = (Border)VisualTreeHelper.GetChild(TerminalListBox, 0);
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
                //TerminalListBox.SelectedIndex = TerminalListBox.Items.Count - 1;
                //TerminalListBox.ScrollIntoView(TerminalListBox.SelectedItem);
            });
        }

        private void OnConfigurePrinters()
        {
            var printersConfigurationView = _serviceProvider.GetService<IPrintersConfigurationView>();
            printersConfigurationView.Owner = Application.Current?.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var result = printersConfigurationView.ShowDialog();
            if (result.HasValue && result.Value)
            {
                if(!PrinterConfigurationManagerService.Config.Printers.Contains(SelectedPrinter))
                {
                    SelectedPrinter = null;
                }
            }
        }

        private void OnConnectToggle()
        {
            if(SelectedPrinter != null)
            {
                if(_printerControllerService.IsConnected)
                {
                    _printerControllerService.Disconnect();
                }
                else
                {
                    _printerControllerService.Connect(SelectedPrinter);
                }
            }
        }
    }
}
