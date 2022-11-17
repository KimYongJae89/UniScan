using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;
using UniEye.Base.Settings;

namespace UniScanG.Gravure.UI.Setting
{
    public partial class SettingGeneralPage : UserControl, ISettingSubPage, IMultiLanguageSupport
    {
        public SettingGeneralPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            StringManager.AddListener(this);
        }
        
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize()
        {
            this.propertyGrid.SelectedObject = (Gravure.Settings.AdditionalSettings)AdditionalSettings.Instance();
            this.propertyGrid.PropertySort= PropertySort.CategorizedAlphabetical;
        }

        public void UpdateData()
        {
            if(InvokeRequired)
            {
                Invoke(new  UpdateDataDelegate(UpdateData));
                return;
            }
            this.propertyGrid.SelectedObject = (Gravure.Settings.AdditionalSettings)AdditionalSettings.Instance();
            //this.propertyGrid1.Update();
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //AdditionalSettings.Instance().Save();
        }
    }
}
