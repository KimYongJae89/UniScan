using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;

namespace UniScanG.Common.Util
{
    public interface IVncControl
    {
        void InitHandle(IntPtr handle);
        void Disable();
        void Enable();
        void ExitProcess();
        void ProcessExited(object sender, EventArgs e);
        InspectorObj GetInspector();
    }

    public static class VncHelper
    {
        [DllImport("User32", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern void SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_SHOWNORMAL = 1;

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        public static bool FindWindow(Process process)
        {
            IntPtr procHandler = FindWindow(null, process.MainWindowTitle);

            if (procHandler == null)
                return false;

            // 활성화
            ShowWindow(procHandler, SW_SHOWNORMAL);
            SetForegroundWindow(procHandler);

            return true;
        }

        public static Process OpenVnc(Process process, string ipAddress, IntPtr handle, string vncPath)
        {
            bool existWindow = false;

            if (process != null)
            {
                existWindow = FindWindow(process);
                return process;
            }

            Process newProcess = OpenVnc(ipAddress, vncPath);
            
            if (newProcess != null)
            {
                newProcess.WaitForInputIdle();
                System.Threading.Thread.Sleep(1000);

                SetParent(newProcess.MainWindowHandle, handle);
                
                ShowWindowAsync(newProcess.MainWindowHandle, 3);
            }

            return newProcess;
        }

        private static void ProcessAsyncTask(Process process)
        {

        }

        private static Process OpenVnc(string ipAddress, string vncPath)
        {
            Process process = new Process();

            process.StartInfo.FileName = vncPath;
            process.StartInfo.Arguments = String.Format(@"{0}", ipAddress);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;

                // Simple ProgressForm에 가려서 메시지가 안보임
                Task.Run(() => MessageForm.Show(null, StringManager.GetString("Can't excute VNC Viewer.")));
                
                process = null;
            }

            return process;
        }
    }
}
