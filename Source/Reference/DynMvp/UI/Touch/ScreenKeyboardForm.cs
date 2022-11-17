using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.UI.Touch
{
    public delegate void KeyInputedDelegatge(string inputString);

    public partial class ScreenKeyboardForm : Form
    {
        private string valueName;
        public string ValueName
        {
            set { valueName = value; }
        }

        private Control linkedControl;
        public Control LinkedControl
        {
            set { linkedControl = value; }
        }

        private bool password = false;
        public bool Password
        {
            get { return password; }
            set { password = value; }
        }

        public KeyInputedDelegatge KeyInputed;

        public ScreenKeyboardForm()
        {
            InitializeComponent();
        }

        private void ScreenKeyboardForm_Load(object sender, EventArgs e)
        {
            valueText.Text = linkedControl.Text;
            TextBox linkedTextBox = linkedControl as TextBox;
            if (linkedTextBox != null)
                valueText.PasswordChar = linkedTextBox.PasswordChar;

            if (linkedControl as NumericUpDown == null)
            {
                upButton.Enabled = false;
                downButton.Enabled = false;
            }

            labelName.Text = valueName;
            valueText.Focus();
        }

        private void valueText_TextChanged(object sender, EventArgs e)
        {
            linkedControl.Text = valueText.Text;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            valueText.Text = (Convert.ToInt32(valueText.Text) - 1).ToString();
            linkedControl.Text = valueText.Text;
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            valueText.Text = (Convert.ToInt32(valueText.Text) + 1).ToString();
            linkedControl.Text = valueText.Text;
        }
    }
}
