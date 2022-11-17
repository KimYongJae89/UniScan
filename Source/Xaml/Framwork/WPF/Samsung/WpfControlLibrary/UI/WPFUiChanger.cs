using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UniEye.Base.UI;

namespace WpfControlLibrary.UI
{
    public class NotifyHandler : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void Notify(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (PropertyChanged != null)
                PropertyChanged(sender, propertyChangedEventArgs);
        }
    }

    public abstract class WPFUiChanger
    {
        public abstract UIElement GetMainPage();
        public abstract UIElement GetStatusStrip();
    }
}
