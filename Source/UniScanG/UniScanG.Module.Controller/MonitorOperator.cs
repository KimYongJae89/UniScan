using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using UniEye.Base.MachineInterface;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Common.UI;
using UniScanG.Common.Util;
using UniScanG.Module.Controller.Exchange;
using UniScanG.Module.Controller.Settings.Monitor;
using UniScanG.Data;
using UniScanG.Gravure.Settings;
using System.Text;
using System.Threading;
using DynMvp.Devices;

namespace UniScanG.Module.Controller
{
    internal class MonitorOperator : ExchangeOperator, IServerExchangeOperator, IUserHandlerListener
    {
        List<IVisitListener> visitListenerList = new List<IVisitListener>();

        public override bool IsConnected => true;

        public InspectorObj[] Inspectors => this.server.InspectorList.ToArray();

        Server server;
        public Server Server
        {
            get { return server; }
        }

        public MonitorOperator()
        {
            this.server = new Server(MonitorSystemSettings.Instance().ServerSetting);
        }

        public override void Initialize()
        {
            this.server.Initialize();
            UserHandler.Instance().AddListener(this);
        }

        public override void Release()
        {
            this.server.Release();
        }

        public override bool ModelTrained(ModelDescription modelDescription)
        {
            modelDescription.IsTrained = server.ModelTrained(modelDescription);
            
            return modelDescription.IsTrained;
        }

        delegate void SyncModelDelegate(int camId);
        public void SyncModel(int camId)
        {
            System.Windows.Forms.Form mainForm = ConfigHelper.Instance().MainForm;
            if (mainForm.InvokeRequired)
            {
                mainForm.Invoke(new SyncModelDelegate(SyncModel), camId);
                return;
            }

            // 클라이언트가 없으면 Sync 동작 안 함.
            //int clientCount = GetInspectorList().FindAll(f => f.Info.CamIndex == camId && f.Info.ClientIndex > 0).Count;
            //if (clientCount == 0)
            //    return;

            UniScanG.UI.Etc.ProgressForm progressForm = new UniScanG.UI.Etc.ProgressForm();

            progressForm.TitleText = StringManager.GetString(this.GetType().FullName, "Sync") + string.Format(" (IM{0})", camId + 1);
            progressForm.MessageText = StringManager.GetString(this.GetType().FullName, "Model Sync");
            progressForm.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(SyncModel);
            progressForm.BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SyncModelComplete);
            progressForm.Argument = camId;
            progressForm.TopMost = false;

            progressForm.ShowDialog(mainForm);
            
            if (syncDone == false)
            {
                // Exception Throw??
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(StringManager.GetString("Model Sync Fail..."));
                sb.Append(syncDoneMessage);
                MessageForm.Show(null, sb.ToString());
                return;
            }

            GetInspectorList().FindAll(f => f.Info.CamIndex == camId).ForEach(f => f.Refesh());
            this.server.ReselectModel(camId);
        }

