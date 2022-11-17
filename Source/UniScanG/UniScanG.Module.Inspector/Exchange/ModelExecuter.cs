using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanG.Common;
using UniScanG.Common.Exchange;
using UniScanG.UI;
using UniScanG.UI.Teach;

namespace UniScanG.Module.Inspector.Exchange
{
    public class ModelExecuter : UniScanG.Common.Exchange.MachineIfExecuter
    {
        ModellerPageExtender modellerPageExtender;

        public ModelExecuter() : base()
        {
            this.modellerPageExtender = SystemManager.Instance().ModellerPageExtender as ModellerPageExtender;
        }

        protected override void AddExchangeCommand()
        {
            this.exchangeCommandList.AddRange(new ExchangeCommand[]
            {
                ExchangeCommand.M_CREATE,
                ExchangeCommand.M_SELECT,
                ExchangeCommand.M_RESELECT,
                ExchangeCommand.M_REFRESH,
                ExchangeCommand.M_CLOSE,
                ExchangeCommand.M_DELETE,
                ExchangeCommand.U_CHANGE,
                ExchangeCommand.C_SYNC,
                ExchangeCommand.C_SPD,
                ExchangeCommand.C_LICENSE,
            });
        }

        protected override bool Execute(MachineIfProtocolWithArguments command)
        {
            string[] splitCommand = command.ToString().Split(',');

            ExchangeCommand modelCommand = IsExcutable(splitCommand[0]);
            bool result = modelCommand != ExchangeCommand.None;
            if (!result)
                return false;
            try
            {
                switch (modelCommand)
                {
                    case ExchangeCommand.M_CREATE:
                    case ExchangeCommand.M_SELECT:
                    case ExchangeCommand.M_RESELECT:
                    case ExchangeCommand.M_DELETE:
                        int camId = int.Parse(splitCommand[1]);
                        int clientId = int.Parse(splitCommand[2]);
                        if ((camId < 0 || camId == SystemManager.Instance().ExchangeOperator.GetCamIndex()) &&
                            (clientId < 0 || clientId == SystemManager.Instance().ExchangeOperator.GetClientIndex()))
                        {
                            List<string> argList = new List<string>();
                            argList.Add(modelCommand.ToString());
                            argList.Add(SystemManager.Instance().ExchangeOperator.GetCamIndex().ToString());
                            argList.Add(SystemManager.Instance().ExchangeOperator.GetClientIndex().ToString());

                            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.J_RUNNING, argList.ToArray());

                            // 글로벌 파라메터 로드
                            Gravure.Vision.AlgorithmSetting.Instance().Load();

                            string[] modelDiscArgs = splitCommand.Skip(3).ToArray();
                            bool visit = (modelCommand == ExchangeCommand.M_SELECT);
                            if(modelCommand == ExchangeCommand.M_RESELECT)
                                modelDiscArgs = SystemManager.Instance().CurrentModel.ModelDescription.GetArgs();

                            bool done = false;
                            switch(modelCommand)
                            {
                                case ExchangeCommand.M_CREATE:
                                done = (CreateModel(modelDiscArgs) != null);
                                    break;

                                case ExchangeCommand.M_SELECT:
                                case ExchangeCommand.M_RESELECT:
                                    done = LoadModel(modelDiscArgs);

                                    if (done && visit)
                                        ((InspectorOperator)SystemManager.Instance().ExchangeOperator).PreparePanel(ExchangeCommand.V_INSPECT);
                                    break;

                                case ExchangeCommand.M_DELETE:
                                    done = DeleteModel(modelDiscArgs);
                                    break;
                            }
                            
                            argList.AddRange(modelDiscArgs);
                            SystemManager.Instance().ExchangeOperator.SendCommand(done ? ExchangeCommand.J_DONE : ExchangeCommand.J_ERROR, argList.ToArray());
                        }
                        break;

                    case ExchangeCommand.M_REFRESH:
                        //SystemManager.Instance().ModelManager.Refresh();
                        SystemManager.Instance().ExchangeOperator.UpdateModelList();
                        //SystemManager.Instance().UiController.ChangeTab("Model");
                        break;

                    case ExchangeCommand.M_CLOSE:
                        SystemManager.Instance().ModelManager.CloseModel();
                        SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.J_DONE,
                            ExchangeCommand.M_CLOSE.ToString(), 
                            SystemManager.Instance().ExchangeOperator.GetCamIndex().ToString(),
                            SystemManager.Instance().ExchangeOperator.GetClientIndex().ToString());
                        break;

                    case ExchangeCommand.U_CHANGE:
                        UserHandler.Instance().CurrentUser = new User(splitCommand[1], "", (UserType)Enum.Parse(typeof(UserType), splitCommand[2]));
                        break;

                    case ExchangeCommand.C_SYNC:
                        AdditionalSettings.Instance().Load();
                        SystemManager.Instance().ExchangeOperator.ModelTeachDone(-1);
                        break;

                    case ExchangeCommand.C_SPD:
                        modellerPageExtender.LineStartState = bool.Parse(splitCommand[1]);
                        modellerPageExtender.LineSpeedMpm = float.Parse(splitCommand[2]);
                        break;

                    case ExchangeCommand.C_LICENSE:
                        {
                            string[] licenses = splitCommand.Skip(1).ToArray();
                            if (!LicenseManager.Licenses.SequenceEqual(licenses))
                            {
                                lock (LicenseManager.Licenses)
                                {
                                    string oldLicense = LicenseManager.GetString();
                                    LicenseManager.Clear();
                                    Array.ForEach(licenses, f => LicenseManager.Set(f, true));
                                    LicenseManager.Save();
                                    string newLicense = LicenseManager.GetString();
                                    LogHelper.Debug(LoggerType.Operation, $"License Changed - [{oldLicense}] -> [{newLicense}]");

                                    // 강제종료 -> 런처가 살려줄꺼임..
                                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                                }
                            }
                        }
                        break;

                    default:
                        result = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Operation, string.Format("ModelExecuter::Execute({0}), {1}", modelCommand.ToString(), ex.Message));
                LogHelper.Error(LoggerType.Operation, ex.StackTrace.ToString());
            }
            return result;
        }

        private UniScanG.Data.Model.ModelDescription CreateModel(string[] modelDiscArgs)
        {
            try
            {
                SystemManager.Instance().ModelManager.Refresh();

                UniScanG.Data.Model.ModelDescription modelDescription = SystemManager.Instance().ModelManager.GetModelDescription(modelDiscArgs) as UniScanG.Data.Model.ModelDescription;
                if (modelDescription != null)
                    return modelDescription;

                modelDescription = SystemManager.Instance().ModelManager.CreateModelDescription() as UniScanG.Data.Model.ModelDescription;
                modelDescription.FromArgs(modelDiscArgs);
                if (SystemManager.Instance().ExchangeOperator.NewModel(modelDescription))
                    return modelDescription;

                return null;

            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, $"ModelExecuter::CreateModel - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return null;
            }
        }

        private bool LoadModel(string[] modelDiscArgs)
        {
            try
            {
                SystemManager.Instance().ModelManager.Refresh();

                UniScanG.Data.Model.ModelDescription modelDescription = SystemManager.Instance().ModelManager.GetModelDescription(modelDiscArgs) as UniScanG.Data.Model.ModelDescription;
                if (modelDescription == null)
                {
                    // 모델이 없으면 자동으로 새로 만듬....
                    modelDescription = CreateModel(modelDiscArgs);
                    //modelDescription = SystemManager.Instance().ModelManager.CreateModelDescription() as UniScanG.Data.Model.ModelDescription;
                    //modelDescription.FromArgs(modelDiscArgs);
                    //SystemManager.Instance().ModelManager.SaveModelDescription(modelDescription);
                    //SystemManager.Instance().ModelManager.Refresh();
                }

                return SystemManager.Instance().ExchangeOperator.SelectModel(modelDescription);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, $"ModelExecuter::LoadModel - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return false;
            }
        }

        private bool DeleteModel(string[] modelDiscArgs)
        {
            try
            {
                SystemManager.Instance().ModelManager.Refresh();

                UniScanG.Data.Model.ModelDescription modelDescription = SystemManager.Instance().ModelManager.GetModelDescription(modelDiscArgs) as UniScanG.Data.Model.ModelDescription;
                if (modelDescription == null)
                    return true;

                SystemManager.Instance().ExchangeOperator.DeleteModel(modelDescription);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, $"ModelExecuter::CreateModel - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return false;
            }
        }

    }
}
