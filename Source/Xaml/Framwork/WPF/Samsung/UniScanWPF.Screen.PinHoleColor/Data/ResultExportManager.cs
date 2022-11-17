using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanWPF.Screen.PinHoleColor.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.Data
{
    public class ResultExportManager : INotifyPropertyChanged
    {
        CancellationTokenSource source;

        Thread exportThread;
        ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        Queue<InspectResult> resultQueue = new Queue<InspectResult>();

        static ResultExportManager instance;

        public int QueueCount { get => resultQueue.Count; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static ResultExportManager Instance()
        {
            if (instance == null)
                instance = new ResultExportManager();

            return instance;
        }
        
        public void ExportResult(InspectResult inspectResult)
        {
            lock (inspectResult)
                resultQueue.Enqueue(inspectResult);

            OnPropertyChanged("QueueCount");

            manualResetEvent.Set();
        }

        public void Start()
        {
            manualResetEvent.Reset();

            source = new CancellationTokenSource();

            exportThread = new Thread(ThreadProc);
            exportThread.IsBackground = true;
            exportThread.Priority = ThreadPriority.Highest;
            exportThread.Start();
        }

        public void Stop(int milliSecond = 5000)
        {
            source.Cancel();

            manualResetEvent.Set();

            if (!exportThread.Join(TimeSpan.FromMilliseconds(milliSecond)))
            {
                exportThread.Abort();
                LogHelper.Debug(LoggerType.Operation, "Export Thread abort");
            }
        }

        public void ThreadProc()
        {
            while (source.IsCancellationRequested == false || resultQueue.Count != 0)
            {
                if (resultQueue.Count == 0)
                    manualResetEvent.Reset();

                manualResetEvent.WaitOne();
                if (resultQueue.Count == 0)
                    continue;

                InspectResult inspectResult = null;
                lock (resultQueue)
                    inspectResult = resultQueue.Dequeue();

                if (inspectResult == null)
                    continue;

                OnPropertyChanged("QueueCount");

                inspectResult.ExportResult();
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
