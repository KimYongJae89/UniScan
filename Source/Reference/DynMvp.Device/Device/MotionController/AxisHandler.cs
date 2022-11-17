using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using System.IO;

using DynMvp.Base;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using DynMvp.Device;
using DynMvp.Device.Device.MotionController;

namespace DynMvp.Devices.MotionController
{
    public enum AxisHandlerType
    {
        None, RobotStage, Converyor
    }

    public class AxisConfiguration : List<AxisHandler>
    {
        string configurationFile = String.Empty;
        public string ConfigurationFile
        {
            get { return configurationFile; }
            set { configurationFile = value; }
        }


        public new AxisHandler this[int index]
        {
            get { return base[index]; }
        }

        public AxisHandler GetAxisHandler(string name)
        {
            return Find(x => x.Name == name);
        }

        public AxisHandler GetAxisHandler(string name, AxisHandlerType type)
        {
            return Find(x => x.Name == name.ToString() && x.HandlerType == type);
        }

        public bool LoadConfiguration(MotionList motionList)
        {
            if (String.IsNullOrEmpty(configurationFile))
                configurationFile = String.Format(@"{0}\..\Config\AxisConfiguration.xml", Environment.CurrentDirectory);

            if (File.Exists(configurationFile))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(configurationFile);

                XmlElement axisConfigurationElement = xmlDocument.DocumentElement;

                foreach (XmlElement axisHandlerElement in axisConfigurationElement)
                {
                    if (axisHandlerElement.Name == "AxisHandler")
                    {
                        string name = XmlHelper.GetValue(axisHandlerElement, "Name", "");

                        AxisHandler axisHandler = GetAxisHandler(name);
                        if (axisHandler == null)
                        {
                            axisHandler = new AxisHandler(name);
                            Add(axisHandler);
                        }
                        axisHandler.Load(axisHandlerElement, motionList);
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool LoadConfiguration()
        {
            if (String.IsNullOrEmpty(configurationFile))
                configurationFile = String.Format(@"{0}\..\Config\AxisConfiguration.xml", Environment.CurrentDirectory);

            if (File.Exists(configurationFile))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(configurationFile);

                XmlElement axisConfigurationElement = xmlDocument.DocumentElement;

                foreach (XmlElement axisHandlerElement in axisConfigurationElement)
                {
                    if (axisHandlerElement.Name == "AxisHandler")
                    {
                        string name = XmlHelper.GetValue(axisHandlerElement, "Name", "");

                        AxisHandler axisHandler = GetAxisHandler(name);
                        if (axisHandler != null)
                        {
                            axisHandler.Load(axisHandlerElement);
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void SaveConfiguration()
        {
            if (String.IsNullOrEmpty(configurationFile))
                configurationFile = String.Format(@"{0}\..\Config\AxisConfiguration.xml", Environment.CurrentDirectory);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement axisConfigurationElement = xmlDocument.CreateElement("", "AxisHandlerConfig", "");
            xmlDocument.AppendChild(axisConfigurationElement);

            foreach (AxisHandler axisHandler in this)
            {
                XmlElement axisHandlerElement = xmlDocument.CreateElement("", "AxisHandler", "");
                axisConfigurationElement.AppendChild(axisHandlerElement);

                axisHandler.Save(axisHandlerElement);
            }

            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "\t";
            xmlSettings.NewLineHandling = NewLineHandling.Entitize;
            xmlSettings.NewLineChars = "\r\n";

            XmlWriter xmlWriter = XmlWriter.Create(configurationFile, xmlSettings);

            xmlDocument.Save(xmlWriter);
            xmlWriter.Flush();
            xmlWriter.Close();
        }

        public void SetupAxisHandler(string[] axisHandlerNames)
        {
            Clear();

            foreach (string name in axisHandlerNames)
            {
                Add(new AxisHandler(name));
            }
        }

        public void Initialize(MotionList motionList)
        {
            foreach (AxisHandler axisHandler in this)
            {
                axisHandler.Initialize(motionList, 3);
            }
        }
    }

    public delegate void RobotEventDelegate(AxisPosition axisPosition);

    public class AxisHandler
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        AxisHandlerType handlerType = AxisHandlerType.None;
        public AxisHandlerType HandlerType
        {
            get { return handlerType; }
            set { handlerType = value; }
        }

        RobotEventDelegate onBeginMove;
        public RobotEventDelegate OnBeginMove
        {
            get { return onBeginMove; }
            set { onBeginMove = value; }
        }

        RobotEventDelegate onEndMove;
        public RobotEventDelegate OnEndMove
        {
            get { return onEndMove; }
            set { onEndMove = value; }
        }

        RobotAligner robotAligner = new RobotAligner();
        public RobotAligner RobotAligner
        {
            get { return robotAligner; }
            set { robotAligner = value; }
        }

        private static int motionDoneCheckIntervalMs = 10;
        public static int MotionDoneCheckIntervalMs
        {
            get { return AxisHandler.motionDoneCheckIntervalMs; }
            set { AxisHandler.motionDoneCheckIntervalMs = value; }
        }

        private static MovingProfileType movingProfileType;
        public static MovingProfileType MovingProfileType
        {
            get { return AxisHandler.movingProfileType; }
            set { AxisHandler.movingProfileType = value; }
        }

        AxisPosition lastAxisPosition = null;
        public AxisPosition LastAxisPosition
        {
            get { return lastAxisPosition; }
        }

        List<Axis> axisList = new List<Axis>();
        public List<Axis> AxisList
        {
            get { return axisList; }
        }

        // 두 개 이상의 축을 동기 구동해야 할 경우, 고유 축만의 목록
        List<Axis> uniqueAxisList = new List<Axis>();
        public List<Axis> UniqueAxisList
        {
            get { return uniqueAxisList; }
        }

        public Axis GetUniqueAxis(string axisName)
        {
            return uniqueAxisList.Find(x => x.Name == axisName);
        }

        public Axis this[int key]
        {
            get { return axisList[key]; }
            set { axisList[key] = value; }
        }

        public override string ToString()
        {
            return name;
        }

        public AxisHandler(string name)
        {
            this.name = name;
            ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmStatus;
        }

        private void ErrorManager_OnResetAlarmStatus()
        {
           axisList.ForEach(f =>
            {
                f.ResetAlarm();
                f.TurnOnServo(true);
            });
        }

        public void Clear()
        {
            axisList.Clear();
        }

        public int NumAxis
        {
            get { return axisList.Count; }
        }

        public int NumUniqueAxis
        {
            get { return uniqueAxisList.Count; }
        }

        public Axis AddAxis(string name, Motion motion, int axisNo)
        {
            return AddAxis(name, motion, axisNo, this.handlerType != AxisHandlerType.Converyor);
        }

        public Axis AddAxis(string name, Motion motion, int axisNo, bool isNeedHome)
        {
            Axis axis = new Axis(name, motion, axisNo, isNeedHome);
            axisList.Add(axis);

            if (GetUniqueAxis(name) == null)
                uniqueAxisList.Add(axis);

            return axis;
        }

        public void AddAxis(Axis axis)
        {
            if (axisList.IndexOf(axis) == -1)
                axisList.Add(axis);

            if (GetUniqueAxis(axis.Name) == null)
                uniqueAxisList.Add(axis);
        }

        public void RemoveAxisAt(int index)
        {
            axisList.RemoveAt(index);
        }

        public void Initialize(MotionList motionList, int axisNum)
        {
            this.axisList.Clear();

            //this.motionDoneWaitTimeMs = 20000;
            //this.motionDoneCheckIntervalMs = 10;
            if (motionList.Count == 0)
            {
                return;
            }

            Motion motion = motionList.GetMotion(0);

            for (int axisNo = 0; axisNo < axisNum; axisNo++)
            {
                string axisName = string.Format("{0}", (AxisName)axisNo);

                // 컨베이어는 홈을 잡을 필요 없음.
                Axis axis = new Axis(axisName, motion, axisNo, this.handlerType == AxisHandlerType.Converyor);
                axisList.Add(axis);
            }
        }

        public AxisPosition CreatePosition(string name = "")
        {
            AxisPosition axisPosition = new AxisPosition(uniqueAxisList.Count);
            axisPosition.Name = name;

            return axisPosition;
        }

        public void Load(XmlElement axisHandlerElement, MotionList motionList)
        {
            name = XmlHelper.GetValue(axisHandlerElement, "Name", "");
            handlerType = (AxisHandlerType)Enum.Parse(typeof(AxisHandlerType), XmlHelper.GetValue(axisHandlerElement, "HandlerType", AxisHandlerType.None.ToString()));
            int numAxis = Convert.ToInt32(XmlHelper.GetValue(axisHandlerElement, "NumAxis", "0"));

            axisList.Clear();

            motionDoneCheckIntervalMs = Convert.ToInt32(XmlHelper.GetValue(axisHandlerElement, "MotionDoneCheckIntervalMs", "10"));

            if (numAxis == 0)
                return;

            int axisId = 0;

            foreach (XmlElement axisElement in axisHandlerElement)
            {
                if (axisElement.Name == "Axis")
                {
                    string axisName = XmlHelper.GetValue(axisElement, "AxisName", "");
                    string motionName = XmlHelper.GetValue(axisElement, "Motion", "");
                    int axisNo = Convert.ToInt32(XmlHelper.GetValue(axisElement, "AxisNo", "0"));

                    if (String.IsNullOrEmpty(motionName))
                    {
                        ErrorManager.Instance().Report(ErrorCodeMotion.Instance.InvalidSetting, ErrorLevel.Error,
                            this.name, "Motion name is empty", null);
                        continue;
                    }

                    Motion motion = motionList.GetMotion(motionName);
                    if (motion == null)
                    {
                        ErrorManager.Instance().Report(ErrorCodeMotion.Instance.InvalidSetting, ErrorLevel.Error,
                            this.name, "Invalid Motion name", null);
                        continue;
                    }

                    Axis axis = new Axis(axisName, motion, axisNo, this.handlerType != AxisHandlerType.Converyor);
                    axisList.Add(axis);
                    if (GetUniqueAxis(axisName) == null)
                        uniqueAxisList.Add(axis);

                    axisList[axisId].AxisParam.Load(axisElement);
                    axisId++;

                    if (numAxis <= axisId)
                        break;
                }
            }
        }

        public void Load(XmlElement axisHandlerElement)
        {
            name = XmlHelper.GetValue(axisHandlerElement, "Name", "");
            handlerType = (AxisHandlerType)Enum.Parse(typeof(AxisHandlerType), XmlHelper.GetValue(axisHandlerElement, "HandlerType", AxisHandlerType.None.ToString()));
            motionDoneCheckIntervalMs = Convert.ToInt32(XmlHelper.GetValue(axisHandlerElement, "MotionDoneCheckIntervalMs", "10"));

            if (axisList.Count == 0)
                return;

            int axisId = 0;

            foreach (XmlElement axisElement in axisHandlerElement)
            {
                if (axisElement.Name == "Axis")
                {
                    string axisName = XmlHelper.GetValue(axisElement, "AxisName", "");

                    axisList[axisId].AxisParam.Load(axisElement);
                    axisId++;
                }
            }
        }


        public void Save(XmlElement axisHandlerElement)
        {
            XmlHelper.SetValue(axisHandlerElement, "Name", name);
            XmlHelper.SetValue(axisHandlerElement, "HandlerType", this.handlerType.ToString());

            XmlHelper.SetValue(axisHandlerElement, "NumAxis", axisList.Count.ToString());
            XmlHelper.SetValue(axisHandlerElement, "MotionDoneCheckIntervalMs", motionDoneCheckIntervalMs.ToString());

            foreach (Axis axis in axisList)
            {
                XmlElement axisElement = axisHandlerElement.OwnerDocument.CreateElement("", "Axis", "");
                axisHandlerElement.AppendChild(axisElement);

                XmlHelper.SetValue(axisElement, "AxisName", axis.Name);
                XmlHelper.SetValue(axisElement, "Motion", axis.Motion.Name);
                XmlHelper.SetValue(axisElement, "AxisNo", axis.AxisNo.ToString());

                axis.AxisParam.Save(axisElement);
            }
        }

        public RectangleF GetWorkingRange()
        {
            Axis xAxis = GetAxis(AxisName.X.ToString());
            Axis yAxis = GetAxis(AxisName.Y.ToString());
            if (xAxis == null || yAxis == null)
            {
                return RectangleF.Empty;
            }
            return RectangleF.FromLTRB(xAxis.GetNegativeLimitPos(), yAxis.GetNegativeLimitPos(), xAxis.GetPositiveLimitPos(), yAxis.GetPositiveLimitPos());
        }

        private bool CanSyncMotion()
        {
            Motion motion = null;
            foreach (Axis axis in axisList)
            {
                if (motion == null)
                    motion = axis.Motion;
                else if (motion != axis.Motion)
                    return false;
            }

            return motion.CanSyncMotion();
        }

        public Axis GetAxis(int axisId)
        {
            return axisList[axisId];
        }

        public Axis GetAxisByNo(int axisNo)
        {
            foreach (Axis axis in axisList)
            {
                if (axis.AxisNo == axisNo)
                    return axis;
            }

            return null;
        }

        public Axis GetAxis(string name)
        {
            foreach (Axis axis in axisList)
            {
                if (axis.Name == name)
                    return axis;
            }

            return null;
        }

        public bool HomeMove(string name, CancellationTokenSource cancellationTokenSource)
        {
            foreach (Axis axis in axisList)
            {
                if (axis.Name == name)
                {
                    if (axis.HomeMove() == false)
                    {
                        StopMove();
                        return false;
                    }
                }
            }

            return true;
        }

        public bool HomeMove(int axisId)
        {
            if (axisList[axisId].HomeMove() == false)
            {
                StopMove();
                return false;
            }

            return true;
        }

        public Task StartHomeMove()
        {
            Task task = new Task(new Action(HomeMoveProc));
            task.Start();

            return task;
        }

        public void HomeMoveProc()
        {
            for (int i = 0; i <= GetMaxHomeOrder(); i++)
            {
                List<Axis> homeAxisList = GetAxisListByHomeOrder(i);
                if (homeAxisList.Count > 0)
                    HomeMove(homeAxisList);
            }
        }

        int GetMaxHomeOrder()
        {
            return axisList.Max(x => x.HomeOrder);
        }

        List<Axis> GetAxisListByHomeOrder(int homeOrder)
        {
            List<Axis> homeAxisList = new List<Axis>();

            foreach (Axis axis in axisList)
            {
                //if(axis.Motion.IsHomeDone(axis.AxisNo) == false)
                {
                    if (axis.HomeOrder == homeOrder)
                    {
                        homeAxisList.Add(axis);
                    }
                }
            }

            return homeAxisList;
        }

        public bool HomeMove(CancellationTokenSource cancellationTokenSource = null)
        {
            if (HomeMove(axisList, cancellationTokenSource) == false)
            {
                StopMove();
                return false;
            }

            return true;
        }

        bool HomeMove(List<Axis> homeMoveAxisList, CancellationTokenSource cancellationTokenSource = null)
        {
            bool isStopped = Array.TrueForAll(IsMoveOn(), f => f == false);
            if (isStopped == false)
                return false;

            if (onBeginMove != null)
                onBeginMove(null);

            bool waitHome = false;
            foreach (Axis axis in homeMoveAxisList)
            {
                //axis.Motion.ClearHomeDone(axis.AxisNo);

                waitHome = true;
                if (axis.StartHomeMove() == false)
                {
                    StopMove();
                    return false;
                }

                Thread.Sleep(50);
            }

            if (waitHome)
            // 모터가 모두 Home이 되어있었다면, 아래 함수에서 무한루프에 빠짐.
            {
                if (WaitHomeDone(homeMoveAxisList, cancellationTokenSource) == false)
                    return false;
            }
            Thread.Sleep(500);

            foreach (Axis axis in homeMoveAxisList)
            {
                if (axis.AxisParam.OriginPulse != 0)
                {
                    if (axis.Move(axis.AxisParam.OriginPulse) == false)
                        return false;
                }
            }

            ResetState();

            return true;
        }

        public void ResetAlarm()
        {
            foreach (Axis axis in axisList)
                axis.ResetAlarm();
        }

        public void ResetAlarmOn(bool isOn)
        {
            foreach (Axis axis in axisList)
                axis.ResetAlarmOn(isOn);
        }

        public void TurnOnServo(bool bOnOff)
        {
            foreach (Axis axis in axisList)
                axis.TurnOnServo(bOnOff);
        }

        public bool StartMove(string axisName, float position, MovingParam movingParam = null)
        {
            if (onBeginMove != null)
                onBeginMove(null);

            foreach (Axis axis in axisList)
            {
                if (axis.Name == axisName)
                {
                    if (axis.StartMove(position, movingParam) == false)
                    {
                        StopMove();
                        return false;
                    }
                }
            }

            return true;
        }

        public bool StartMove(int axisId, float position)
        {
            if (onBeginMove != null)
                onBeginMove(null);

            if (axisList[axisId].StartMove(position) == false)
            {
                StopMove();
                return false;
            }

            return true;
        }

        public bool StartMove(int[] axisId, AxisPosition position, MovingParam movingParam = null)
        {
            lastAxisPosition = position;

            position = RobotAligner.Align(position);

            if (onBeginMove != null)
                onBeginMove(position);

            foreach (int index in axisId)
            {
                if (axisList[index].StartMove(position[index], movingParam) == false)
                {
                    StopMove();
                    return false;
                }
            }

            return true;
        }

        public virtual bool StartMove(int axisId, AxisPosition position, MovingParam movingParam = null)
        {
            if (onBeginMove != null)
                onBeginMove(null);

            lastAxisPosition = position.Clone();

            position = RobotAligner.Align(position);

            string axisName = uniqueAxisList[axisId].Name;
            if (StartMove(axisName, position[axisId], movingParam) == false)
            {
                StopMove();
                return false;
            }

            return true;
        }

        public bool Move(int axisId, float position)
        {
            if (StartMove(axisId, position) == false)
                return false;

            return WaitMoveDone();
        }

        public bool Move(int axisId, float position, CancellationTokenSource cancellationTokenSource = null)
        {
            if (StartMove(axisId, position) == false)
                return false;

            return WaitMoveDone(cancellationTokenSource);
        }

        public bool Move(int axisId, AxisPosition axisPosition, CancellationTokenSource cancellationTokenSource = null)
        {
            if (StartMove(axisId, axisPosition) == false)
                return false;

            return WaitMoveDone(cancellationTokenSource);
        }

        public bool Move(string axisName, float position)
        {
            if (StartMove(axisName, position) == false)
                return false;

            return WaitMoveDone();
        }

        public bool Move(int[] axisIndex, AxisPosition position, MovingParam movingParam = null)
        {
            if (StartMove(axisIndex, position, movingParam) == false)
                return false;

            return WaitMoveDone();
        }

        public bool Move(AxisPosition axisPosition, MovingParam movingParam = null)
        {
            if (StartMove(axisPosition, movingParam) == false)
                return false;

            return WaitMoveDone();
        }

        public bool Move(AxisPosition axisPosition, CancellationTokenSource cancellationTokenSource)
        {
            if (StartMove(axisPosition) == false)
                return false;

            return WaitMoveDone(cancellationTokenSource);
        }

        public void ResetPosition()
        {
            AxisPosition axisPosition = CreatePosition();
            axisPosition.ResetPosition();

            SetPosition(new AxisPosition());
        }

        public bool StartRelativeMove(string name, float position, MovingParam movingParam = null)
        {
            if (onBeginMove != null)
                onBeginMove(null);

            foreach (Axis axis in axisList)
            {
                if (axis.Name == name)
                {
                    if (axis.StartRelativeMove(position, movingParam) == false)
                    {
                        StopMove();
                        return false;
                    }
                }
            }

            return true;
        }

        public bool RelativeMove(string name, float position)
        {
            if (StartRelativeMove(name, position) == false)
                return false;

            return WaitMoveDone();
        }

        public bool RelativeMove(AxisPosition axisPosition, MovingParam movingParam = null)
        {
            if (StartRelativeMove(axisPosition, movingParam) == false)
                return false;

            return WaitMoveDone();
        }

        public bool IsMoveDone()
        {
            foreach (Axis axis in axisList)
            {
                if (axis.IsMoveDone() == false)
                    return false;
            }

            return true;
        }

        public bool[] IsMoveOn()
        {
            bool[] moveOn = new bool[axisList.Count()];
            for (int i = 0; i < axisList.Count; i++)
                moveOn[i] = axisList[i].IsMoveOn();

            return moveOn;
        }

        public void ResetState()
        {
            foreach (Axis axis in axisList)
            {
                axis.ResetState();
            }
        }

        public bool IsMovingTimeOut()
        {
            foreach (Axis axis in axisList)
            {
                if (axis.IsMovingTimeOut() == true)
                    return true;
            }

            return false;
        }

        public bool CheckValidState()
        {
            foreach (Axis axis in axisList)
            {
                if (axis.CheckValidState() == false)
                    return false;
            }

            return true;
        }

        public bool WaitMoveDone(CancellationTokenSource cancellationTokenSource = null, int timeOutms = 300000)
        {
            TimeOutTimer timeOutTimer = new TimeOutTimer();
            timeOutTimer.Start(timeOutms);

            try
            {
                while (IsMoveDone() == false)
                {
                    Application.DoEvents();

                    if (CheckValidState() == false)
                    {
                        ResetState();
                        StopMove();
                        return false;
                    }

                    if (timeOutTimer.TimeOut)
                    {
                        ErrorManager.Instance().Report(ErrorCodeMotion.Instance.MovingTimeOut, ErrorLevel.Error,
                            this.name, "WaitMoveDone Timeout", null);

                        ResetState();
                        StopMove();
                        return false;
                    }

                    if (cancellationTokenSource != null)
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    Thread.Sleep(motionDoneCheckIntervalMs);
                }

                ResetState();

                if (onEndMove != null)
                    onEndMove(GetActualPos());
            }
            catch (OperationCanceledException)
            {
                ResetState();
                StopMove();
                return false;
            }

            return true;
        }

        public bool WaitHomeDone(CancellationTokenSource cancellationTokenSource = null)
        {
            return WaitHomeDone(axisList, cancellationTokenSource);
        }

        public bool WaitHomeDone(List<Axis> axisList, CancellationTokenSource cancellationTokenSource = null)
        {
            try
            {
                while (IsAllHomeDone() == false)
                {
                    if (CheckValidState() == false)
                    {
                        ResetState();
                        StopMove();
                        return false;
                    }

                    if (cancellationTokenSource != null)
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    Thread.Sleep(motionDoneCheckIntervalMs);
                }

                WaitMoveDone();
                //ResetState();

                if (onEndMove != null)
                    onEndMove(GetActualPos());
            }
            catch (OperationCanceledException)
            {
                ResetState();
                StopMove();

                return false;
            }

            return true;
        }

        // 동기 구동이 가능한 모션 보드의 경우, 동기 구동이 되는 함수로 호출을 해 주는 것이 좋다.
        public virtual bool StartMove(AxisPosition position, MovingParam movingParam = null)
        {
            lastAxisPosition = position.Clone();

            position = RobotAligner.Align(position);
            for (int index = 0; index < position.NumAxis && index < axisList.Count; index++)
            {
                string axisName = uniqueAxisList[index].Name;
                if (StartMove(axisName, position[index], movingParam) == false)
                {
                    StopMove();
                    return false;
                }
            }

            return true;
        }

        public virtual bool StartRelativeMove(AxisPosition position, MovingParam movingParam = null)
        {
            lastAxisPosition = position;

            for (int index = 0; index < position.NumAxis && index < axisList.Count; index++)
            {
                string axisName = uniqueAxisList[index].Name;
                if (StartRelativeMove(axisName, position[index], movingParam) == false)
                {
                    StopMove();
                    return false;
                }
            }

            return true;
        }

        public bool StartContinuousMove(bool negative = false)
        {
            if (onBeginMove != null)
                onBeginMove(null);

            foreach (Axis axis in axisList)
            {
                if (axis.ContinuousMove(null, negative) == false)
                    return false;
            }

            ResetState();

            return true;
        }

        public bool StartContinuousMove(int axisId, bool negative)
        {
            if (onBeginMove != null)
                onBeginMove(null);

            if (uniqueAxisList.Count <= axisId)
                return false;
            
            string uniqueAxisName = uniqueAxisList[axisId].Name;
            foreach (Axis axis in axisList)
            {
                if (axis.Name == uniqueAxisName)
                    if (axis.ContinuousMove(null, axis.AxisParam.Inverse ? !negative : negative) == false)
                        return false;
            }

            ResetState();

            return true;
        }

        public bool StartContinuousMove(int[] axisId, bool negative, MovingParam movingParam = null)
        {
            if (onBeginMove != null)
                onBeginMove(null);

            foreach (Axis axis in axisList)
            {
                for (int i = 0; i < axisId.Count(); i++)
                {
                    if (axis.AxisNo == axisId[i])
                        if (axis.ContinuousMove(null, negative) == false)
                            return false;
                }
            }

            ResetState();

            return true;
        }

        public bool StartContinuousMove(string name, bool negative)
        {
            if (onBeginMove != null)
                onBeginMove(null);

            foreach (Axis axis in axisList)
            {
                if (axis.Name == name)
                    if (axis.ContinuousMove(null, negative) == false)
                        return false;
            }

            ResetState();

            return true;
        }

        public bool ContinuousMove(MovingParam movingParam, bool negative = false)
        {
            foreach (Axis axis in axisList)
            {
                if (axis.ContinuousMove(movingParam, negative) == false)
                {
                    ErrorManager.Instance().Report(ErrorCodeMotion.Instance.Moving, ErrorLevel.Error,
                        this.name, "Handler {0}", new object[] { this.name });
                    return false;
                }
            }

            ResetState();

            return true;
        }

        public void StopMove()
        {
            foreach (Axis axis in axisList)
                axis.StopMove();
        }

        public void EmergencyStop()
        {
            foreach (Axis axis in axisList)
                axis.EmergencyStop();
        }

        public AxisPosition GetCommandPos()
        {
            List<float> commandPos = new List<float>();
            foreach (Axis axis in uniqueAxisList)
                commandPos.Add(axis.GetCommandPos());

            AxisPosition axisPosition = new AxisPosition();
            axisPosition.SetPosition(commandPos.ToArray());

            return RobotAligner.InverseAlign(axisPosition);
        }

        public AxisPosition GetActualPos()
        {
            List<float> actualPos = new List<float>();
            foreach (Axis axis in uniqueAxisList)
                actualPos.Add(axis.GetActualPos());

            AxisPosition axisPosition = new AxisPosition();
            axisPosition.SetPosition(actualPos.ToArray());

            return RobotAligner.InverseAlign(axisPosition);
        }

        public AxisPosition[] GetLimitPos()
        {
            List<float>[] limitPos = new List<float>[2] { new List<float>(), new List<float>() };
            foreach (Axis axis in uniqueAxisList)
            {
                limitPos[0].Add(axis.GetNegativeLimitPos());
                limitPos[1].Add(axis.GetPositiveLimitPos());
            }

            AxisPosition[] axisPositions = new AxisPosition[2];
            for (int i = 0; i < 2; i++)
            {
                AxisPosition axisPosition = new AxisPosition();
                axisPosition.SetPosition(limitPos[i].ToArray());
                axisPositions[i] = RobotAligner.InverseAlign(axisPosition);
            }

            return axisPositions;
        }

        public void SetPosition(string axisName, float position)
        {
            foreach (Axis axis in axisList)
            {
                if (axis.Name == axisName)
                    axis.SetPosition(position);
            }
        }

        public void SetPosition(AxisPosition axisPosition)
        {
            AxisPosition alignPosition = RobotAligner.Align(axisPosition);
            for (int index = 0; index < alignPosition.NumAxis && index < axisList.Count; index++)
            {
                string axisName = uniqueAxisList[index].Name;
                SetPosition(axisName, alignPosition[index]);
            }
        }

        public virtual bool IsOnError()
        {
            foreach (Axis axis in axisList)
            {
                if (axis.IsOnError() == true)
                    return true;
            }

            return false;
        }

        public bool[] IsAmpFault()
        {
            bool[] ampFault = new bool[axisList.Count()];
            foreach (Axis axis in axisList)
                ampFault[axis.AxisNo] = axis.IsAmpFault();

            return ampFault;
        }

        public bool[] IsServoOn()
        {
            bool[] servoOn = new bool[axisList.Count()];
            foreach (Axis axis in axisList)
                servoOn[axis.AxisNo] = axis.IsServoOn();

            return servoOn;
        }

        public bool IsAllServoOn()
        {
            foreach (Axis axis in axisList)
            {
                if (axis.IsServoOn() == false)
                    return false;
            }
            return true;
        }

        public bool[] IsHomeOn()
        {
            bool[] homeOn = new bool[axisList.Count()];
            foreach (Axis axis in axisList)
                homeOn[axis.AxisNo] = axis.IsHomeOn();

            return homeOn;
        }

        public bool IsAllHomeOn()
        {
            foreach (Axis axis in axisList)
            {
                if (axis.IsHomeOn() == false)
                    return false;
            }
            return true;
        }

        public bool[] IsPositiveOn()
        {
            bool[] positiveOn = new bool[axisList.Count()];
            foreach (Axis axis in axisList)
                positiveOn[axis.AxisNo] = axis.IsPositiveOn();

            return positiveOn;
        }

        public bool[] IsNegativeOn()
        {
            bool[] negativeOn = new bool[axisList.Count()];
            foreach (Axis axis in axisList)
                negativeOn[axis.AxisNo] = axis.IsNegativeOn();

            return negativeOn;
        }

        public bool IsPositiveLimit(AxisPosition axisPosition = null)
        {
            Dictionary<int, bool> positiveLimit = new Dictionary<int, bool>();
            foreach (Axis axis in axisList)
                positiveLimit.Add(axis.AxisNo, axis.IsPositiveLimit(axisPosition.Position[axis.AxisNo]));

            return positiveLimit.Values.Any(f => f == true);
        }

        public bool IsNegativeLimit(AxisPosition axisPosition = null)
        {
            Dictionary<int, bool> negativeLimit = new Dictionary<int, bool>();
            foreach (Axis axis in axisList)
                negativeLimit.Add(axis.AxisNo, axis.IsPositiveLimit(axisPosition.Position[axis.AxisNo]));

            return negativeLimit.Values.Any(f => f == true);
        }

        public bool IsLimit(AxisPosition axisPosition = null)
        {
            return IsPositiveLimit(axisPosition) || IsNegativeLimit(axisPosition);
        }

        public bool IsAllHomeDone()
        {
            foreach (Axis axis in axisList)
            {
                if (axis.Motion.IsHomeDone(axis.AxisNo) == false)
                    return false;
            }

            return true;
        }

        public bool StartMultipleHomeMove(CancellationTokenSource cancellationTokenSource)
        {
            bool result = true;
            foreach (Axis axis in axisList)
            {
                if (axis.StartHomeMove() == false)
                {
                    axis.StopMove();
                    result = false;
                }
            }

            return result;
        }

        // 한 축에 2개 이상의 모션이 붙어있는 경우, 함께 움직임.
        public bool StartMultipleMove(AxisPosition axisPosition, float targetVelocity = -1)
        {
            float minVelocity = axisList.Min(axis => axis.AxisParam.MovingParam.MaxVelocity);
            Axis minVelocityAxis = axisList.Find(axis => axis.AxisParam.MovingParam.MaxVelocity == minVelocity);
            MovingParam movingParam = new MovingParam(minVelocityAxis.AxisParam.MovingParam);

            if (targetVelocity > 0)
                movingParam.MaxVelocity = targetVelocity;

            return minVelocityAxis.StartMultiMove(axisList.ConvertAll(axis => axis.AxisNo).ToArray(), axisPosition.Position, movingParam); ;
        }

        public bool StartCmp(string axisName, int startPos, int dist, bool plus)
        {
            Axis founded = axisList.Find(axis => axis.Name == axisName);
            if (founded == null)
                return false;

            return founded.Motion.StartCmp(founded.AxisNo, startPos, dist, plus);
        }

        public bool EndCmp(string axisName)
        {
            Axis founded = axisList.Find(axis => axis.Name == axisName);
            if (founded == null)
                return false;

            return founded.Motion.EndCmp(founded.AxisNo);
        }
    }
}
