using DynMvp.Authentication;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UniEye.Base.UI
{
    public delegate void UpdateControlDelegate(string item, object value);
    public delegate void ClearPanelDelegate();
    
    public interface IPage
    {
        /// <summary>
        /// 컨트롤의 Text 등을 변경하고자 할 때
        /// </summary>
        /// <param name="item"></param>
        /// <param name="value"></param>
        void UpdateControl(string item, object value);

        /// <summary>
        /// Visible 속성 변경시
        /// </summary>
        /// <param name="visibleFlag"></param>
        void PageVisibleChanged(bool visibleFlag);
    }

    public interface IImageContainer
    {

    }

    public delegate void OnModelChangedDelegate();
    //public delegate void OnLotChangedDelegate();
    public delegate void OnWorkerChangedDelegate(string opName);

    public interface IMainForm 
    {
        IInspectionPage InspectPage { get; }
        ITeachPage TeachPage { get; }
        IModelManagerPage ModelManagerPage { get; }
        IReportPage ReportPage { get; }
        ISettingPage SettingPage { get; }
        
        void EnableTabs();
        void PageChange(IMainTabPage page, UserType userType = UserType.Maintrance);

        void OnModelChanged();
        void WorkerChanged(string opName);

        void ModifyTeaching(string imageFileName);
    }

    public interface IRemoteControl
    {
        void RcChangeMode(string modeStr);
        void RcTeach();
        void RcInspect();
        void RcGrab();
        void RcStopGrab();
    }

    public interface IMainTabPage: IPage
    {
        void EnableControls(UserType userType);
    }

    public interface IModelManagerPage : IMainTabPage
    {

    }

    public interface IInspectionPage : IMainTabPage
    {
        List<IInspectionPanel> InspectionPanelList { get; }
        void ProductInspected(InspectionResult inspectionResult);
    }

    public delegate void ProductInspectedDelegate(InspectionResult inspectionResult);

    public interface IInspectionPanel
    {
        void Initialize();

        void ClearPanel();
        void EnterWaitInspection();
        void ExitWaitInspection();

        void OnPreInspection();
        void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult);
        void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult);
        void TargetInspected(Target target, InspectionResult targetInspectionResult);
        void ProductInspected(InspectionResult inspectionResult);
        void OnPostInspection();
        void ModelChanged(Model model = null);
        void InfomationChanged(object obj = null);

        
    }

    public interface ITeachPage : IMainTabPage
    {
        
    }

    public interface IReportPage : IMainTabPage
    {
        void Initialize();
        void Refresh();
        void ModelAutoSelector();
        void Clear();
    }

    public interface IReportPanel
    {
        void Initialize();
        void Refresh();
        void Clear();
        void Search(ProductionBase production);
        //void Export(ProductionBase[] productions);
    }

    public interface ISettingPage : IMainTabPage
    {
        void Initialize();
        void SaveSettings();
    }
    public interface IModellerPage : IMainTabPage
    {

    }

}
