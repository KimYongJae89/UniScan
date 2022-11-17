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
using System.Text.RegularExpressions;

namespace UniScanS.Data.UI
{
    public partial class InputForm : Form
    {
        string inputText;
        public string InputText
        {
            get { return inputText; }
            set { inputText = value; }
        }

        string nowString;
        public string NowString
        {
            get { return nowString; }
            set { nowString = value; }
        }


        public InputForm(string lableString)
        {
            InitializeComponent();

            labelTitle.Text = lableString;
            btnOk.Text = StringManager.GetString(this.GetType().FullName, btnOk.Text);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, btnCancel.Text);
            labelTitle.Text = StringManager.GetString(this.GetType().FullName, labelTitle.Text);
            if (InputForm.ActiveForm != null)
                InputForm.ActiveForm.Text = StringManager.GetString(this.GetType().FullName, InputForm.ActiveForm.Text);

            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }

        public InputForm(string lableString, string nowString)
        {
            InitializeComponent();

            labelTitle.Text = lableString;
            btnOk.Text = StringManager.GetString(this.GetType().FullName, btnOk.Text);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, btnCancel.Text);
            labelTitle.Text = StringManager.GetString(this.GetType().FullName, labelTitle.Text);
            inputTextBox.Text = nowString;
            if (InputForm.ActiveForm != null)
                InputForm.ActiveForm.Text = StringManager.GetString(this.GetType().FullName, InputForm.ActiveForm.Text);

            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }

        private void inputTextBox_Enter(object sender, EventArgs e)
        {
            UpDownControl.ShowControl("Text", (Control)inputTextBox);
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
            inputTextBox.Text = inputText;
        }        

        private void inputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.Enter)
                btnOk_Click(sender, null);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (inputTextBox.Text.Length == 0)
            {
                inputTextBox.Select(0, inputTextBox.Text.Length);
                errorProvider.SetError(inputTextBox, "Invalid Input");
            }

            bool ok = CheckValid();
            if (ok == false)
            {
                inputTextBox.Select(0, inputTextBox.Text.Length);
                errorProvider.SetError(inputTextBox, "Invalid Input");
                return;
            }
            else
            {
                errorProvider.Clear();
            }

            try
            {
                InputText = inputTextBox.Text;
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void inputTextBox_Validating(object sender, CancelEventArgs e)
        {
            Regex rgx = new Regex(@"[A-Z0-9_]");
            MatchCollection matches = rgx.Matches(inputTextBox.Text);
            e.Cancel = matches.Count != inputTextBox.Text.Length;
            if (e.Cancel == true)
            {
                inputTextBox.Select(0, inputTextBox.Text.Length);
                errorProvider.SetError(inputTextBox, "Invalid Input");
            }
            else
            {
                errorProvider.Clear();
            }
        }

        private bool CheckValid()
        {
            Regex rgx = new Regex(@"[A-Z0-9_]");
            MatchCollection matches = rgx.Matches(inputTextBox.Text);
            return  matches.Count == inputTextBox.Text.Length;
        }
    }
}
