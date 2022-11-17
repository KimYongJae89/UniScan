using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Authentication;
using DynMvp.UI.Touch;
using System;
using UniScanM.Operation;

namespace UniScanM.EDMS.UI
{
    class InspectionPage : UniScanM.UI.InspectionPage
    {


        public override void OnManualButton_Click()
        {
            // base.OnManualButton_Click();
            //if (UniScanM.Authorize.AuthorizeHelper.Authorize(UserType.Admin | UserType.Maintrance) == false)
            //{
            //    MessageForm.Show(null, StringManager.GetString("Permission is invaild."));
            //    return;
            //}

           // if (MessageForm.Show(null, StringManager.GetString("'Force Start Mode' can occur abnormal inspection. Continue?"), MessageFormType.YesNo) == DialogResult.Yes)
            {
                UniScanM.EDMS.Data.Model currentModel = SystemManager.Instance().CurrentModel as UniScanM.EDMS.Data.Model;
                UniScanM.EDMS.Data.MobileParam mobieinfo = currentModel.Mobileparam;

                if (mobieinfo.lotno == "") mobieinfo.lotno = string.Format("Lot.{0}", DateTime.Now.ToString("yyMMddHHmm"));

                MobileInputForm inputForm = new MobileInputForm("Insert Production Information",
                    mobieinfo.lotno,
                    mobieinfo.lineSpeed,
                    mobieinfo.worker,
                    mobieinfo.model,
                    mobieinfo.maxDistance);


                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    var state = ((PLCInspectStarter)(SystemManager.Instance().InspectStarter)).MelsecMonitor.State;
                    state.ModelName =inputForm.ModelText;
                    state.LotNo = inputForm.LotNoText;
                    //state.Paste = "Paste";
                    state.SpSpeed = inputForm.SpeedText; state.PvSpeed = state.SpSpeed;
                    state.Worker = inputForm.WorkerText;
                    state.PvPosition = 0;
                    //state.RollDia = 149;
                    //todo Limit Max roll distance 
                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    mobieinfo.lotno = inputForm.LotNoText;
                    mobieinfo.lineSpeed = inputForm.SpeedText;
                    mobieinfo.worker = inputForm.WorkerText;
                    mobieinfo.model = inputForm.ModelText;
                    mobieinfo.maxDistance = inputForm.MaxDistance;
                    SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (ChangeStartMode(StartMode.Manual))
                    {
                        SystemManager.Instance().InspectStarter.PreStartInspect(false);
                        UiHelper.SuspendDrawing(this);
                        SystemManager.Instance().InspectStarter.OnStartInspection();
                        UiHelper.ResumeDrawing(this);
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            buttonAutoManSwitch.Enabled = false;
            buttonAutoManSwitch.Visible = false;
            ChangeStartMode(StartMode.Stop);
            UpdateStatusLabel();

        }

    
    }
}



 