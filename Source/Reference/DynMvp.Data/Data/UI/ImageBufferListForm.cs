using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DynMvp.Base;
using DynMvp.Data;

namespace DynMvp.Data.UI
{
    public partial class ImageBufferListForm : Form
    {
        Model model = null;

        public ImageBufferListForm()
        {
            InitializeComponent();

            // language change
            addImageBufferButton.Text = StringManager.GetString(this.GetType().FullName,addImageBufferButton.Text);
            deleteImageBufferButton.Text = StringManager.GetString(this.GetType().FullName,deleteImageBufferButton.Text);
            moveUpButton.Text = StringManager.GetString(this.GetType().FullName,moveUpButton.Text);
            moveDownButton.Text = StringManager.GetString(this.GetType().FullName,moveDownButton.Text);
            moveDownButton.Text = StringManager.GetString(this.GetType().FullName,moveDownButton.Text);
        }

        public void UpdateImageList(Model model)
        {
            imageBufferPaths.Items.Clear();

            this.model = model;

            RefreshList();
        }

        public void RefreshList()
        {
            imageBufferPaths.Items.Clear();

            if (model == null)
                return;

            foreach (string imageBufferFile in model.ImageBufferPathList)
            {
                string fileName = Path.GetFileNameWithoutExtension(imageBufferFile);
                imageBufferPaths.Items.Add(fileName);
            }

            if (imageBufferPaths.Items.Count > 0)
                imageBufferPaths.SelectedIndex = 0;
        }

        private void addImageBufferButton_Click(object sender, EventArgs e)
        {
            if (model == null)
                return;

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                model.ImageBufferPathList.Add(dialog.SelectedPath);
                int index = imageBufferPaths.Items.Add(Path.GetFileNameWithoutExtension(dialog.SelectedPath));
                imageBufferPaths.SelectedIndex = index;

                //model.SaveModel();
            }
        }

        private void deleteImageBufferButton_Click(object sender, EventArgs e)
        {
            if (model == null)
                return;

            model.ImageBufferPathList.RemoveAt(imageBufferPaths.SelectedIndex);
            //model.SaveModel();

            RefreshList();
        }

        private void imageBufferFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (model == null )
                return;

//            model.CurrentImageBufferPath = model.ImageBufferPathList[imageBufferPaths.SelectedIndex];
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {

        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {

        }

        private void ImageBufferListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
