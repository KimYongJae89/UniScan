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
using System.Threading;

namespace UniScanG.Data.UI
{
    public delegate bool ValidCheckFunc(string inputValue);
    public partial class InputForm : Form
    {
        System.Windows.Forms.Timer cancellationTokenTimer = new System.Windows.Forms.Timer();

        string inputText;
        public string InputText
        {
            get { return inputText; }
        }

        string nowString;
        public string NowString
        {
            get { return nowString; }
            set { nowString = value; }
        }

        ValidCheckFunc validCheckFunc;
        public ValidCheckFunc ValidCheckFunc
        {
            get { return validCheckFunc; }
            set { validCheckFunc = value; }
        }

        CancellationToken? cancellationToken;
        public CancellationToken? CancellationToken
        {
            get { return cancellationToken; }
            set { cancellationToken = value; }
        }

        public InputForm(string lableString, string nowString = "")
        {
            InitializeComponent();

            InitializeText(lableString, nowString);

            //btnOk.DialogResult = DialogResult.OK;
            //btnCancel.DialogResult = DialogResult.Cancel;
        }

        private void InitializeText(string labelString, string nowString = "")
        {
            btnOk.Text = StringManager.GetString(this.GetType().FullName, btnOk.Text);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, btnCancel.Text);

            if (string.IsNullOrEmpty(labelString))
                labelTitle.Text = StringManager.GetString(this.GetType().FullName, labelTitle.Text);
            else
                labelTitle.Text = labelString;

            inputTextBox.Text = nowString;
            //if (InputForm.ActiveForm != null)
            //    InputForm.ActiveForm.Text = StringManager.GetString(this.GetType().FullName, InputForm.ActiveForm.Text);
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
            this.cancellationTokenTimer.Interval = 500;
            this.cancellationTokenTimer.Tick += CancellationTokenTimer_Tick;
            this.cancellationTokenTimer.Start();

            this.StartPosition = FormStartPosition.CenterParent;
            this.inputTextBox.Focus();
        }

        private void CancellationTokenTimer_Tick(object sender, EventArgs e)
        {
            if (!this.cancellationToken.HasValue)
                return;

            if (this.cancellationToken.Value.IsCancellationRequested)
            {
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void PressEnter()
        {
            if (MessageForm.Show(this, string.Format(StringManager.GetString("[{0}] is Correct?"), inputTextBox.Text), MessageFormType.YesNo) == DialogResult.Yes)
                btnOk_Click(this, null);
        }

        private void PressEscape()
        {
            if (MessageForm.Show(this, StringManager.GetString("Escape?"), MessageFormType.YesNo) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
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
                inputText = inputTextBox.Text;
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
            this.DialogResult = DialogResult.Cancel;
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
            if (validCheckFunc != null)
                return validCheckFunc(inputTextBox.Text);

            Regex rgx = new Regex(@"[A-Z0-9_]");
            MatchCollection matches = rgx.Matches(inputTextBox.Text);
            return matches.Count == inputTextBox.Text.Length;
        }

        private void InputForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                PressEscape();
            if (e.KeyCode == Keys.Enter)
                PressEnter();
        }
    }
}
