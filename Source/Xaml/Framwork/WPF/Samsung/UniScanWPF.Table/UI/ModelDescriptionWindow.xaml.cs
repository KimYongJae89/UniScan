using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UniScanWPF.Table.Data;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// ModelDescriptionWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModelDescriptionWindow : Window, IMultiLanguageSupport
    {
        public ModelDescription ModelDescription { get; set; }
        public bool IsNewModel { get; set; }

        public ModelDescriptionWindow(ModelDescription modelDescription, bool isNewModel)
        {
            InitializeComponent();
            
            ModelDescription = modelDescription;
            IsNewModel = isNewModel;

            this.DataContext = this;

            LocalizeHelper.AddListener(this);
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsNewModel)
            {
                if (string.IsNullOrEmpty(this.ModelDescription.Name) ||
                    string.IsNullOrEmpty(this.ModelDescription.ScreenName) ||
                    this.ModelDescription.MarginUm.IsEmpty ||
                    string.IsNullOrEmpty(this.ModelDescription.Paste) ||
                    this.ModelDescription.Thickness < 0 ||
                    this.ModelDescription.Name.Any(ch => char.GetUnicodeCategory(ch) == System.Globalization.UnicodeCategory.OtherLetter) ||
                    this.ModelDescription.ScreenName.Any(ch => char.GetUnicodeCategory(ch) == System.Globalization.UnicodeCategory.OtherLetter) ||
                    this.ModelDescription.Paste.Any(ch => char.GetUnicodeCategory(ch) == System.Globalization.UnicodeCategory.OtherLetter))
                {
                    CustomMessageBox.Show(LocalizeHelper.GetString("Invalid model info."), null, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }

                if (SystemManager.Instance().ModelManager.IsModelExist(this.ModelDescription))
                {
                    CustomMessageBox.Show(LocalizeHelper.GetString("This model is exist."), null, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }
            }
            DialogResult = true;
            this.Close();
        }

        private void Cancle_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            LocalizeHelper.RemoveListener(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
 
        }
    }
}
