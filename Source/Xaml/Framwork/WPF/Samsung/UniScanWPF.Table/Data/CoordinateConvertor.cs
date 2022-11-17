using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanWPF.Table.Data
{
    public static class CoordinateConvertor
    {
        public static AxisPosition Convert2Sheet(AxisPosition axisPositions) 
        {
            float xPos = axisPositions.Position[0];
            float yPos = axisPositions.Position[1];

            AxisPosition[] limitPos = SystemManager.Instance().DeviceController.RobotStage.GetLimitPos();
            RectangleF working = SystemManager.Instance().DeviceController.RobotStage.GetWorkingRange();

            AxisPosition newAxisPosition = new AxisPosition((limitPos[1].Position[0] + limitPos[0].Position[0] - xPos), yPos);
            return newAxisPosition;
        }
        
    }
}
