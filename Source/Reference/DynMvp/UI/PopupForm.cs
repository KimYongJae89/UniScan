using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.UI
{
    public partial class PopupForm : Form
    {
        public PopupForm()
        {
            InitializeComponent();
            Screen screen = Screen.FromControl(this);
            Location = new Point(screen.Bounds.Right - Width, screen.Bounds.Bottom - Height);
            message.Text = StringManager.GetString(this.GetType().FullName, message);
        }

        public void Show(string title, string message)
        {
            Text = title;
            this.message.Text = message;
            Show();
        }
        
        private void PopupForm_Load(object sender, EventArgs e)
        {
            showTimer.Start();
        }

        private void PopupForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }

        private void showTimer_Tick(object sender, EventArgs e)
        {
            showTimer.Stop();
            Close();
        }
    }
}
