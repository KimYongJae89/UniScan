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

namespace DynMvp.UI
{
    public delegate bool InputFormValidCheckFunc(string inputValue);
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

        InputFormValidCheckFunc validCheckFunc;
        public InputFormValidCheckFunc ValidCheckFunc
        {
            get { return validCheckFunc; }
            set { validCheckFunc = value; }
        }

        public InputForm(string lableString)
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

        public InputForm(string lableString, string nowString)
        {
            InitializeComponent();

            labelTitle.Text = lableString;
            inputTextBox.Text = nowString;

            btnOk.Text = StringManager.GetString(this.GetType().FullName, btnOk);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, btnCancel);
            //labelTitle.Text = StringManager.GetString(this.GetType().FullName, labelTitle);
            //if (InputForm.ActiveForm != null)
            //    InputForm.ActiveForm.Text = StringManager.GetString(this.GetType().FullName, InputForm.ActiveForm);

            this.StartPosition = FormStartPosition.CenterScreen;
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

        }        

        private void inputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
            //    this.Close();
            //if (e.KeyCode == Keys.Enter)
            //    btnOk_Click(sender, null);
        }

        private void inputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keys key = (Keys)e.KeyChar;

            switch(key)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.Enter:
                    btnOk_Click(sender, null);
                    break;
            }
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool ok = false;
            if (validCheckFunc!=null)
            {
                ok = validCheckFunc(inputTextBox.Text);
            }
            else
            {
                ok = true;
                if (inputTextBox.Text == "")
                {
                    MessageBox.Show(StringManager.GetString("Input Data is Too Short."));
                    ok = false;
                }
            }

            try
            {
                if (ok)
                {
                    InputText = inputTextBox.Text;
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