        bool syncDone = false;
        string syncDoneMessage = "";
        private void SyncModel(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int camIndex = (int)e.Argument;

            LogHelper.Debug(LoggerType.Operation, string.Format("MonitorOperator::SyncModel - Cam: {0}", camIndex));

            List<InspectorObj> targetInspectorObjList = GetInspectorList().FindAll(obj => obj.Info.CamIndex == camIndex);
            InspectorObj baseInspectorObj = targetInspectorObjList.Find(f => f.Info.ClientIndex <= 0);
            if (baseInspectorObj == null)
            {
                e.Result = new Tuple<int, string>(
                    camIndex
                    , string.Format(StringManager.GetString("Can not found master device of Cam {0}"), camIndex));
                return;
            }

            UniScanG.Data.Model.Model curModel = SystemManager.Instance().CurrentModel as UniScanG.Data.Model.Model;
            if (curModel == null)
            {
                e.Result = new Tuple<int, string>(
                   camIndex
                   , string.Format(StringManager.GetString("There is no Model selected.")));
                return;
            }

            worker?.ReportProgress(0, string.Format("0 / {0}", targetInspectorObjList.Count));
            string srcModelPath = baseInspectorObj.ModelManager.GetModelPath(curModel.ModelDescription);
            string srcCommonFile = Path.GetFullPath(Path.Combine(srcModelPath, "..", "..", "..", "..", "Config", "AlgorithmSetting.xml"));
            for (int i = 0; i < targetInspectorObjList.Count; i++)
            {
                InspectorObj targetInspectorObj = targetInspectorObjList[i];
                targetInspectorObj.ModelManager.Refresh();

                bool exist = false;
                for (int j = 0; j < 5; j++) // 가끔 모델이 존재하는데도 False가 올 때가 있음...
                {
                    exist = targetInspectorObj.ModelManager.IsModelExist(curModel.ModelDescription);
                    if (exist)
                        break;
                    Thread.Sleep(1000);
                }

                if (exist == false)
                {
                    e.Result = new Tuple<int, string>(
                    camIndex
                    , string.Format(StringManager.GetString("Model is NOT exist in {0}"), targetInspectorObj.Info.GetName()));
                    return;
                }

                //bool isTrained = baseInspectorObj.IsTrained(curModel.ModelDescription);
                //if (isTrained == true)
                {
                    try
                    {
                        string dstModelPath = targetInspectorObj.ModelManager.GetModelPath(curModel.ModelDescription);
                        string dstCommonFile = Path.GetFullPath(Path.Combine(dstModelPath, "..", "..", "..", "..", "Config", "AlgorithmSetting.xml"));
                        //if (srcModelPath != dstModelPath)
                        //    FileHelper.CopyDirectory(srcModelPath, dstModelPath, false, true);

                        if (srcCommonFile != dstCommonFile)
                            FileHelper.CopyFile(srcCommonFile, dstCommonFile, true);

                        FileInfo[] modelFileInfos = new DirectoryInfo(srcModelPath).GetFiles();
                        foreach (FileInfo modelFileInfo in modelFileInfos)
                        {
                            if (modelFileInfo.Name.StartsWith("~") || modelFileInfo.Extension.Equals(".bak"))
                                continue;

                            string srcModelFile = modelFileInfo.FullName;
                            string dstModelFile = Path.Combine(dstModelPath, modelFileInfo.Name);
                            if (srcCommonFile != dstCommonFile)
                                FileHelper.CopyFile(srcModelFile, dstModelFile, true);
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageForm.Show(null, string.Format(StringManager.GetString("Cannot Sync {0} - {1}"), targetInspectorObjList[i].Info.GetName(), ex.Message));
                        throw ex;
                    }
                }

                worker?.ReportProgress((i + 1) * 100 / targetInspectorObjList.Count, string.Format("{0} / {1}", i + 1, targetInspectorObjList.Count));

                e.Result = new Tuple<int, string>(camIndex, null);
            }
        }

        private void SyncModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Exception ex = e.Error;
            Tuple<int, string> tuple = (Tuple<int, string>)e.Result;
            if (string.IsNullOrEmpty(tuple.Item2) && ex == null)
            {
                syncDone = true;
                syncDoneMessage = "";
            }
            else
            {
                syncDone = false;
                syncDoneMessage = (ex != null) ? ex.Message : tuple.Item2;
            }

            LogHelper.Debug(LoggerType.Operation, string.Format("MonitorOperator::SyncModelComplete - Cam: {0}, {1}"
                , tuple.Item1, syncDoneMessage));
        }

        public override bool ModelExist(ModelDescription modelDescription)
        {
            if (server == null)
                return false;

            return server.ModelExist(modelDescription);
        }

