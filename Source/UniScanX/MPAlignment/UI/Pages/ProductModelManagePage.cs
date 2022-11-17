using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Data;
using System.IO;
using DynMvp.Base;
using UniScanX.MPAlignment.UI.Components;

namespace UniScanX.MPAlignment.UI.Pages
{
    public partial class ProductModelManagePage : UserControl
    {

        private UniScanX.MPAlignment.Data.ModelManager GetModelManager()
        {
            return SystemManager.Instance().ModelManager as UniScanX.MPAlignment.Data.ModelManager; 
        }

        public ProductModelManagePage()
        {
            InitializeComponent();
            UpdateList();
        }
        void UpdateList()
        {
            UpdateProductModelCards();
        }

        private ProductModelCard CreateModelCard(ModelDescription desc)
        {
            var card = new ProductModelCard(desc);
            card.ModelModified = productModelCard_ModelModified;
            card.ModelCopied = productModelCard_ModelCopied;
            card.ModelDeleted = productModelCard_ModelDeleted;
            card.ModelSelected = productModelCard_ModelSelected;//double Click
            card.ModelCaptured = productModelCard_ModelCaptured;//click

            return card;
        }
        void UpdateProductModelCards()
        {
            this.SuspendLayout();
            flpBoardModelList.Controls.Clear();
            foreach (var desc in GetModelManager().ModelDescriptionList)
            {
                var card = CreateModelCard(desc as ModelDescription);
                flpBoardModelList.Controls.Add(card);
            }
            this.ResumeLayout();
        }

        private void productModelCard_ModelModified(ModelDescription desc)
        {
            SystemManager.Instance().ModelManager.EditModel(desc);
        }

        private void productModelCard_ModelCopied(ModelDescription desc)
        {
            if (desc != null)
            {
                var description = desc as MPAlignment.Data.ModelDescription;
                if (Path.GetDirectoryName(description.ModelPath) != GetModelManager().GetModelPath(description))
                    return;
                var card = CreateModelCard(desc);
                flpBoardModelList.Controls.Add(card);
            }
        }

        private void productModelCard_ModelDeleted(ModelDescription desc)
        {
           if(desc != null)
            {
                SystemManager.Instance().ModelManager.DeleteModel(desc);
                var controls = flpBoardModelList.Controls.Find("ProductModelCard",false);
                foreach( var control in controls)
                {
                    ProductModelCard card = (ProductModelCard)control;
                    if (card.CurModelDescription == desc)
                    {
                        flpBoardModelList.Controls.Remove(control);
                    }
                }
            }
        }
        private void productModelCard_ModelSelected(ModelDescription desc) //double Click
        {
            GetModelManager().LoadModel(desc, null); //내부 이벤트 발생하여 메인폼과 inspectionPage 전달.
        }

        private void productModelCard_ModelCaptured(ModelDescription desc) //click //선택되어 프리뷰를 보여줌.
        {
            if (desc == null)
                return;

            string path = Path.Combine(GetModelManager().GetModelPath(desc));
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            string imagePath = Path.Combine(path, GetModelManager().PreviewImageName);
            Image image = Properties.Resources.machineImage2;

            if (File.Exists(imagePath) == false)
            {
                if (picModelImage.Image != null)
                {
                    picModelImage.Image.Dispose();
                    picModelImage.Image = null;
                }
                return;
            }

            picModelImage.SuspendLayout();
            image = ImageHelper.LoadImage(imagePath);

            if (picModelImage.Image != null)
            {
                picModelImage.Image.Dispose();
                picModelImage.Image = null;
            }

            picModelImage.Image = (Image)image.Clone();
            picModelImage.SuspendLayout();
            image.Dispose();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            var newModelForm = new ModelInfoForm();
            if (newModelForm.ShowDialog() == DialogResult.OK) //여그도 좀 그렇다...너무 엮여있음.
            {
                UpdateProductModelCards();
            }
        }

        private void btnCloseModel_Click(object sender, EventArgs e)
        {
            GetModelManager().CloseCurrentModel();
        }

    }
}
