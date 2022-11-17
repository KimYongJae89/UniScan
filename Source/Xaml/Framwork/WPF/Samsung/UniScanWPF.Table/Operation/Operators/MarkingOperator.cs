//using DynMvp.Base;
//using DynMvp.Devices;
//using DynMvp.Devices.Light;
//using DynMvp.Devices.MotionController;
//using DynMvp.Vision;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Media.Imaging;
//using System.Xml;
//using UniEye.Base.Settings;
//using UniScanWPF.Table.Data;
//using UniScanWPF.Table.Inspect;
//using UniScanWPF.Table.Settings;
//using WpfControlLibrary.UI;

//namespace UniScanWPF.Table.Operation.Operators
//{
//    public class MarkingOperator : Operator
//    {
//        MarkingOperatorSettings settings;
//        public MarkingOperatorSettings Settings { get => settings; }

//        public MarkingOperator() : base()
//        {
//            settings = new MarkingOperatorSettings();
//        }
        
//        public void Marking(Defect defect)
//        {
//            float posX = defect..ScanOperatorResult.AxisPosition[0] - (defect.DefectBlob.RotateCenterPt.X * 5.0f) + settings.OffsetX;
//            float posY = defect.ExtractorResult.ScanOperatorResult.AxisPosition[1] + (defect.DefectBlob.RotateCenterPt.Y * 5.0f) + settings.OffsetY;
//            AxisPosition axisPosition = new AxisPosition(new float[] { posX, posY });

//            SystemManager.Instance().DeviceController.RobotStage.StartMultipleMove(axisPosition, settings.TargetVelocity);
//            SystemManager.Instance().DeviceController.RobotStage.WaitMoveDone();
//            UniScanWPF.Table.Device.PortMap portMap = SystemManager.Instance().DeviceBox.PortMap as UniScanWPF.Table.Device.PortMap;
//            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(portMap.GetOutPort(UniScanWPF.Table.Device.PortMap.OutPortName.OutCylinderUp), true);
//            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(portMap.GetOutPort(UniScanWPF.Table.Device.PortMap.OutPortName.OutCylinderDown), false);
//            Thread.Sleep(1000);
//            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(portMap.GetOutPort(UniScanWPF.Table.Device.PortMap.OutPortName.OutCylinderUp), false);
//            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(portMap.GetOutPort(UniScanWPF.Table.Device.PortMap.OutPortName.OutCylinderDown), true);
//        }

//        public override bool Start()
//        {
//            return true;
//        }

//        protected override bool PostGrab()
//        {
//            return true;
//        }

//        protected override bool PrepereGrab()
//        {
//            return true;
//        }
//    }

//    public class MarkingOperatorSettings : UnitSettings
//    {
//        int offsetX;
//        int offsetY;
//        float targetVelocity;

//        [CatecoryAttribute("Marking"), NameAttribute("Offset X")]
//        public int OffsetX { get => offsetX; set => offsetX = value; }

//        [CatecoryAttribute("Marking"), NameAttribute("Offset Y")]
//        public int OffsetY { get => offsetY; set => offsetY = value; }

//        [CatecoryAttribute("Marking"), NameAttribute("Velocity")]
//        public float TargetVelocity { get => targetVelocity; set => targetVelocity = value; }

//        protected override void Initialize()
//        {
//            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Marking");
//        }

//        public override void Load(XmlElement xmlElement)
//        {
//            offsetX = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "OffsetX", "0"));
//            offsetY = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "OffsetY", "0"));
//            targetVelocity = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "TargetVelocity", "150000"));
//        }

//        public override void Save(XmlElement xmlElement)
//        {
//            XmlHelper.SetValue(xmlElement, "OffsetX", offsetX.ToString());
//            XmlHelper.SetValue(xmlElement, "OffsetY", offsetY.ToString());
//            XmlHelper.SetValue(xmlElement, "TargetVelocity", targetVelocity.ToString());
//        }
//    }
//}
