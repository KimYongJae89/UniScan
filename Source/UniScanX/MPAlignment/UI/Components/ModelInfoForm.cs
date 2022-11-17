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
using UniScanX.MPAlignment.Data;


namespace UniScanX.MPAlignment.UI.Components
{
    public delegate bool delegateExistModel(string modelname);

    public partial class ModelInfoForm : Form
    {
        ModelDescription modelDesc = null; //인자로 넘겨받은 모델만, 에디팅할때만 사용!

        public delegateExistModel _ExsitModel=null;
        public ModelDescription ModelDesc
        {
            get { return modelDesc; }
            set { modelDesc = value; }
        }

        bool copyModelMode;

        ModelDescription newModelDesc;
        public ModelDescription NewModelDesc
        {
            get { return newModelDesc; }
            set { newModelDesc = value; }
        }

        public ModelInfoForm(ModelDescription modeldesc = null, bool copyModel = false)
        {
            InitializeComponent();

            if(modeldesc != null)
            {
                modelDesc = modeldesc;
                Updatedata(false);
                this.copyModelMode = copyModel;
                if(copyModel == false)
                {
                    txtName.Enabled = false;
                }

            }
        }
        private void Updatedata(bool update=true)
        {
            if(update)  //ui -> data
            {
                modelDesc.Name = txtName.Text;
                SizeF size = new SizeF();
                size.Width = float.Parse(tbWidth.Text);
                size.Height = float.Parse(tbHeight.Text);
                modelDesc.PatternSize = size;
                modelDesc.Description = txtDescription.Text;
            }
            else //data -> ui
            {
                txtName.Text = modelDesc.Name;
                tbWidth.Text = modelDesc.PatternSize.Width.ToString();
                tbHeight.Text = modelDesc.PatternSize.Height.ToString();
                txtDescription.Text = modelDesc.Description;
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            //{
            //    e.Handled = true;
            //}
            //숫자만 입력되도록 필터링  //숫자와 백스페이스를 제외한 나머지를 바로 처리
            if (!(
                char.IsDigit(e.KeyChar) || 
                e.KeyChar == Convert.ToChar(Keys.Back) || 
                e.KeyChar == '.'
                ))   
            {
                e.Handled = true;
            }

            //// only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (modelDesc != null) //모델 수정 및 복사
            {
                //기존 모델  어째? 좀 애매하네.
                if (copyModelMode)  //복사
                {
                    if (ExistModelName(txtName.Text))
                        return;
                    else
                        CopyModel();
                }
                else //수정
                {
                    Updatedata(true);
                    SystemManager.Instance().ModelManager.SaveModelDescription(modelDesc);
                }

            }
            else if (null == CreateNewModel()) //모델 생성에 문제가 있음. 똑같은 모델이 있다거나.
            {
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private bool ExistModelName(string name)
        {
            if (((ModelManager)(SystemManager.Instance().ModelManager)).IsModelExist(name))
            {
                var messageForm = new NoticeMessageForm("Invaild",
                    $"{name} already resistered. Please input another name.",
                    DialogResultType.OK);
                messageForm.ShowDialog();
                return true;
            }
            return false;
        }

        private ModelDescription CreateNewModel()
        {
            newModelDesc = (MPAlignment.Data.ModelDescription)SystemManager.Instance().ModelManager.CreateModelDescription();
            newModelDesc.Name = txtName.Text;
            newModelDesc.Description = txtDescription.Text;

            SizeF size = new SizeF();
            size.Width = float.Parse(tbWidth.Text);
            size.Height = float.Parse(tbHeight.Text);
            newModelDesc.PatternSize = size;

            if (ExistModelName(newModelDesc.Name))
            {       
                return null;
            }
            SystemManager.Instance().ModelManager.AddModel(newModelDesc);
            return newModelDesc;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CopyModel()
        {
            DynMvp.Data.ModelDescription data;
            CreateNewModel();
            SystemManager.Instance().ModelManager.CopyModelData(
               ((DynMvp.Data.ModelDescription)modelDesc),
                ((DynMvp.Data.ModelDescription)newModelDesc)
                );
        }

    }
}
