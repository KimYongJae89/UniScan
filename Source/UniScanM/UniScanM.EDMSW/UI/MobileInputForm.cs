using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.UI.Touch;
using DynMvp.Base;

namespace UniScanM.EDMSW.UI
{
    public delegate bool InputFormValidCheckFunc(string inputValue);
    public partial class MobileInputForm : Form
    {
        string inputText;
        public string InputText
        {
            get { return inputText; }
            set { inputText = value; }
        }

        string lotNoText;
        public string LotNoText
        {
            get { return lotNoText; }
            set { lotNoText = value; }
        }

        double speedText;
        public double SpeedText
        {
            get { return speedText; }
            set { speedText = value; }
        }
        int maxDistance;
        public int MaxDistance
        {
            get { return maxDistance; }
            set { maxDistance = value; }
        }

        string workerText;
        public string WorkerText
        {
            get { return workerText; }
            set { workerText = value; }
        }

        string modelText;
        public string ModelText
        {
            get { return modelText; }
            set { modelText = value; }
        }

        string nowString;
        public string NowString
        {
            get { return nowString; }
            set { nowString = value; }
        }

        InputFormValidCheckFunc validCheckFunc;

        public InputFormValidCheckFunc ValidCheckFunc
        {
            get { return validCheckFunc; }
            set { validCheckFunc = value; }
        }

        public MobileInputForm(string lableString)
        {
            InitializeComponent();

            labelTitle.Text = lableString;
            btnOk.Text = StringManager.GetString(this.GetType().FullName, btnOk);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, btnCancel);
            labelTitle.Text = StringManager.GetString(this.GetType().FullName, labelTitle);
            //if (InputForm.ActiveForm != null)
            //    InputForm.ActiveForm.Text = StringManager.GetString(this.GetType().FullName, InputForm.ActiveForm);

            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public MobileInputForm(string labelString, string lotString, double speed, string workerString, string modelString, int maxdistance)
        {
            InitializeComponent();

            labelTitle.Text = labelString;
            lotNoTextBox.Text = lotString;
            speedTextBox.Text = speed.ToString();
            workerTextBox.Text = workerString;
            modelTextBox.Text = modelString;
            maxDistance = maxdistance;
            tbMaxDistance.Text = maxDistance.ToString();

            btnOk.Text = StringManager.GetString(this.GetType().FullName, btnOk);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, btnCancel);
            //labelTitle.Text = StringManager.GetString(this.GetType().FullName, labelTitle);
            //if (InputForm.ActiveForm != null)
            //    InputForm.ActiveForm.Text = StringManager.GetString(this.GetType().FullName, InputForm.ActiveForm);

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void inputTextBox_Enter(object sender, EventArgs e)
        {
            UpDownControl.ShowControl("Text", (Control)lotNoTextBox);
        }

        private void inputTextBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideAllControls();
        }

        public void ChangeLocation(Point point)
        {
            this.Location = point;
        }

        private void InputForm_Load(object sender, EventArgs e)
        {

        }

        private void inputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(sender, null);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool ok = false;
            if (validCheckFunc != null)
            {
                ok = validCheckFunc(lotNoTextBox.Text) && validCheckFunc(speedTextBox.Text) && validCheckFunc(workerTextBox.Text) && validCheckFunc(modelTextBox.Text);
            }
            else
            {
                ok = true;
                if (lotNoTextBox.Text == "" || speedTextBox.Text == "" || workerTextBox.Text == "" || modelTextBox.Text == "")
                {
                    MessageBox.Show(StringManager.GetString("Input Data is Too Short."));
                    ok = false;
                }
            }

            try
            {
                if (ok)
                {
                    LotNoText = lotNoTextBox.Text;
                    SpeedText = double.Parse(speedTextBox.Text);
                    WorkerText = workerTextBox.Text;
                    modelText = modelTextBox.Text;
                    maxDistance = int.Parse(tbMaxDistance.Text);
 
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(StringManager.GetString("Check Input Data."));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
