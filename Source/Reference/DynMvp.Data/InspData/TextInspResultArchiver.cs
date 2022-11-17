using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.Data;
using System.Drawing;
using System.Threading;

namespace DynMvp.InspData
{
    public class TextInspResultArchiver : InspResultArchiver
    {
        //protected void InitInspectionResultColumns(DataTable dataTable)
        //{
        //    dataTable.Columns.Add(StringManager.GetString(this.GetType().FullName, "InspectionStep"), typeof(string));
        //    dataTable.Columns.Add(StringManager.GetString(this.GetType().FullName, "GroupId"), typeof(string));
        //    dataTable.Columns.Add(StringManager.GetString(this.GetType().FullName, "TargetId"), typeof(string));
        //    dataTable.Columns.Add(StringManager.GetString(this.GetType().FullName, "TargetName"), typeof(string));
        //    dataTable.Columns.Add(StringManager.GetString(this.GetType().FullName, "PropeId"), typeof(string));
        //    dataTable.Columns.Add(StringManager.GetString(this.GetType().FullName, "Type"), typeof(string));
        //    dataTable.Columns.Add(StringManager.GetString(this.GetType().FullName, "Result"), typeof(string));
        //    dataTable.Columns.Add(StringManager.GetString(this.GetType().FullName, "ValueData"), typeof(string));
        //}

        public void Save(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            string fileName = String.Format("{0}\\result.csv", inspectionResult.ResultPath);

            ResultCount resultCount = new ResultCount();
            inspectionResult.GetResultCount(resultCount);

            StringBuilder resultStringBuilder = new StringBuilder();

            string dateTimeString = inspectionResult.InspectionStartTime.ToString("yyyy\\/MM\\/dd HH:mm:ss");

            resultStringBuilder.Append("model_name, serial_number, barcode_number, inspection_time, inspection_result, job_operator, num_defects");
            resultStringBuilder.AppendLine();

            resultStringBuilder.Append(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                inspectionResult.ModelName, //0
                inspectionResult.InspectionNo, //1
                "",//inspectionResult.InputBarcode, //2
                dateTimeString,//3
                resultCount.Judgment.ToString(), //4
                inspectionResult.JobOperator, //5
                resultCount.numReject//6
                ));
            resultStringBuilder.AppendLine();

            foreach (KeyValuePair<string, int> defectCount in resultCount.numTargetTypeDefects)
            {
                if (defectCount.Key != "")
                {
                    resultStringBuilder.Append(String.Format("PartDefectCount , {0}, {1}", defectCount.Key, defectCount.Value));
                    resultStringBuilder.AppendLine();
                }
            }

            resultStringBuilder.Append("Header, InspectionStep, TargetGroupId, TargetId, TargetName, ProbeId, ProbeType, TargetType, ResultType, ValueCount, Name, Value, Ucl, Lcl");
            resultStringBuilder.AppendLine();

            foreach (ProbeResult probeResult in inspectionResult)
            {
                Probe probe = probeResult.Probe;
                int inspectionStep = probe.Target.TargetGroup.InspectionStep.StepNo;
                int targetGroupId = probe.Target.TargetGroup.GroupId;
                int targetId = probe.Target.Id;
                string targetName = probe.Target.Name;
                int probeId = probe.Id;
                string probeType = probe.ProbeType.ToString();
                string targetType = probe.Target.TypeName;
                int numValue = probeResult.ResultValueList.Count;

                string valueStr = "";
                foreach(ProbeResultValue resultValue in probeResult.ResultValueList)
                {
                    if (resultValue.Name == "Result")
                        continue;

                    valueStr += resultValue.Name + ";" + resultValue.Value.ToString() + ";" + resultValue.Ucl.ToString() + ";" + resultValue.Lcl.ToString() + ";" ;
                }

                resultStringBuilder.Append(String.Format("ProbeResult2, {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                    inspectionStep, targetGroupId, targetId, targetName, probeId, probeType, targetType,
                    probeResult.Judgment.ToString(), valueStr));
                resultStringBuilder.AppendLine();
            }

