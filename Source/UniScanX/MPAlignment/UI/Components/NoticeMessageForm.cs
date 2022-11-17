using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniScanX.MPAlignment.UI.Components
{
    public enum DialogResultType
    {
        OK, OkCancel
    }
    public partial class NoticeMessageForm : Form
    {
        public NoticeMessageForm(string title, string message, DialogResultType dialogResultType = DialogResultType.OkCancel)
        {
            InitializeComponent();
            lblTitle.Text = title;
            txtMessage.Text = message;
            if(dialogResultType == DialogResultType.OK)
            {
                this.btnCancel.Visible = false;
            }
        }

        public NoticeMessageForm(NoticeMessageFormInfo aoiMessageFormInfo)
        {
            InitializeComponent();
            lblTitle.Text = aoiMessageFormInfo.Title;
            txtMessage.Text = aoiMessageFormInfo.Message;
            if (aoiMessageFormInfo.DialogResultType == DialogResultType.OK)
            {
                this.btnCancel.Visible = false;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    public class NoticeMessageFormInfo
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DialogResultType DialogResultType { get; set; } = DialogResultType.OkCancel;
    }
}
