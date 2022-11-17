using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnieyeLauncher.Operation
{
    public enum EventType { Start, Stop, Message };

    public delegate void EventHandlerDelegate(Operator sender, EventType eventType, string message);
    public abstract class Operator
    {
        public event EventHandlerDelegate EventHandler;

        private static FileInfo FileInfoDebug = new FileInfo(Path.GetFullPath(Path.Combine(Program.WorkingDirectory, "Log", "Launcher_Debug.log")));
        private static FileInfo FileInfoError = new FileInfo(Path.GetFullPath(Path.Combine(Program.WorkingDirectory, "Log", "Launcher_Error.log")));

        private static StreamWriter StreamWriterDebug;
        private static StreamWriter StreamWriterError;

        public string WorkingDirectory => this.workingDirectory;
        protected string workingDirectory;

        public abstract bool Use { get; }

        public bool IsRun { get; protected set; }

        public bool IsActive { get; protected set; }

        public string Bin => Path.Combine(this.workingDirectory, "Bin");

        public string Update => Path.Combine(this.workingDirectory, "Update");

        public Operator()
        {
            this.workingDirectory = Program.WorkingDirectory;
        }

        protected void OnEvent(EventType eventType, string message)
        {
            WriteLog($"{eventType}-{message}");
            EventHandler?.Invoke(this, eventType, message);
        }

        internal virtual Color GetStripColor()
        {
            return this.IsActive ? Color.Green :
                this.IsRun ? Color.LightGreen :
               this.Use ? Color.Yellow : Control.DefaultBackColor;
        }

        public static void WriteLog(Exception ex)
        {
            DateTime now = DateTime.Now;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("[{0}] {1}: {2}", now.ToString("yyyy-MM-dd HH:mm:ss"), ex.GetType().Name, ex.Message));
            sb.AppendLine(ex.StackTrace);
            sb.AppendLine("============================");

            WriteLog(Operator.FileInfoError, ref Operator.StreamWriterError, sb.ToString());
        }

        public static void WriteLog(string str)
        {
            WriteLog(Operator.FileInfoDebug, ref Operator.StreamWriterDebug, str);
        }

        private static void WriteLog(FileInfo fileInfo, ref StreamWriter streamWriter, string message)
        {
            DateTime now = DateTime.Now;

            if (fileInfo.CreationTime.Date != now.Date)
            {
                string file = fileInfo.FullName;
                string moveTo = $"{fileInfo.FullName}.{fileInfo.CreationTime.Date.ToString("yyyy-MM-dd")}";

                streamWriter?.Close();
                streamWriter = null;

                if (File.Exists(moveTo))
                    File.Delete(moveTo);

                if (File.Exists(file))
                    File.Move(file, moveTo);
            }

            if (streamWriter == null)
            {
                if (!fileInfo.Directory.Exists)
                    fileInfo.Directory.Create();
                streamWriter = new StreamWriter(fileInfo.FullName, true);
            }

            streamWriter.WriteLine(string.Format("[{0}] {1}", now.ToString("yyyy-MM-dd HH:mm:ss"), message));
            streamWriter.Flush();
            fileInfo.Refresh();
        }
    }
}
