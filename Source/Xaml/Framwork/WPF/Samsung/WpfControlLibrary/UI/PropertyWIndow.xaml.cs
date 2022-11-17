using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlLibrary.Helper;

namespace WpfControlLibrary.UI
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class CatecoryAttribute : System.Attribute
    {
        private string catecory;
        public string Catecory
        {
            get
            {
                return catecory;
            }
        }

        public CatecoryAttribute(string catecory)
        {
            this.catecory = catecory;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class NameAttribute : System.Attribute
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        public NameAttribute(string name)
        {
            this.name = name;
        }
    }

    public interface ISavableObj
    {
        void Save(string fileName = "");
    }

    /// <summary>
    /// PropertyWIndow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PropertyWIndow : Window, IMultiLanguageSupport
    {
        object obj;

        public PropertyWIndow(string title, object obj)
        {
            InitializeComponent();
            this.obj = obj;

            UpdatePropertyListBox(obj);

            LocalizeHelper.AddListener(this);
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void UpdatePropertyListBox(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] propertyInfoArray = type.GetProperties();
            List<Tuple<string, List<PropertyInfo>>> propertyList = new List<Tuple<string, List<PropertyInfo>>>();
            foreach (PropertyInfo propertyInfo in propertyInfoArray)
            {
                CatecoryAttribute category = propertyInfo.GetCustomAttribute(typeof(CatecoryAttribute)) as CatecoryAttribute;

                if (category == null)
                    continue;

                Tuple<string, List<PropertyInfo>> foundList = propertyList.Find(tuple => tuple.Item1 == category.Catecory.ToString());
                
                if (foundList == null)
                {
                    foundList = new Tuple<string, List<PropertyInfo>>(category.Catecory.ToString(), new List<PropertyInfo>());
                    propertyList.Add(foundList);
                }
                
                foundList.Item2.Add(propertyInfo);
            }

            foreach (Tuple<string, List<PropertyInfo>> tuple in propertyList)
            {
                propertyStack.Children.Add(CreateLabel(tuple.Item1.ToString(), 22));
                foreach (PropertyInfo propertyInfo in tuple.Item2)
                {
                    propertyStack.Children.Add(CreatePropertyGrid(obj, propertyInfo, 22));
                }
            }

            propertyStack.DataContext = obj;
        }

        private Label CreateLabel(string category, int fontSize)
        {
            Label label = new Label();
            label.Style = this.FindResource("BasicLabel") as Style;
            label.Foreground = this.FindResource("FontBrush") as Brush;
            label.Background = this.FindResource("NormalBrush") as Brush;

            label.Content = category;
            label.FontSize = fontSize;
            label.BorderBrush = Brushes.Black;
            label.BorderThickness = new Thickness(1);

            return label;
        }

        private Grid CreatePropertyGrid(object refObj, PropertyInfo propertyInfo, int fontSize)
        {
            NameAttribute nameAttribute = propertyInfo.GetCustomAttribute(typeof(NameAttribute)) as NameAttribute;

            string name = nameAttribute != null ? nameAttribute.Name : propertyInfo.Name;

            Label label = new Label();
            label.Style = this.FindResource("BasicLabel") as Style;
            label.Foreground = this.FindResource("FontBrush") as Brush;
            label.Background = this.FindResource("LightBrush") as Brush;
            label.Content = name;
            label.FontSize = fontSize;
            label.BorderBrush = Brushes.Black;
            label.BorderThickness = new Thickness(1);

            UIElement element = null;
            
            Binding binding = new Binding();
            binding.Path = new PropertyPath(propertyInfo.Name);
            switch (propertyInfo.PropertyType.Name)
            {
                case "Boolean":
                    CheckBox checkBox = new CheckBox();
                    checkBox.SetBinding(CheckBox.IsCheckedProperty, binding);
                    checkBox.Foreground = this.FindResource("FontBrush") as Brush;
                    checkBox.FontSize = fontSize;
                    checkBox.HorizontalAlignment = HorizontalAlignment.Center;
                    checkBox.HorizontalContentAlignment = HorizontalAlignment.Center;
                    checkBox.VerticalContentAlignment = VerticalAlignment.Center;
                    element = checkBox;
                    break;
                default:
                    TextBox textBox = new TextBox();
                    textBox.SetBinding(TextBox.TextProperty, binding);
                    textBox.Foreground = this.FindResource("FontBrush") as Brush;
                    textBox.FontSize = fontSize;
                    textBox.TextAlignment = TextAlignment.Center;
                    element = textBox;
                    break;
            }

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Grid.SetColumn(label, 0);
            Grid.SetColumn(element, 1);

            grid.Children.Add(label);
            grid.Children.Add(element);

            return grid;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ISavableObj savableObj = this.obj as ISavableObj;
            if (savableObj != null)
                savableObj.Save();
        }
    }
}
