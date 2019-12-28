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
using System.Windows.Controls;
using System.Windows.Media;
using System.Text;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;

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
        [Reactive] public string CommandText { get; set; }
        public ReactiveCommand<Unit, Unit> Send { get; }
        public List<string> CommandHistory { get; }
        public ReactiveCommand<Unit, Unit> PreviousCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NextCommand { get; set; }

        private readonly IServiceProvider _serviceProvider;
        private readonly IPrinterControllerService _printerControllerService;
        private int _historyIndex = 0;

        public MainWindowViewModel(
            IServiceProvider serviceProvider,
            IPrinterConfigurationManagerService printerConfigurationManagerService,
            IPrinterControllerService printerControllerService)
        {
            _serviceProvider = serviceProvider;
            _printerControllerService = printerControllerService;
            PrinterConfigurationManagerService = printerConfigurationManagerService;
            ValidationContext = new ValidationContext();
            CommandHistory = new List<string>();

            printerControllerService.ReceivedData += PrinterControllerService_ReceivedData;

            ConfigurePrinters = ReactiveCommand.Create(new Action(OnConfigurePrinters));
            ConnectToggle = ReactiveCommand.Create(new Action(OnConnectToggle));
            Packets = new ObservableCollection<PrinterPacket>();
            Send = ReactiveCommand.Create(new Action(OnSend));
            PreviousCommand = ReactiveCommand.Create(new Action(OnPreviousCommand));
            NextCommand = ReactiveCommand.Create(new Action(OnNextCommand));
        }

        private void PrinterControllerService_ReceivedData(
            object sender,
            PrinterControllerReceivedDataEventArgs e)
        {
            TerminalListBox.Dispatcher.Invoke(() =>
            {
                Packets.Add(e.Packet);
                if(VisualTreeHelper.GetChildrenCount(TerminalListBox) > 0)
                {
                    Border border = (Border)VisualTreeHelper.GetChild(TerminalListBox, 0);
                    ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                    scrollViewer.ScrollToBottom();
                }
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

        private void OnSend()
        {
            if (SelectedPrinter != null && _printerControllerService.IsConnected)
            {
                var data = CommandText;
                CommandText = string.Empty;
                _printerControllerService.Write(
                    $"{data}\n",
                    Encoding.ASCII);
                CommandHistory.Add(data);
                _historyIndex = 0;
            }
        }

        private void OnPreviousCommand()
        {
            if(_historyIndex < CommandHistory.Count)
            {
                _historyIndex += 1;
                var index = CommandHistory.Count - _historyIndex;
                CommandText = CommandHistory[index];
            }
        }

        private void OnNextCommand()
        {
            if (_historyIndex > 1)
            {
                _historyIndex -= 1;
                var index = CommandHistory.Count - _historyIndex;
                CommandText = CommandHistory[index];
            }
        }
    }
}
