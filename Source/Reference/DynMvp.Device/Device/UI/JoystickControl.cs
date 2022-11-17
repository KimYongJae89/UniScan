using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Devices.MotionController;

namespace DynMvp.Device.UI
{
    public interface IJoystickControl
    {
        void InitControl();
        void Initialize(AxisHandler axisHandler);
        void MoveAxis(int axisNo, int direction);
        void StopAxis();
    }
}
