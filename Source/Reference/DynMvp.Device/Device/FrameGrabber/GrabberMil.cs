using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.Xml;

using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;

using Matrox.MatroxImagingLibrary;

namespace DynMvp.Devices.FrameGrabber
{
    public enum MilSystemType { Solios, Rapixo}

    public class CameraInfoMil : CameraInfo
    {
        public MilSystemType SystemType { get; set; }
        public CameraType CameraType { get; set; }
        public EClientType ClientType { get; set; }
        public uint SystemNum { get; set; }
        public uint DigitizerNum { get; set; }
        public bool UseChunkMemory { get; set; }
        public uint GrabBufferCount { get; set; }

        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string DcfFilePath { get; set; }

        public CameraInfoMil()
        {
            GrabberType = GrabberType.MIL;
        }

        public override void LoadXml(XmlElement xmlElement)
        {
            base.LoadXml(xmlElement);

            this.SystemType = (MilSystemType)Enum.Parse(typeof(MilSystemType), XmlHelper.GetValue(xmlElement, "SystemType", "Solios"));
            this.SystemNum = Convert.ToUInt32(XmlHelper.GetValue(xmlElement, "SystemNum", "0"));
            this.DigitizerNum = Convert.ToUInt32(XmlHelper.GetValue(xmlElement, "DigitizerNum", "0"));
            this.CameraType = XmlHelper.GetValue(xmlElement, "CameraType", CameraType.PrimeTech_PXCB120VTH);
            this.ClientType = XmlHelper.GetValue(xmlElement, "ClientType", EClientType.Master);
            this.DcfFilePath = XmlHelper.GetValue(xmlElement, "DcfFilePath", "");
            this.UseChunkMemory = XmlHelper.GetValue(xmlElement, "UseChunkMemory", false);
            this.GrabBufferCount = Convert.ToUInt32(XmlHelper.GetValue(xmlElement, "GrabBufferCount", "15"));
        }

        public override void SaveXml(XmlElement xmlElement)
        {
            base.SaveXml(xmlElement);

            XmlHelper.SetValue(xmlElement, "SystemType", this.SystemType.ToString());
            XmlHelper.SetValue(xmlElement, "SystemNum", this.SystemNum.ToString());
            XmlHelper.SetValue(xmlElement, "DigitizerNum", this.DigitizerNum.ToString());
            XmlHelper.SetValue(xmlElement, "CameraType", this.CameraType);
            XmlHelper.SetValue(xmlElement, "ClientType", this.ClientType);
            XmlHelper.SetValue(xmlElement, "DcfFilePath", this.DcfFilePath);
            XmlHelper.SetValue(xmlElement, "UseChunkMemory", this.UseChunkMemory);
            XmlHelper.SetValue(xmlElement, "GrabBufferCount", this.GrabBufferCount);
        }

        internal string GetDcfFile(bool calibrationMode)
        {
            if (string.IsNullOrEmpty(this.DcfFilePath))
            {
                switch (this.CameraType)
                {
                    case CameraType.PrimeTech_PXCB120VTH:
                        return "MIL90_PXCB120VTH1_1tap_HW.dcf";
                    case CameraType.Crevis_MC_D500B:
                        return "MIL10_SOL_5MCREVIS_2TAP_HWTRIG.dcf";
                    case CameraType.PrimeTech_PXCB16QWTPM:
                        return "HWTRIG.dcf";
                    case CameraType.PrimeTech_PXCB16QWTPMCOMPACT:
                        return "HWTRIG2.dcf";
                    case CameraType.HV_B550CTRG1:
                        return "HV_B550C_TRG1.dcf";
                    case CameraType.HV_B550CTRG2:
                        return "HV_B550C_TRG2.dcf";
                    case CameraType.EliixaPlus16K:
                        return "Eliixa16K.dcf";
                    case CameraType.UNiiQA:
                        return "UNiiQA.dcf";
                }

                return "Default.dcf";
            }

            if (calibrationMode)
            {
                string directory = System.IO.Path.GetDirectoryName(this.DcfFilePath);
                string name = System.IO.Path.GetFileNameWithoutExtension(this.DcfFilePath);
                string ext = System.IO.Path.GetExtension(this.DcfFilePath);
                string newName = $"{name}_Calib{ext}";
                return System.IO.Path.Combine(directory, newName);
            }
            return this.DcfFilePath;
        }

    }