        public override bool SelectModel(ModelDescription modelDescription)
        {
            try
            {
                bool ok = base.SelectModel(modelDescription);
                if (!ok)
                    return false;

                SelectRemoteModel(-1, -1, modelDescription);

                Model curModle = SystemManager.Instance().CurrentModel;
                if (curModle.LightParamSet.LightParamList.Count == 0)
                {
                    AdditionalSettings additionalSettings = AdditionalSettings.Instance() as AdditionalSettings;
                    for (int i = 0; i < additionalSettings.DefaultLightParamList.Count; i++)
                        curModle.LightParamSet.LightParamList.Add((LightParam)additionalSettings.DefaultLightParamList[i].Clone());
                }
                else if (curModle.LightParamSet.LightParamList.Count == 1 && string.IsNullOrEmpty(curModle.LightParamSet.LightParamList[0].Name))
                // 기존 자동조명 모델 호환
                {
                    curModle.LightParamSet.LightParamList[0].Name = "40.0";
                }
                return true;
            }
            catch(OperationCanceledException ex)
            {
                MessageForm.Show(ConfigHelper.Instance().MainForm, StringManager.GetString(ex.Message));
                return false;
            }
            catch (Exception ex)
            {
                AlarmException alarmException = new AlarmException(ErrorCodeModel.Instance.FailToReadParamFile, ErrorLevel.Error,
                    ex.Message, new object[] { modelDescription.Name }, "");
                ErrorManager.Instance().Report(alarmException);

                //System.Windows.Forms.Form mainForm = MonitorConfigHelper.Instance().MainForm;
                //MessageForm.Show(mainForm, ex.Message);
                return false;
            }
        }

        public void SelectRemoteModel(int camId, int cliendId, ModelDescription modelDescription)
        {
            server.SelectModel(camId, cliendId, modelDescription);
        }

        public override void CloseModel()
        {
            server.CloseModel();
            base.CloseModel();
        }

        public override void DeleteModel(ModelDescription modelDescription)
        {
            server.DeleteModel(modelDescription);

            base.DeleteModel(modelDescription);
        }

        public override bool NewModel(ModelDescription modelDescription)
        {
            server.NewModel(modelDescription);
            return base.NewModel(modelDescription);
        }

        public InspectorObj GetInspector(int camId, int clientId)
        {
            return server.InspectorList.Find(f => f.Info.CamIndex == camId && f.Info.ClientIndex == clientId);
        }


        public List<InspectorObj> GetInspectorList(int sheetNo = -1)
        {
            return server.InspectorList.FindAll(f => f.IsInspectable(sheetNo));
        }

        public void CloseVnc()
        {
            //server.SendVisit(ExchangeCommand.V_DONE);
        }

        public Process OpenVnc(Process process, string ipAddress, IntPtr handle)
        {
            //server.SendVisit(eVisit);

            return VncHelper.OpenVnc(process, ipAddress, handle, MonitorSystemSettings.Instance().VncPath);
        }

        public override void SendCommand(ExchangeCommand exchangeCommand, params string[] args)
        {
            server.SendCommand(exchangeCommand, args);
        }

        public bool ModelTrained(int camIndex, int clientIndex, ModelDescription modelDescription)
        {
            return server.ModelTrained(camIndex, clientIndex, modelDescription);
        }

        public void UserChanged()
        {
            User user = UserHandler.Instance().CurrentUser;
            server.SendCommand(ExchangeCommand.U_CHANGE, user.Id, user.UserType.ToString());
        }

        public override int GetCamIndex()
        {
            return -1;
        }

        public override int GetClientIndex()
        {
            return -1;
        }

        public override string GetRemoteIpAddress()
        {
            return null;
        }

        public override bool SaveModel()
        {
            Model model = SystemManager.Instance().CurrentModel;
            if (model == null)
                return false;

            Bitmap image = SheetCombiner.CreateModelImage(model.ModelDescription);
            if (image != null)
            {
                string imagePath = (SystemManager.Instance().ModelManager as ModelManager)?.GetPreviewImagePath(model.ModelDescription, "");
                ImageHelper.SaveImage(image, imagePath);
            }

            model.ModelDescription.IsTrained = server.ModelTrained(model.ModelDescription);
            model.Modified = true;
            bool ok = SystemManager.Instance().ModelManager.SaveModel(model);
            if (ok)
                ModelTeachDone(-1);
            return ok;
        }
    }
}
