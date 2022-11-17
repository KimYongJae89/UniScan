using DynMvp.Base;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;
using UniScanG.Data;

namespace UniScanG.UI.TeachPage.Monitor
{
    public partial class TeachPage : UserControl, IMainTabPage, IVncContainer, IModelListener, IMultiLanguageSupport
    {
        List<IVncControl> vncButtonList = new List<IVncControl>();

        IServerExchangeOperator server;
        
        public TeachPage()
        {
            InitializeComponent();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            MonitorUiChanger monitorUiChanger = (MonitorUiChanger)SystemManager.Instance().UiChanger;

            List<Control> buttonList = monitorUiChanger.GetTeachButtons(this);
            foreach (Control button in buttonList)
            {
                if (button is IVncControl)
                    vncButtonList.Add((IVncControl)button);
            }

            foreach (IVncControl vncButton in vncButtonList)
            {
                vncButton.InitHandle(remoteTeachingPanel.Handle);
                buttonPanel.Controls.Add((Control)vncButton);
            }

            settingPanel.Controls.Add(monitorUiChanger.CreateTeachSettingPanel());
            StringManager.AddListener(this);
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == true)
            {
            }
            else
            {
                ProcessExited();
            }
        }

        public void EnableControls() { }

        public void ProcessStarted(IVncControl startVncButton)
        {
            foreach (IVncControl vncButton in vncButtonList)
            {
                if (vncButton != startVncButton)
                    vncButton.Disable();
            }
        }

        public void ProcessExited()
        {
            foreach (IVncControl vncButton in vncButtonList)
            {
                vncButton.ExitProcess();
                vncButton.Enable();
            }
        }

        public void UpdateControl(string item, object value) { }
        public void PageVisibleChanged(bool visibleFlag) { }
        public void ModelChanged()
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;
;
            
            Bitmap image = SheetCombiner.CreateModelImage(SystemManager.Instance().CurrentModel.ModelDescription);
            if (image != null)
                UpdateImage(image);
        }

        public void ModelTeachDone()
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            Bitmap image = SheetCombiner.CreateModelImage(SystemManager.Instance().CurrentModel.ModelDescription);
            if (image != null)
            {
                string imagePath = SystemManager.Instance().ModelManager.GetPreviewImagePath(SystemManager.Instance().CurrentModel.ModelDescription);
                ImageHelper.SaveImage(image, imagePath);
                
                UpdateImage(image);
            }
        }

        delegate void UpdateImageDelegate(Bitmap image);
        private void UpdateImage(Bitmap image)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateImageDelegate(UpdateImage), image);
                return;
            }

            prevImage.Image = image;
            prevImage.Invalidate();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
