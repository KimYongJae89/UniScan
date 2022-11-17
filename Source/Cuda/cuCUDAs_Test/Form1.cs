using DynMvp.UI;
using DynMvp.Vision.Cuda;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cuCUDAs_Test
{
    public partial class Form1 : Form
    {
        enum LogLevel { Info, Error}
        List<uint> imageList;

        public Form1()
        {
            InitializeComponent();

            this.imageList = new List<uint>();
        }

        private void ConsoleWriteLineColor(string str, ConsoleColor fg, ConsoleColor bg)
        {
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;

            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] {str}");

            //StackTrace st = new StackTrace();
            //StackFrame sf = st.GetFrame(2);
            //string methodName = sf.GetMethod().Name;

            //Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] {methodName} -> {str}");
        }

        private void ConsoleWriteLine(string str, LogLevel level= LogLevel.Info)
        {
            ConsoleColor fg = ConsoleColor.White;
            ConsoleColor bg = (level == LogLevel.Info ? ConsoleColor.Black : ConsoleColor.Red);
            ConsoleWriteLineColor(str, fg, bg);
        }

        private void UpdateInitButtens(bool isInitialized)
        {
            this.buttonInit.Enabled = isInitialized;
            this.buttonReset.Enabled = isInitialized;
        }

        private async Task ThrowIfOnError(Action action)
        {
            await Task.Run(() =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    ConsoleWriteLine($"{ex.GetType().Name}: {ex.Message}", LogLevel.Error);
                }
            });
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await ThrowIfOnError(() =>
            {
                ConsoleWriteLine("Search Cuda Devices");
                int deviceCount = CudaMethods.CUDA_DEVICE_COUNT();
                ConsoleWriteLine($"Device founded. Count: {deviceCount}");

                UiHelper.SetNumericMinMax(this.numericUpDown1, 0, deviceCount - 1);
                
                if (deviceCount == 0)
                {
                    ConsoleWriteLine($"There is no device. Abort.");
                    UpdateInitButtens(false);
                }

                for (int i = 0; i < deviceCount; i++)
                {
                    IntPtr ptr = CudaMethods.CUDA_DEVICE_NAME(i);
                    string deviceName = Marshal.PtrToStringAnsi(ptr);
                    ConsoleWriteLine($"Device {i}: {deviceName}");
                }

                CudaTest.SelectDevice(0);
                UiHelper.SetNumericValue(this.numericUpDown1, 0);
            });
        }

        private async void buttonSelect_Click(object sender, EventArgs e)
        {
            await ThrowIfOnError(() =>
            {
                int deviceId = (int)this.numericUpDown1.Value;
                CudaTest.SelectDevice(deviceId);
                ConsoleWriteLine($"Device initialize success. DeviceNo: {deviceId }");
            });
        }

        private async void buttonReset_Click(object sender, EventArgs e)
        {
            await ThrowIfOnError(() =>
            {
                ConsoleWriteLine($"Reset devices.");
                CudaTest.ResetDevice();
            });
        }

        private async void buttonAlloc_Click(object sender, EventArgs e)
        {
            await ThrowIfOnError(() =>
            {
            Size size = new Size(10000, 10000);
            uint id = CudaTest.Alloc(size);
            ConsoleWriteLine($"Allocated Image {id}. Size: {size}");

            this.imageList.Add(id);
            });
        }

        private async void buttonFree_Click(object sender, EventArgs e)
        {
            await ThrowIfOnError(() =>
            {
            if (this.imageList.Count == 0)
            {
                ConsoleWriteLine($"There is no allocated Image. Abort");
                return;
            }

            uint id = this.imageList.Last();
            CudaTest.Free(id);

            ConsoleWriteLine($"Image {id} Disposed.");
            this.imageList.Remove(id);
            });
        }
    }
}
