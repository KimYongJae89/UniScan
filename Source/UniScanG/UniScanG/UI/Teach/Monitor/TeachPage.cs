using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Util;
using UniScanG.Data;

namespace UniScanG.UI.Teach.Monitor
{
    public partial class TeachPage : UserControl, IMainTabPage, IVncContainer, IModelListener, IUserHandlerListener, IMultiLanguageSupport
    {
        List<IVncControl> vncButtonList = new List<IVncControl>();

        IServerExchangeOperator server;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

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
                {
                    IVncControl vncControl = button as IVncControl;
                    vncControl.InitHandle(remoteTeachingPanel.Handle);
                    vncButtonList.Add(vncControl);
                    panelButtonL.Controls.Add(button);
                }
                else
                {
                    panelButtonR.Controls.Add(button);
                }

            }

            settingPanel.Controls.Add(monitorUiChanger.CreateTeachSettingPanel());
            settingPanel.AutoSize = true;
            settingPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            settingPanel.Visible = settingPanel.Controls.Count > 0;

            StringManager.AddListener(this);
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            UserHandler.Instance().AddListener(this);
        }

        ~TeachPage()
        {
            ExitAllVncProcess(true);
            //this.vncButtonList.ForEach(f => f.ExitProcess());
        }

        public void EnableControls(UserType user) { }

        public void ProcessStarted(IVncControl startVncButton)
        {
            SystemManager.Instance().DeviceController.OnEnterWaitInspection("Stage", "Light", "Encoder");
        }

        public void ProcessExited()
        {
            SystemManager.Instance().DeviceController.OnExitWaitInspection();
            //SystemManager.Instance().ExchangeOperator.SaveModel();
            this.ModelTeachDone(-1);
        }

        public void UpdateControl(string item, object value) { }

        public void PageVisibleChanged(bool visibleFlag)
        {
            if (!visibleFlag)
            {
                ExitAllVncProcess(false);
            }
        }

        public void ExitAllVncProcess(bool terminateProgram)
        {
            new DynMvp.UI.Touch.SimpleProgressForm().Show(() =>
            {
                vncButtonList.ForEach(f => f.ExitProcess());
                if (!terminateProgram)
                {
                    if (SystemManager.Instance().InspectRunner.IsIdleIntpect())
                        ProcessExited();

                    ((IServerExchangeOperator)SystemManager.Instance().ExchangeOperator).SendCommand(Common.Exchange.ExchangeCommand.V_INSPECT);
                }
            });
        }

        public void ModelChanged()
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            //Bitmap image = SheetCombiner.CreateModelImage(SystemManager.Instance().CurrentModel.ModelDescription);
            Bitmap image = (SystemManager.Instance().ModelManager as ModelManager)?.GetPreviewImage(SystemManager.Instance().CurrentModel.ModelDescription, "");
            UpdateImage(image);
        }

        public void ModelTeachDone(int camId)
        {
            if (camId < 0)
            {
                string imagePath = (SystemManager.Instance().ModelManager as ModelManager)?.GetPreviewImagePath(SystemManager.Instance().CurrentModel.ModelDescription, "");
                if (File.Exists(imagePath))
                {
                    Bitmap bitmap = (Bitmap)ImageHelper.LoadImage(imagePath);
                    UpdateImage(bitmap);
                }
            }
        }
        public void ModelRefreshed() { }

        delegate void UpdateImageDelegate(Bitmap image);
        private void UpdateImage(Bitmap image)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateImageDelegate(UpdateImage), image);
                return;
            }

            prevImage.Image = image;
            prevImage.Invalidate();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegate(UserChanged));
                return;
            }

            User curUser = UserHandler.Instance().CurrentUser;
            if (curUser != null)
            {
                List<Control> list = vncButtonList.FindAll(f => f.GetInspector().Info.ClientIndex > 0).ConvertAll<Control>(g => (Control)g);
                list.ForEach(f => f.Visible = UserHandler.Instance().CurrentUser.IsMasterAccount);
            }
        }
    }
}