            File.WriteAllText(fileName, resultStringBuilder.ToString(), Encoding.Default);
        }

        public void GetProbeResult(InspectionResult inspectionResult)
        {
            string dataFile = String.Format("{0}\\result.csv", inspectionResult.ResultPath);

            string[] lines = File.ReadAllLines(dataFile, Encoding.Default);

            foreach (string line in lines)
            {
                ProbeResult probeResult;

                string[] words = line.Split(new char[] { ',' }, 2);
                if (words[0].Trim() == "ProbeResult")
                {
                    words = words[1].Split(new char[] { ',' });

                    string type = words[4].Trim();
                    probeResult = ProbeResult.CreateProbeResult((ProbeType)Enum.Parse(typeof(ProbeType), type));

                    probeResult.GroupId = Convert.ToInt32(words[0].Trim());
                    probeResult.TargetId = Convert.ToInt32(words[1].Trim());
                    probeResult.TargetName = words[2].Trim();
                    probeResult.ProbeId = Convert.ToInt32(words[3].Trim());
                    probeResult.Judgment = (Judgment)Enum.Parse(typeof(Judgment), words[6].Trim());

                    float value = Convert.ToSingle(words[7].Trim());
                    float ucl = Convert.ToSingle(words[8].Trim());
                    float lcl = Convert.ToSingle(words[9].Trim());

                    ProbeResultValue probeResultValue = new ProbeResultValue("Value", ucl, lcl, value);
                    probeResult.AddResultValue(probeResultValue);

                    inspectionResult.AddProbeResult(probeResult);
                }
                else if (words[0].Trim() == "ProbeResult2")
                {
                    words = words[1].Split(new char[] { ',' }, 9);

                    string type = words[5].Trim();
                    probeResult = ProbeResult.CreateProbeResult((ProbeType)Enum.Parse(typeof(ProbeType), type));

                    probeResult.StepNo = Convert.ToInt32(words[0].Trim());
                    probeResult.GroupId = Convert.ToInt32(words[1].Trim());
                    probeResult.TargetId = Convert.ToInt32(words[2].Trim());
                    probeResult.TargetName = words[3].Trim();
                    probeResult.ProbeId = Convert.ToInt32(words[4].Trim());
                    probeResult.Judgment = (Judgment)Enum.Parse(typeof(Judgment), words[7].Trim());

                    string[] valueStrList = words[8].Trim().Split('/');

                    if (valueStrList.Count() == 1)
                    {
                        string[] tokens = valueStrList[0].Split(';');
                        int tokenCount = (tokens.Count() / 4) * 4;
                        for (int i=0; i< tokenCount; i += 4)
                        {
                            ProbeResultValue resultValue = null;
                            string valueStr = tokens[i + 1];
                            if (valueStr.IndexOf('{') > -1)
                            {
                                object value = null;
                                if (valueStr.IndexOf("Width") > -1)
                                {
                                    valueStr = valueStr.Trim(new char[] { '{', '}' });
                                    string[] valueToken = valueStr.Split(',');

                                    float width = Convert.ToSingle(valueToken[0].Split('=')[1]);
                                    float height = Convert.ToSingle(valueToken[1].Split('=')[1]);
                                    value = new SizeF(width, height);
                                }

                                resultValue = new ProbeResultValue(tokens[i], value);
                            }
                            else
                            {
                                resultValue = new ProbeResultValue(tokens[i], Convert.ToSingle(tokens[i + 2]), Convert.ToSingle(tokens[i + 3]), Convert.ToSingle(valueStr));
                            }
                            probeResult.AddResultValue(resultValue);
                        }
                    }
                    else
                    {
                        foreach (string valueStr in valueStrList)
                        {
                            if (String.IsNullOrEmpty(valueStr) == false)
                            {
                                string[] tokens = valueStr.Split(';');
                                ProbeResultValue resultValue = new ProbeResultValue(tokens[0], Convert.ToSingle(tokens[2]), Convert.ToSingle(tokens[3]), Convert.ToSingle(tokens[1]));

                                probeResult.AddResultValue(resultValue);
                            }
                        }
                    }

                    inspectionResult.AddProbeResult(probeResult);
                }
            }
        }

