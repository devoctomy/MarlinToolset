using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MarlinToolset.Views
{
    public interface IPrintersConfigurationView
    {
        Window Owner { get; set; }
        bool? ShowDialog();
    }
}
