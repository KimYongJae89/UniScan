using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UniEye.Base.Settings;

namespace UniEye.Base.Data
{
    public enum DataPathType
    {
        Model_Day, Model_Day_Hour, Model_Day_Time, Day_Model, Day_Hour_Model
    }

    public class PathManager
    {
        static DataPathType dataPathType = DataPathType.Model_Day;
        public static DataPathType DataPathType
        {
            get { return dataPathType; }
            set { dataPathType = value; }
        }

        public static string GetResultPath(string modelName, DateTime dateTime, string serialNo, string sequenceNo = "")
        {
            return GetResultPath(PathSettings.Instance().Result, modelName, dateTime, serialNo, sequenceNo);
        }

        public static string GetRemoteResultPath(string modelName, DateTime dateTime, string serialNo, string sequenceNo = "")
        {
            return GetResultPath(PathSettings.Instance().RemoteResult, modelName, dateTime, serialNo, sequenceNo);
        }

        public static string GetResultPath(string resultPath, string modelName, DateTime dateTime, string serialNo, string sequenceNo = "")
        {
            string path;
            DateTime curTime = dateTime;// DateTime.Now;

            switch (dataPathType)
            {
                default:
                case DataPathType.Model_Day:
                    path = String.Format("{0}\\{1}\\{2}", resultPath, modelName, curTime.ToString("yyyyMMdd"));
                    break;
                case DataPathType.Model_Day_Hour:
                    path = String.Format("{0}\\{1}\\{2}\\{3}", resultPath, modelName, curTime.ToString("yyyyMMdd"), curTime.ToString("HH"));
                    break;
                case DataPathType.Model_Day_Time:
                    path = String.Format("{0}\\{1}\\{2}\\{3}", resultPath, modelName, curTime.ToString("yyyyMMdd"), curTime.ToString("HHmmss"));
                    break;
                case DataPathType.Day_Model:
                    path = String.Format("{0}\\{1}\\{2}", resultPath, curTime.ToString("yyyyMMdd"), modelName);
                    break;
                case DataPathType.Day_Hour_Model:
                    path = String.Format("{0}\\{1}\\{2}\\{3}", resultPath, curTime.ToString("yyyyMMdd"), curTime.ToString("HH"), modelName);
                    break;
            }

            path += "\\" + serialNo;

            if (string.IsNullOrEmpty(sequenceNo) == false)
                path += "\\" + sequenceNo;

            //Directory.CreateDirectory(path);

            return path;
        }

        public static string GetCustumPath(params string[] paths)
        {
            string path = paths[0];

            for (int i = 1; i < paths.Count(); i++)
                path += "\\" + paths[i];

            return path;
        }

        public static string GetSummaryPath(string resultPath, string modelName, DateTime dateTime)
        {
            string path;
            DateTime curTime = DateTime.Now;

            switch (dataPathType)
            {
                default:
                case DataPathType.Model_Day:
                    path = String.Format("{0}\\{1}\\{2}", resultPath, modelName, curTime.ToString("yyyyMMdd"));
                    break;
                case DataPathType.Model_Day_Hour:
                    path = String.Format("{0}\\{1}\\{2}\\{3}", resultPath, modelName, curTime.ToString("yyyyMMdd"), curTime.ToString("HH"));
                    break;
                case DataPathType.Day_Model:
                    path = String.Format("{0}\\{1}\\{2}", resultPath, curTime.ToString("yyyyMMdd"), modelName);
                    break;
                case DataPathType.Day_Hour_Model:
                    path = String.Format("{0}\\{1}\\{2}\\{3}", resultPath, curTime.ToString("yyyyMMdd"), curTime.ToString("HH"), modelName);
                    break;
            }
            return path;
        }
    }
}
