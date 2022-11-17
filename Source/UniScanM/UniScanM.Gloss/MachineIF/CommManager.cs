using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.RabbitMQ;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.MachineIF
{
    public abstract class CommManager
    {
        private static CommManager _instance;

        public UniEye.Base.MachineInterface.MachineIf MachineIf { get; set; }
        public UniScanSCommandParser CommandParser { get; set; }

        public static CommManager Instance()
        {
            if (_instance != null)
                return _instance;

            return null;
        }

        public static void SetInstance(GlossCommManager instance)
        {
            _instance = instance;
        }

        protected virtual void DataReceived(ReceivedPacket receivedPacket) { }

        public virtual void Connect()
        {
            MachineIf?.Initialize();
        }

        public virtual void Disconnect()
        {
            MachineIf?.Release();
        }

        public bool IsConnected()
        {
            if (MachineIf == null)
                return false;
            return MachineIf.IsConnected;
        }

        public bool SendMessage(EUniScanSCommand command)
        {
            return SendMessage(command, null);
        }

        public bool SendMessage(EUniScanSCommand command, params string[] args)
        {
            if (MachineIf == null)
                return false;

            var machineIfProtocol = new TcpIpMachineIfProtocol(command, true, 500);
            return MachineIf.SendCommand(machineIfProtocol, args).IsResponced;
        }
    }

    public delegate bool EnterWaitDelegate(string lotName);
    public delegate bool ExitWaitDelegate();

    public class GlossCommManager : CommManager
    {
        public EnterWaitDelegate EnterWaitDelegate { get; set; }
        public ExitWaitDelegate ExitWaitDelegate { get; set; }

        private object inspectLockObj = new object();

        public GlossCommManager()
        {
            var tcpIpInfo = new TcpIpInfo(GlossSettings.Instance().CMMQTTIpAddress, 4000);
            var IfSetting = new TcpIpMachineIfSetting(MachineIfType.TcpClient) { TcpIpInfo = tcpIpInfo };
            var rabbitMQMachineIf = new RabbitMQMachineIf(IfSetting, "GM", "GM");
            rabbitMQMachineIf.DataReceived = DataReceived;
            rabbitMQMachineIf.IsAsync = true;

            MachineIf = rabbitMQMachineIf;
            CommandParser = new UniScanSCommandParser();

            TopicInitialize();
        }

        public void TopicInitialize()
        {
            var rabbitMQMachineIf = MachineIf as RabbitMQMachineIf;
            var config = GlossSettings.Instance();

            rabbitMQMachineIf.ResetSendTopic();
            rabbitMQMachineIf.AddSendTopic(config.GMTopicName);

            rabbitMQMachineIf.ResetReceiveTopic();
            rabbitMQMachineIf.AddReceiveTopic(config.CMTopicName);
        }

        public override void Connect()
        {
            base.Connect();
            while (MachineIf?.IsConnected == false)
            {
                if (MessageBox.Show("RabbitMQ 서버가 열려있지 않습니다. 서버를 확인 해주세요", "에러", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                    base.Connect();
                else
                    break;
            }
        }

        protected override void DataReceived(ReceivedPacket receivedPacket)
        {
            CommandInfo commandInfo = CommandParser?.Parse(receivedPacket);
            if (commandInfo == null)
                return;

            switch (commandInfo.Command)
            {
                case EUniScanSCommand.EnterWait: EnterWait(commandInfo); break;
                case EUniScanSCommand.ExitWait: ExitWait(commandInfo); break;

                default: SendMessage(commandInfo.Command); break;
            }
        }

        // 광택도는 단일 모델이므로 스킵
        //private void OpenModel(CommandInfo commandInfo)
        //{
        //    if (Monitor.TryEnter(inspectLockObj))
        //    {
        //        try
        //        {
        //            string modelName = commandInfo.Parameters[0];
        //            string modelPath = $@"\\{GlossSettings.Instance().CMMQTTIpAddress}\{commandInfo.Parameters[1]}";

        //            ModelManager ModelManager = ModelManager.Instance() as ModelManager;
        //            ModelManager.ModelPath = modelPath;
        //            ModelManager.Refresh();
        //            ModelManager.OpenModel(modelName, null);

        //            SendMessage(EUniScanCCommand.OpenModel, "");
        //            Debug.WriteLine(string.Format("CommManager - OpenModel"));
        //        }
        //        catch (Exception e)
        //        {
        //            SendMessage(EUniScanCCommand.OpenModel, e.Message);
        //            LogHelper.Debug(LoggerType.Error, "CommManager - OpenModel - Fail");
        //            LogHelper.Debug(LoggerType.Error, e.StackTrace);
        //        }
        //        finally
        //        {
        //            Monitor.Exit(inspectLockObj);
        //        }
        //    }
        //}

        private void EnterWait(CommandInfo commandInfo)
        {
            if (Monitor.TryEnter(inspectLockObj))
            {
                try
                {
                    var dbName = commandInfo.Parameters[0];
                    var resultImagePath = $@"\\{GlossSettings.Instance().CMDBIpAddress}\{commandInfo.Parameters[1]}"; //resultPath에 lotName 까지만 이어진 폴더 경로
                    var lotName = commandInfo.Parameters[2];
                    var lineSpeed = commandInfo.Parameters[3];

                    EnterWaitDelegate?.Invoke(lotName);

                    SendMessage(EUniScanSCommand.EnterWait);
                    Debug.WriteLine(string.Format("CommManager - EnterWait"));
                }
                catch (Exception e)
                {
                    SendMessage(EUniScanSCommand.EnterWait, e.Message);
                    LogHelper.Debug(LoggerType.Error, "CommManager - EnterWait - Fail");
                    LogHelper.Debug(LoggerType.Error, e.StackTrace);
                }
                finally
                {
                    Monitor.Exit(inspectLockObj);
                }
            }
        }

        private void ExitWait(CommandInfo commandInfo)
        {
            if (Monitor.TryEnter(inspectLockObj))
            {
                try
                {
                    ExitWaitDelegate?.Invoke();
                    Debug.WriteLine(string.Format("CommManager - ExitWait"));
                }
                catch (Exception e)
                {
                    LogHelper.Debug(LoggerType.Error, "CommManager - ExitWait - Fail");
                    LogHelper.Debug(LoggerType.Error, e.StackTrace);
                }
                finally
                {
                    Monitor.Exit(inspectLockObj);
                    SendMessage(EUniScanSCommand.ExitWait);
                }
            }
        }
    }
}
