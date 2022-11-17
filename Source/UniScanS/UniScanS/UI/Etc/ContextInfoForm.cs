using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanS.Screen.Data;

namespace UniScanS.UI.Etc
{
    public partial class ContextInfoForm : Form, IMultiLanguageSupport
    {
        SheetSubResult sheetSubResult = null;

        public ContextInfoForm()
        {
            InitializeComponent();

            this.TopMost = true;
            StringManager.AddListener(this);
        }

        private void ContextInfoForm_Load(object sender, EventArgs e)
        {
        }

        public void CanvasPanel_MouseLeaved(CanvasPanel canvasPanel)
        {
            Hide();
        }

        public void CanvasPanel_FigureFocused(Figure figure)
        {
            if (figure == null)
            {
                this.sheetSubResult = null;
                Hide();
                return;
            }

            if (figure.Tag is SheetSubResult)
            {
                this.sheetSubResult = (SheetSubResult)figure.Tag;
                this.Location = MousePosition;

                defectType.Text = StringManager.GetString(this.GetType().FullName, sheetSubResult.DefectType.ToString());
                image.Image = sheetSubResult.Image;
                Show();
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
