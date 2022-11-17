using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoParamTuner.Base
{
    internal abstract class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public abstract void Save(string xmlFile);
        public abstract void Load(string xmlFile);

        internal T Get<T>(string propName)
        {
            PropertyInfo propertyInfo = this.GetType().GetProperty(propName);
            return (T)propertyInfo.GetValue(this);
        }

        internal void Set<T>(string propName, T value)
        {
            PropertyInfo propertyInfo = this.GetType().GetProperty(propName);

            T getValue = (T)propertyInfo.GetValue(this);
            if (getValue == null && value == null)
                return;

            if (getValue != null && getValue.Equals(value))
                return;

            propertyInfo.SetValue(this, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
