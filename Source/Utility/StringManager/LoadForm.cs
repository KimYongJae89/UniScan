using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StringManager
{
    public partial class LoadForm : Form
    {
        public List<StringTable> RefStringTableList => this.refStringTableList;
        List<StringTable> refStringTableList = null;

        public List<StringTable> ComStringTableList => comStringTableList;
        List<StringTable> comStringTableList = null;

        public LoadForm()
        {
            InitializeComponent();

            string defRefPath = @"D:\Project_UniScan\UniScan\Runtime\Config\StringTable_ko-kr - 복사본.xml";
            if (File.Exists(defRefPath))
                txtRef.Text = defRefPath;

            string defComPath = @"D:\Project_UniScan\UniScan\Runtime\Config\StringTable_zh-cn - 복사본.xml";
            if (File.Exists(defComPath))
                txtComp.Text = defComPath;
        }

        private void btnBrowseRef_Click(object sender, EventArgs e)
        {
            txtRef.Text = Browse();
        }

        private void btnBrowseComp_Click(object sender, EventArgs e)
        {
            txtComp.Text = Browse();
        }

        private string Browse()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Xml Files (*.xml)|*.xml";
            if (dlg.ShowDialog() != DialogResult.OK)
                throw new OperationCanceledException();

            return dlg.FileName;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.refStringTableList = GetStringTableList(txtRef.Text);
            this.comStringTableList = GetStringTableList(txtComp.Text);

            this.DialogResult = DialogResult.OK;
        }

        private List<StringTable> GetStringTableList(string filePath)
        {
            bool ok = DynMvp.Base.StringManager.Load(filePath);
            if (!ok)
                throw new Exception("StringTable Load Fail");
            return new List<StringTable>(DynMvp.Base.StringManager.StringTableList);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
