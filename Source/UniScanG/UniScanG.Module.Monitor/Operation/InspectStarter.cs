using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniScanG.Data;
using UniScanG.MachineIF;
using UniScanG.Module.Monitor.Device;

namespace UniScanG.Module.Monitor.Operation
{
    public enum StartMode { Auto,Force,Stop}

    public class InspectStarter : ThreadHandler
    {
        MachineIfData machineIfData = null;

        public StartMode StartMode { get => this.startMode; set => this.startMode = value; }
        StartMode startMode = StartMode.Auto;

        public InspectStarter() : base("InspectStarter",null , false)
        {
            this.workingThread = new Thread(ThreadProc);
            //this.workingThread.SetApartmentState(ApartmentState.STA);

            this.machineIfData = ((DeviceController)SystemManager.Instance().DeviceController).MachineIfMonitor?.MachineIfData as MachineIfData;            
        }

        private void ThreadProc()
        {
            Thread.Sleep(5000);
            string oldLot = machineIfData.GET_LOT;
            RewinderZone oldRewinderZone = machineIfData.RewinderZone;

            while (!this.requestStop)
            {
                Thread.Sleep(500);

                bool isRunning = SystemState.Instance().IsInspectOrWait;
                bool isAutoMode = this.startMode == StartMode.Auto;

                if (isRunning)
                {
                    if (this.startMode == StartMode.Stop || (isAutoMode && !machineIfData.GET_START_STILLIMAGE))
                    {
                        SystemManager.Instance().InspectRunner.ExitWaitInspection();
                    }
                    else if (oldRewinderZone != machineIfData.RewinderZone || oldLot != machineIfData.GET_LOT)
                    {
                        LotChange();
                    }
                }
                else
                {
                    if ((this.startMode == StartMode.Force || (isAutoMode && machineIfData.GET_START_STILLIMAGE)))
                    {
                        try
                        {
                            SystemManager.Instance().InspectRunner.EnterWaitInspection();
                        }catch(Exception ex)
                        {
                            this.StartMode = StartMode.Stop;
                            SystemManager.Instance().InspectRunner.ExitWaitInspection();

                            App.Current.Dispatcher.Invoke(() =>
                            {
                                WpfControlLibrary.UI.CustomMessageBox.Show(ex.Message);
                            });
                        }
                    }
                }

                oldLot = machineIfData.GET_LOT;
                oldRewinderZone = machineIfData.RewinderZone;
            }
        }

        public void LotChange()
        {
            string newLot = machineIfData.GET_LOT;
            if (string.IsNullOrEmpty(newLot))
                newLot = DateTime.Now.ToString("yyyyMMdd");

            int count = SystemManager.Instance().ProductionManager.GetProductions(SystemManager.Instance().CurrentModel.ModelDescription, DateTime.Now, newLot, machineIfData.RewinderZone).Length;
            if (count > 0)
                newLot = string.Format("{0}.{1}", newLot, count);

            SystemManager.Instance().ProductionManager.LotChange(SystemManager.Instance().CurrentModel, DateTime.Now, newLot, machineIfData.RewinderZone, machineIfData.GET_TARGET_SPEED_REAL);
        }
    }
}
