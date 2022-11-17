using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniView.Client.Core
{
    public class RabbitMqAccessInfo
    {
        #region 속성
        /// <summary>
        /// 장비 Id
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// RabbitMq 서버 접속에 필요한 정보
        /// </summary>
        public UserAccessInfo UserAccessInfo { get; set; } = new UserAccessInfo();

        /// <summary>
        /// 라우팅 키
        /// </summary>
        public RabbitMqRoutingKey RabbitMqRoutingKeys { get; set; } = new RabbitMqRoutingKey();
        #endregion


        #region 생성자
        public RabbitMqAccessInfo()
        {
        }

        public RabbitMqAccessInfo(string deviceId, UserAccessInfo userAccessInfo, string exchange, string sendkey, string recvKey = "")
        {
            DeviceId = deviceId;
            UserAccessInfo = userAccessInfo;
            RabbitMqRoutingKeys.Exchange = exchange;
            RabbitMqRoutingKeys.SendTopicKey = sendkey;
        }
        #endregion


        #region 메서드
        public virtual void Load(string fileFullPath)
        {
            if (File.Exists(fileFullPath) == false)
            {
                Save(fileFullPath);
            }
            using (StreamReader file = File.OpenText(fileFullPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                var config = (RabbitMqAccessInfo)serializer.Deserialize(file, typeof(RabbitMqAccessInfo));
                DeviceId = config.DeviceId;
                UserAccessInfo = config.UserAccessInfo;
                RabbitMqRoutingKeys = config.RabbitMqRoutingKeys;
            }
        }

        public virtual void Save(string fileFullPath)
        {
            using (StreamWriter file = File.CreateText(fileFullPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, this);
            }
        }
        #endregion
    }
}