        public InspectionResult Load(string dataPath)
        {
            if (File.Exists(dataPath) == false)
                return null;

            InspectionResult inspectionResult = null;
            try
            {
                inspectionResult = LoadInspResult(dataPath);
            }
            catch (Exception ex)
            {
                LogHelper.Warn(LoggerType.Operation, "Fail to read result data. " + ex.Message);
            }

            return inspectionResult;
        }

        InspectionResult LoadInspResult(string dataPath)
        {
            if (File.Exists(dataPath) == false)
                return null;

            InspectionResult inspectionResult = new InspectionResult();

            using (StreamReader reader = new StreamReader(dataPath))
            {
                reader.ReadLine(); // Skip
                string[] words = reader.ReadLine().Split(new char[] { ',' });

                if (words.Count() == 6)
                {
                    inspectionResult.ModelName = words[0].Trim();
                    //inspectionResult.InputBarcode = words[1].Trim();
                    inspectionResult.InspectionNo = "";// inspectionResult.InputBarcode;
                    inspectionResult.InspectionStartTime = DateTime.Parse(words[2]);

                    inspectionResult.Judgment = (Judgment)Enum.Parse(typeof(Judgment), words[3].Trim());
                    inspectionResult.JobOperator = words[4].Trim();
                }
                else
                {
                    inspectionResult.ModelName = words[0].Trim();
                    inspectionResult.InspectionNo = words[1].Trim();
                    //inspectionResult.InputBarcode = words[2].Trim();
                    inspectionResult.InspectionStartTime = DateTime.Parse(words[3]);

                    inspectionResult.Judgment = (Judgment)Enum.Parse(typeof(Judgment), words[4].Trim());
                    inspectionResult.JobOperator = words[5].Trim();
                }
                inspectionResult.ResultPath = dataPath.Trim();
            }

            return inspectionResult;
        }

        //public InspectionResult LoadInspResult(string dataPath)
        //{
        //    string dataFile = String.Format("{0}\\result.csv", dataPath);

        //    if (File.Exists(dataFile) == false)
        //        return null;

        //    InspectionResult inspectionResult = new InspectionResult();

        //    using (StreamReader reader = new StreamReader(dataFile))
        //    {
        //        reader.ReadLine(); // Skip
        //        string[] words = reader.ReadLine().Split(new char[] { ',' });

        //        if (words.Count() == 6)
        //        {
        //            inspectionResult.ModelName = words[0].Trim();
        //            //inspectionResult.InputBarcode = words[1].Trim();
        //            inspectionResult.InspectionNo = "";// inspectionResult.InputBarcode;
        //            inspectionResult.InspectionStartTime = DateTime.Parse(words[2]);

        //            inspectionResult.Judgment = (Judgment)Enum.Parse(typeof(Judgment), words[3].Trim());
        //            inspectionResult.JobOperator = words[4].Trim();
        //        }
        //        else
        //        {
        //            inspectionResult.ModelName = words[0].Trim();
        //            inspectionResult.InspectionNo = words[1].Trim();
        //            //inspectionResult.InputBarcode = words[2].Trim();
        //            inspectionResult.InspectionStartTime = DateTime.Parse(words[3]);

        //            inspectionResult.Judgment = (Judgment)Enum.Parse(typeof(Judgment), words[4].Trim());
        //            inspectionResult.JobOperator = words[5].Trim();
        //        }
        //        inspectionResult.ResultPath = dataPath.Trim();
        //    }

        //    return inspectionResult;
        //}
    }
}
