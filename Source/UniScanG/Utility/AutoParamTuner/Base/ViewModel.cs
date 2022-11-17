using AutoParamTuner.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoParamTuner.Base
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Model Model { get; private set; }

        public ViewModel(Model model)
        {
            this.Model = model;
            this.Model.PropertyChanged += this.OnPropertyChanged;
        }

        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        public T Get<T>(string propName)
        {
            System.Reflection.PropertyInfo propertyInfo = this.GetType().GetProperty(propName);
            return (T)propertyInfo.GetValue(this);
        }

        public void Set<T>(string propName, T value)
        {
            System.Reflection.PropertyInfo propertyInfo = this.GetType().GetProperty(propName);
            T oldValue = Get<T>(propName);
            if (!IsEqual(oldValue, value))
            {
                propertyInfo.SetValue(this, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }

        private bool IsEqual<T>(T oldValue, T value)
        {
            if (oldValue == null && value == null)
                return true;

            if (oldValue != null && value != null)
                return oldValue.Equals(value);

            return false;
        }
    }
}
