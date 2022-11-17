using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;
using OpenHardwareMonitor.Hardware;

namespace UniScanG.Common
{
    class LogHeader
    {
        public bool IsHeader => this.isHeader;
        bool isHeader;

        public HardwareType HardwareType => this.hardwareType;
        HardwareType hardwareType;

        public string Identifier => this.identifier;
        string identifier;

        public string SensorName => this.sensorName;
        string sensorName;

        public SensorType SensorType => this.sensorType;
        SensorType sensorType;

        public LogHeader()
        {
            this.isHeader = true;
        }

        public LogHeader(HardwareType hardwareType, string identifier, string sensorName, SensorType sensorType)
        {
            this.isHeader = false;
            this.hardwareType = hardwareType;
            this.identifier = identifier;
            this.sensorName = sensorName;
            this.sensorType = sensorType;
        }

        public override bool Equals(object obj)
        {
            LogHeader logHeader = obj as LogHeader;
            if (logHeader == null)
                return false;

            if (this.hardwareType != logHeader.hardwareType)
                return false;

            if(this.identifier != logHeader.identifier)
                return false;

            if (this.sensorName != logHeader.sensorName)
                return false;

            if (this.sensorType != logHeader.sensorType)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.hardwareType.GetHashCode() ^ this.identifier.GetHashCode() ^ this.sensorName.GetHashCode() ^ this.sensorType.GetHashCode();
        }
    }

    class LogItem
    {
        public LogHeader Header { get; set; }
        public string Value { get; set; }

        public LogItem(LogHeader header, string value)
        {
            this.Header = header;
            this.Value = value;
        }
    }

    public class HardwareMonitor : ThreadHandler
    {
        public static string FileNameFormat = "yyyy-MM-dd";
        string logPath;
        Computer computer;
        int intervalMs;

        string fileStreamPath;
        FileStream fileStream;
        bool isNewFile;
        bool forceWirteHeader;

        List<LogHeader> headerList = new List<LogHeader>();

        public string LogFilePath => Path.Combine(logPath, string.Format("{0}.csv", DateTime.Now.Date.AddDays(0).ToString(FileNameFormat)));

        public HardwareMonitor(string logPath, bool createDirectory)
            : base("HardwareMonitor")
        {
            this.logPath = logPath;
            this.computer = new Computer() { CPUEnabled = true, FanControllerEnabled = true, RAMEnabled = true, GPUEnabled = true, MainboardEnabled = true, HDDEnabled = true };

            if (createDirectory)
                Directory.CreateDirectory(logPath);

            this.headerList.Add(new LogHeader());
        }

        public void Start(int intervalMs)
        {
            try
            {
                //UpdateFileStream();

                this.computer.Open();
                this.intervalMs = intervalMs;
                this.forceWirteHeader = true;
            }
            catch (Exception ex)
            {
                do
                {
                    LogHelper.Error(LoggerType.Error, string.Format("HardwareMonitor::Start - {0} : {1}", ex.GetType().Name, ex.Message));
                    ex = ex.InnerException;
                } while (ex != null);
                return;
            }
            this.workingThread = new System.Threading.Thread(ThreadProc);
            this.requestStop = false;
            base.Start();
            LogHelper.Debug(LoggerType.StartUp, "HardwareMonitor::Start - OK");
        }

        private void ThreadProc()
        {
            while (!this.requestStop)
            {
                WriteLog();
                System.Threading.Thread.Sleep(this.intervalMs);
            }
        }

        private void UpdateFileStream()
        {
            string logFilePath = this.LogFilePath;
            if (string.Equals(this.fileStreamPath, logFilePath))
                return;

            this.fileStream?.Dispose();

            this.fileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            this.isNewFile = (this.fileStream.Length == 0);
            this.forceWirteHeader = true;

            this.fileStreamPath = logFilePath;
        }

        public void Stop()
        {
            base.Stop();
            try
            {
                this.computer.Close();
            }
            catch (Exception) { }

            if (this.fileStream != null)
            {
                this.fileStream.Close();
                this.fileStream.Dispose();
                this.fileStream = null;
            }

            //System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            //psi.FileName = "cmd.exe";
            //psi.RedirectStandardInput = true;
            //psi.UseShellExecute = false;

            //System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi);
            //process.StandardInput.Write("sc query WinRing0_1_2_0" + Environment.NewLine);
            //process.StandardInput.Write("net stop WinRing0_1_2_0" + Environment.NewLine);
            //process.StandardInput.Close();

            //process.WaitForExit();
        }

        private void WriteLog()
        {
            try
            {
                DateTime row = DateTime.Now;
                UpdateFileStream();

                List<LogItem> logItemList = new List<LogItem>();
                logItemList.Add(new LogItem(this.headerList.First(), row.ToString("HH:mm:ss")));

                Array.ForEach(this.computer.Hardware, f =>
                {
                    f.Update();

                    foreach (ISensor sensor in f.Sensors)
                    {
                        if (sensor.Value.HasValue)
                        {
                            LogHeader logHeader = new LogHeader(f.HardwareType, f.Identifier.ToString(), sensor.Name, sensor.SensorType);
                            if (!this.headerList.Contains(logHeader))
                                this.headerList.Add(logHeader);

                            LogItem logItem = new LogItem(logHeader, sensor.Value.ToString());
                            logItemList.Add(logItem);
                        }
                    }
                });

                if (this.isNewFile || this.forceWirteHeader)
                {
                    if(!this.isNewFile)
                    {
                        byte[] bytes = Encoding.Default.GetBytes(Environment.NewLine);
                        this.fileStream.Write(bytes, 0, bytes.Length);
                    }
                    StringBuilder sbHeader = new StringBuilder();
                    sbHeader.AppendLine(string.Join(",", this.headerList.ConvertAll(f => f.IsHeader ? "" : f.HardwareType.ToString())));
                    sbHeader.AppendLine(string.Join(",", this.headerList.ConvertAll(f => f.IsHeader ? "" : f.Identifier)));
                    sbHeader.AppendLine(string.Join(",", this.headerList.ConvertAll(f => f.IsHeader ? "" : f.SensorName)));
                    sbHeader.AppendLine(string.Join(",", this.headerList.ConvertAll(f => f.IsHeader ? "" : f.SensorType.ToString())));
                    WriteStream(sbHeader.ToString());

                    this.forceWirteHeader = this.isNewFile = false;
                }

                StringBuilder sbValue = new StringBuilder();
                this.headerList.ForEach(f => sbValue.AppendFormat("{0},", logItemList.Find(g => g.Header.Equals(f))?.Value));
                sbValue.AppendLine();

                WriteStream(sbValue.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("HardwareMonitor::WriteLog - {0}", ex.Message));
            }
            finally
            {
            }
            //foreach (IHardware subHardware in hardware.SubHardware)
            //    WriteLog(subHardware, string.Format("{0}:{1}", header, subHardware.HardwareType));
        }

        private void WriteStream(string v)
        {
            byte[] bytes = Encoding.Default.GetBytes(v);
            try
            {
                this.fileStream.Write(bytes, 0, bytes.Length);
                this.fileStream.Flush();
            }catch(Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("HardwareMonitor::WriteStream - {0} {1}", ex.ToString(), ex.Message));
            }
        }
    }
}
