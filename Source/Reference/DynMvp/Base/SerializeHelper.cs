using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Base
{
    public static class SerializeHelper
    {
        private static void ThrowIfNotSerializable(Type type)
        {
            if (!type.IsSerializable)
                throw new ArgumentException(string.Format("{0} is not serialized Calss", type.Name));
        }

        public static string Serialize(object obj)
        {
            ThrowIfNotSerializable(obj.GetType());

            string str = "";
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(ms, obj);
                //ms.Position = 0;
                str = Convert.ToBase64String(ms.GetBuffer());
            }
            return str;
        }

        public static T Deserialize<T>(string value) where T : class, new()
        {
            ThrowIfNotSerializable(typeof(T));

            T obj = new T();
            byte[] bytes = Convert.FromBase64String(value);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    if(ms.Capacity>0)
                        obj = (T)binaryFormatter.Deserialize(ms);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, string.Format("SerializeHelper::Deserialize<{0}>  - {1}", typeof(T).Name, ex.Message));
                }
            }
            return obj;
        }
    }
}