    public class MilSystem
    {
        public string SystemDescriptor { get; set; }
        
        public uint SystemNum { get; set; }

        public MIL_ID SystemId { get;private set; }

        public int System => (int)SystemId;

        public MilSystem(string systemDescriptor, uint systemNum)
        {
            this.SystemDescriptor = systemDescriptor;
            this.SystemNum = systemNum;
        }

        public void Alloc()
        {
            if (this.SystemId != MIL.M_NULL)
                return;

            this.SystemId = MIL.MsysAlloc(MIL.M_DEFAULT, this.SystemDescriptor, this.SystemNum, MIL.M_DEFAULT, MIL.M_NULL);
            if (this.SystemId == MIL.M_NULL)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Can't Allocate MIL System. {0}, {1}", this.SystemDescriptor, this.SystemNum));
                throw new Exception($"MilSystem Alloc Fail - Descriptor: {this.SystemDescriptor}, Num: {this.SystemNum}");
            }
        }

        public void Free()
        {
            if (this.SystemId != MIL.M_NULL)
            {
                MIL.MsysFree(this.SystemId);
                this.SystemId = MIL.M_NULL;
            }
        }
    }

    public class GrabberMil : Grabber
    {
        static List<MilSystem> milSystemList = new List<MilSystem>();

        public GrabberMil(string name) : base(GrabberType.MIL, name)
        {
            LogHelper.Debug(LoggerType.StartUp, "MIL Grabber is Created");
        }

        public override Camera CreateCamera(CameraInfo cameraInfo)
        {
            var info = cameraInfo as CameraInfoMil;
            switch (info.SystemType)
            {
                case MilSystemType.Rapixo:
                    return new CameraMilCXP(cameraInfo);

                case MilSystemType.Solios:
                    return new CameraMil(cameraInfo);

                default:
                    throw new NotImplementedException();
            }
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            GeneralCameraListForm form = new GeneralCameraListForm();
            //MatroxBoardListForm form = new MatroxBoardListForm();
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        static string GetSystemDescriptor(MilSystemType systemType)
        {
            string str;
            switch (systemType)
            {
                case MilSystemType.Solios:
                    str = MIL.M_SYSTEM_SOLIOS;
                    break;
                case MilSystemType.Rapixo:
                    str = MIL.M_SYSTEM_RAPIXOCXP;
                    break;

                default:
                    str = MIL.M_SYSTEM_DEFAULT;
                    break;
            }
            return str;
        }

        public static MilSystem GetMilSystem(MilSystemType systemType, uint systemNum)
        {
            string systemDescriptor = GetSystemDescriptor(systemType);

            return GetMilSystem(systemDescriptor, systemNum);

        }

        private static MilSystem GetMilSystem(string systemDescriptor, uint systemNum)
        {
            MilSystem milSystem = milSystemList.Find(x => x.SystemDescriptor == systemDescriptor && x.SystemNum == systemNum);
            if (milSystem == null)
            {
                milSystem = new MilSystem(systemDescriptor, systemNum);
                milSystem.Alloc();
            }

            return milSystem;
        }

        public override bool Initialize(GrabberInfo grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize MultiCam Camera Manager");

            MatroxHelper.InitApplication();

            return true;
        }

        public override void Release()
        {
            base.Release();

            LogHelper.Debug(LoggerType.Shutdown, "Release MilSystem");

            foreach(MilSystem milSystem in milSystemList)
            {
                MIL.MsysFree(milSystem.SystemId);
            }
        }
    }
}
