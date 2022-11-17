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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniScanWPF.Table.Data;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// ModelPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModelPage : Page, IMultiLanguageSupport
    {
        public ModelPage()
        {
            InitializeComponent();
            LocalizeHelper.AddListener(this);
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private bool Filter(object item)
        {
            if (string.IsNullOrEmpty(FilterTextBox.Text))
                return true;
            
            return ((ModelDescription)item).Name.IndexOf(FilterTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible == true)
            {
                ModelList.ItemsSource = null;
                ModelList.ItemsSource = SystemManager.Instance().ModelManager.ModelDescriptionList.OrderByDescending(md => md.LastModifiedDate);

                CollectionViewSource.GetDefaultView(ModelList.ItemsSource).Filter = Filter;
            }
        }

        private void NewModel_Click(object sender, RoutedEventArgs e)
        {
            ModelDescription md = (ModelDescription)SystemManager.Instance().ModelManager.CreateModelDescription();
            if(ShowModelDescriptionWindow(md,true))
            {
                SystemManager.Instance().ModelManager.AddModel(md);
                ModelList.ItemsSource = SystemManager.Instance().ModelManager.ModelDescriptionList.OrderByDescending(f => f.LastModifiedDate);
            }
        }

        private bool ShowModelDescriptionWindow(ModelDescription md, bool isNewModel)
        {
            ModelDescriptionWindow modelDescriptionWindow = new ModelDescriptionWindow(md, isNewModel);
            modelDescriptionWindow.ShowDialog();
            return (modelDescriptionWindow.DialogResult == true);

        }

        private void EditModel_Click(object sender, RoutedEventArgs e)
        {
            if (ModelList.SelectedItems.Count == 0)
                return;

            ModelDescription md = (ModelDescription)ModelList.SelectedItems[0];
            if (md == null)
                return;

            if (ShowModelDescriptionWindow(md, false))
            {
                SystemManager.Instance().ModelManager.SaveModelDescription(md);
                ModelList.ItemsSource = SystemManager.Instance().ModelManager.ModelDescriptionList.OrderByDescending(f => f.LastModifiedDate);
            }
        }

        private void DeleteModel_Click(object sender, RoutedEventArgs e)
        {
            if (ModelList.SelectedItems.Count > 0)
            {
                if (WpfControlLibrary.UI.CustomMessageBox.Show(
                    string.Format(LocalizeHelper.GetString("Are you really going to delete the models [Count : {0}] ?"), ModelList.SelectedItems.Count),
                    null, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    foreach (var md in ModelList.SelectedItems)
                    {
                        if (md != null)
                        {
                            if (SystemManager.Instance().CurrentModel != null)
                                if (SystemManager.Instance().CurrentModel.ModelDescription == md)
                                    SystemManager.Instance().CurrentModel = null;

                            SystemManager.Instance().ModelManager.DeleteModel((ModelDescription)md);
                        }
                    }
                }
            }
            ModelList.SelectedIndex = -1;
            ModelList.ItemsSource = SystemManager.Instance().ModelManager.ModelDescriptionList.OrderByDescending(f => f.LastModifiedDate);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ModelList.ItemsSource != null)
                CollectionViewSource.GetDefaultView(ModelList.ItemsSource).Refresh();
        }

        private void ModelList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ModelDescription md = (ModelDescription)((ListBox)sender).SelectedItem;
            if (md != null)
                SelectModel(md);
        }

        private void SelectModel_Click(object sender, RoutedEventArgs e)
        {
            if (ModelList.SelectedItems.Count == 0)
                return;

            ModelDescription md = (ModelDescription)(ModelList).SelectedItem;
            if (md != null)
                SelectModel(md);
        }

        private void SelectModel(ModelDescription md)
        {
            SimpleProgressWindow loadingWindow = new SimpleProgressWindow(LocalizeHelper.GetString("Load"));

            Model model = null;

            if (md != null)
            {
                loadingWindow.Show(() =>
                {
                    model = (Model)SystemManager.Instance().ModelManager.LoadModel(md, null);

                    if (model != null)
                    {
                        SystemManager.Instance().CurrentModel = model;
                        SystemManager.Instance().OperatorManager.InspectOperator.Settings.MarginMeasureParam.DesignedUm = model.ModelDescription.MarginUm;
                    }
                });
            }

            if (model != null)
            {
                if (model.IsTaught())
                    SystemManager.Instance().MainPage.Navigate(PageType.Inspect);
                else
                    SystemManager.Instance().MainPage.Navigate(PageType.Teach);
            }
        }
    }
}
