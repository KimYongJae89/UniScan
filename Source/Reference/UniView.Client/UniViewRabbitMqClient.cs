using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniView.Client.Core;
using UniView.Client.Protocol;

namespace UniView.Client
{
    public class UniViewRabbitMqClient
    {
        #region 생성자
        public UniViewRabbitMqClient(RabbitMqAccessInfo rabbitMqAccessInfo)
        {
            RabbitMqAccessInfo = rabbitMqAccessInfo;
            RabbitMqClientBase = new RabbitMqClientBase(rabbitMqAccessInfo);
            RabbitMqClientBase.AddExchange(rabbitMqAccessInfo.RabbitMqRoutingKeys.Exchange);
            RabbitMqClientBase.AddSendTopic(rabbitMqAccessInfo.RabbitMqRoutingKeys.SendTopicKey);
            RabbitMqClientBase.ChannelBasicAck += MRabbitMqCommunicator_ChannelBasicAck;
            RabbitMqClientBase.DataRecievedToBytes += DataRecieved;
        }
        #endregion


        #region 속성
        private RabbitMqClientBase RabbitMqClientBase { get; }

        private RabbitMqAccessInfo RabbitMqAccessInfo { get; }

        public bool IsConnected { get => RabbitMqClientBase == null ? false : RabbitMqClientBase.IsConnected; }
        #endregion


        #region 이벤트
        public EventHandler UnknownProtocolRecieved;

        public EventHandler DoCommandEvent;
        #endregion


        #region 메서드
        private void DataRecieved(byte[] bytes, string routingKey)
        {
            string recvString = Encoding.UTF8.GetString(bytes);
            UniViewProtocolBase protocolbase = null;

            if (IsUnknownProtocol(recvString, ref protocolbase))
            {
                return;
            }

            if (protocolbase.PrtocolType != UniViewProtocolType.Command)
            {
                return;
            }

            try
            {
                var visionCommand = JsonConvert.DeserializeObject<UniViewVisionCommandProtocol>(recvString);
                if (visionCommand.VisionCommand == VisionCommandType.Unknown)
                    return;

                Task.Run(() => DoCommandEvent?.Invoke(visionCommand, EventArgs.Empty));
            }
            catch
            {
                Task.Run(() => UnknownProtocolRecieved?.Invoke(recvString, EventArgs.Empty));
            }
        }

        private bool IsUnknownProtocol(string recvString, ref UniViewProtocolBase protocolbase)
        {
            try
            {
                protocolbase = JsonConvert.DeserializeObject<UniViewProtocolBase>(recvString);
            }
            catch
            {
                protocolbase = null;
                Task.Run(() => UnknownProtocolRecieved?.Invoke(this, EventArgs.Empty));
                return true;
            }
            return false;
        }

        private void MRabbitMqCommunicator_ChannelBasicAck(EventArgs e)
        {

        }

        public void Connect(int timeoutSeconds = 2, bool autoReconnect = false)
        {
            RabbitMqClientBase.Connect(timeoutSeconds);
        }

        public bool Reconnect()
        {
            if (RabbitMqClientBase == null)
            {
                throw new InvalidOperationException("UniviewRabbitMqClient class instance not initialized.");
            }

            Disconnect();
            RabbitMqClientBase.Connect();

            return RabbitMqClientBase.IsConnected;
        }

        public bool SendState(UniViewStateProtocol machineStateProtocol)
        {
            try
            {
                machineStateProtocol.SetDeviceId(RabbitMqAccessInfo.DeviceId);

                var serializedData = JsonConvert.SerializeObject(machineStateProtocol);
                var sendData = Encoding.UTF8.GetBytes(serializedData);
                RabbitMqClientBase.SendCommand(sendData);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SendStateAsync(UniViewStateProtocol machineState)
        {
            bool operationResult = false;
            DateTime startTime = DateTime.Now;
            await Task.Run(() =>
            {
                operationResult = SendState(machineState);
            });
            return operationResult;
        }

        public bool SendInspectResult(UniViewResultProtocol univiewResultProtocol)
        {
            try
            {
                univiewResultProtocol.SetDeviceId(RabbitMqAccessInfo.DeviceId);

                var serializedData = JsonConvert.SerializeObject(univiewResultProtocol);
                var sendData = Encoding.UTF8.GetBytes(serializedData);
                RabbitMqClientBase.SendCommand(sendData);
            }
            catch
            {
                return false;
            }
            return true;

        }

        public async Task<bool> SendInspectResultAsync(UniViewResultProtocol univiewResultProtocol)
        {
            DateTime startTime = DateTime.Now;
            bool operationResult = false;
            await Task.Run(() =>
            {
                operationResult = SendInspectResult(univiewResultProtocol);
            });
            return operationResult;
        }

        public void Disconnect()
        {
            RabbitMqClientBase.Dispose();
        }
        #endregion
    }
}
