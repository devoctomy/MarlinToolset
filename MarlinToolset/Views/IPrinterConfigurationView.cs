using MarlinToolset.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MarlinToolset.Views
{
    public interface IPrinterConfigurationView
    {
        Window Owner { get; set; }
        bool? ShowDialog();
        PrinterConfigurationViewModel ViewModel { get; set; }
    }
}
