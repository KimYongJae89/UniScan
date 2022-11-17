using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniView.Client.Core
{
    public partial class RabbitMqClientBase : IDisposable
    {
        #region 생성자
        public RabbitMqClientBase(RabbitMqAccessInfo rabbitMqAccessInfo)
        {
            if (rabbitMqAccessInfo == null)
                throw new Exception("Setting class is null.");
            if (rabbitMqAccessInfo.RabbitMqRoutingKeys == null)
                throw new Exception("Not setted RabbitMqRoutingKeys");
            if (string.IsNullOrEmpty(rabbitMqAccessInfo.RabbitMqRoutingKeys.Exchange))
                throw new Exception("Exchange is null");

            RabbitMqAccessInfo = rabbitMqAccessInfo;
        }
        #endregion


        #region 속성
        public RabbitMqAccessInfo RabbitMqAccessInfo { get; }

        private ConnectionFactory ConnectionFactory { get; set; }

        private IConnection Connection { get; set; }

        private IModel Channel { get; set; }

        private EventingBasicConsumer Consumer { get; set; }

        private List<string> ExchangeList { get; } = new List<string>();

        private List<string> RecvTopicList { get; } = new List<string>();

        private List<string> SendTopicList { get; } = new List<string>();

        public bool IsConnected { get => Connection == null ? false : Connection.IsOpen; }
        #endregion


        #region 대리자
        public delegate void DataRecievedBytesEventHandler(byte[] bytes, string routingKey = null);

        public delegate void ChannelBasicAckEventHandler(EventArgs e);

        public delegate void ErrorOccuredEventHandler(string message);
        #endregion


        #region 이벤트
        public event DataRecievedBytesEventHandler DataRecievedToBytes;

        public event ChannelBasicAckEventHandler ChannelBasicAck;

        public event ErrorOccuredEventHandler ErrorOccured;
        #endregion


        #region 메서드
        public bool Connect(int timeoutSeconds = 2)
        {
            if (timeoutSeconds < 0)
                throw new Exception("Parameter invaild(timeoutSeconds)");
            var userAccessInfo = RabbitMqAccessInfo.UserAccessInfo;
            ConnectionFactory = new ConnectionFactory()
            {
                HostName = userAccessInfo.HostInfo.IpAddress,
                UserName = userAccessInfo.UserAccount.Id,
                Password = userAccessInfo.UserAccount.PasswordHash,
                RequestedHeartbeat = 2/*TimeSpan.FromSeconds(2)*/,
                RequestedConnectionTimeout = timeoutSeconds / 2/*TimeSpan.FromSeconds(timeoutSeconds / 2)*/,
                //ClientProvidedName = RabbitMqAccessInfo.DeviceId,
                Port = 5672
            };

            if (Connection != null)
            {
                if (Connection.IsOpen)
                {
                    Disconnect();
                }
            }

            try
            {
                // ClientProvidedName을 커넥션을 만들어줄 때 넣어주도록 메서드가 바뀜
                Connection = ConnectionFactory.CreateConnection(RabbitMqAccessInfo.DeviceId);
                Connection.ConnectionBlocked += Connection_ConnectionBlocked;
                Connection.ConnectionUnblocked += Connection_ConnectionUnblocked;
                Connection.ConnectionShutdown += Connection_ConnectionShutdown;
                Channel = Connection.CreateModel();
                Consumer = new EventingBasicConsumer(Channel);
                Consumer.Received += Consumer_Received;
                Consumer.Registered += Consumer_Registered;
                Consumer.Shutdown += Consumer_Shutdown;
                Consumer.Unregistered += Consumer_Unregistered;
                Consumer.ConsumerCancelled += Consumer_ConsumerCancelled;

                foreach (var exchange in ExchangeList)
                {
                    Channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic, durable: true, autoDelete: false);
                    var queueName = Channel.QueueDeclare().QueueName;
                    Channel.BasicAcks += Channel_BasicAcks;
                    foreach (var recvTopic in RecvTopicList)
                    {
                        if (string.IsNullOrEmpty(recvTopic))
                            continue;

                        Channel.QueueBind(queue: queueName, exchange: exchange, routingKey: recvTopic);
                    }

                    Channel.BasicConsume(queue: queueName, autoAck: true, consumer: Consumer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ErrorOccured?.Invoke(ex.Message);
            }
            return IsConnected;
        }

        private void Consumer_Shutdown(object sender, ShutdownEventArgs e)
        {

        }

        private void Channel_BasicAcks(object sender, BasicAckEventArgs e)
        {
            Task.Run(() => ChannelBasicAck?.Invoke(e));
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                Connection.Close();
                Connection = null;
            }
        }

        private void Reconnect()
        {
            Disconnect();
            Connect();
        }

        public void AddReceiveTopic(string topic)
        {
            if (RecvTopicList.Exists(x => x == topic))
                return;

            RecvTopicList.Add(topic);
        }

        public void ResetReceiveTopic()
        {


            RecvTopicList.Clear();
        }

        public void AddSendTopic(string topic)
        {
            if (SendTopicList.Exists(x => x == topic))
                return;

            SendTopicList.Add(topic);
        }

        public void ResetSendTopic()
        {
            SendTopicList.Clear();
        }

        public void AddExchange(string exchangeName)
        {
            if (ExchangeList.Exists(x => x == exchangeName))
                return;

            ExchangeList.Add(exchangeName);
        }

        public void ResetExchange()
        {
            ExchangeList.Clear();
        }

        public bool SendCommand(byte[] bytes)
        {
            if (IsConnected == false)
            {
                Connect();
            }
            try
            {
                foreach (var exchange in ExchangeList)
                {
                    foreach (var topic in SendTopicList)
                    {
                        Channel.BasicPublish(exchange: exchange, routingKey: topic, basicProperties: null, body: bytes);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            DataRecievedToBytes?.Invoke(e.Body.ToArray(), e.RoutingKey);
        }

        private void Connection_ConnectionRecoveryError(object sender, ConnectionRecoveryErrorEventArgs e)
        {
        }

        private void Connection_RecoverySucceeded(object sender, EventArgs e)
        {
        }

        private void Connection_ConnectionUnblocked(object sender, EventArgs e)
        {
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            ErrorOccured?.Invoke(e.Reason);
        }


        private void Consumer_Registered(object sender, ConsumerEventArgs e)
        {
        }

        private void Consumer_Unregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void Consumer_ConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }
        #endregion
    }

    public partial class RabbitMqClientBase : IDisposable
    {
        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Disconnect();
                }
                disposedValue = true;
            }
        }

        // ~RabbitMqClientBase() {
        //   Dispose(false);
        // }

        public void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
