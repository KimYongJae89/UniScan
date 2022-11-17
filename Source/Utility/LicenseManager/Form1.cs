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

namespace LicenseManager
{
    public partial class Form1 : Form
    {
        public static string PASSWD => "Vz3j+N7CJ3z7ROCZ+92LRQ=="; // UniEye7880!

        BindingList<string> list  = new BindingList<string>();

        public Form1()
        {
            InitializeComponent();

            this.listBox1.DataSource = this.list;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = DynMvp.Base.LicenseManager.GetBaseboardID(8);
            this.comboBox1.Items.AddRange(Enum.GetNames(typeof(DynMvp.Base.LicenseManager.ELicenses)));
            this.comboBox1.Enabled = false;
#if DEBUG
            this.textBox3.Text = "UniEye7880!";
#endif
        }

        private void CheckAuth()
        {
            byte[] bytes = DynMvp.Base.LicenseManager.Encrypt("00000000", Encoding.Default.GetBytes(textBox3.Text));
            string base64 = Convert.ToBase64String(bytes);
            if (base64 != PASSWD)
                throw new UnauthorizedAccessException("Access Denied");
        }

        private void Command(bool checkAuth, Action action)
        {
            try
            {
                if (checkAuth)
                    CheckAuth();

                action.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            Command(true, new Action(() =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Filter = @"Key File|*.key",
                    FileName = "License.key",
                };

                if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                    return;

                DynMvp.Base.LicenseManager.SaveAs(saveFileDialog.FileName, this.textBox1.Text);
            }));
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            Command(false, new Action(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    InitialDirectory = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Config")),
                    Filter = @"Key File|*.key",
                    FileName = "License.key",
                };

                if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                    return;

                DynMvp.Base.LicenseManager.Load(openFileDialog.FileName, this.textBox1.Text);
                UpdateList();
            }));
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            this.comboBox1.Text = (string)listBox1.SelectedItem;
        }

        private void UpdateList()
        {
            int selectedIndex = this.listBox1.SelectedIndex;
            //this.listBox1.SelectedIndex = -1;
            this.list.Clear();
            Array.ForEach(DynMvp.Base.LicenseManager.Licenses, f => this.list.Add(f));
            this.listBox1.ClearSelected();
            this.listBox1.SelectedIndex = Math.Min(this.list.Count - 1, selectedIndex);
            //textBox2.Text = "";
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Command(true, new Action(() =>
            {
                DynMvp.Base.LicenseManager.Add(this.comboBox1.Text);
                UpdateList();
            }));
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            Command(true, new Action(() =>
            {
                DynMvp.Base.LicenseManager.Remove(this.comboBox1.Text);
                UpdateList();
            }));
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            Command(true, new Action(() =>
            {
                string data = textBox2.Text;
                string[] tokens = data.Split(';');
                Array.ForEach(tokens, f => DynMvp.Base.LicenseManager.Add(f));
                UpdateList();
            }));
        }

        private void buttonGet_Click(object sender, EventArgs e)
        {
            textBox2.Text = DynMvp.Base.LicenseManager.GetString();
        }

        private void buttonKeyDefault_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = DynMvp.Base.LicenseManager.GetBaseboardID(8);
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            byte[] bytes = DynMvp.Base.LicenseManager.Encrypt("00000000", Encoding.Default.GetBytes(textBox3.Text));
            string base64 = Convert.ToBase64String(bytes);
            bool isAuthorized = (base64 == PASSWD);
            this.comboBox1.Enabled = isAuthorized;
            this.buttonSet.Enabled = isAuthorized;
            this.buttonGet.Enabled = isAuthorized;
            this.buttonAdd.Enabled = isAuthorized;
            this.buttonRemove.Enabled = isAuthorized;
        }
    }
}
