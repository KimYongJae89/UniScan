using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UniScanWPF.Table.Device
{
    public class MotionBox : INotifyPropertyChanged
    {
        AxisHandler handler;

        float actualPositionX;
        float actualPositionY;
        bool onMove;

        SolidColorBrush greyBrush = new SolidColorBrush(Colors.LightGray);
        SolidColorBrush greenBrush = new SolidColorBrush(Colors.LightGreen);
        SolidColorBrush redBrush = new SolidColorBrush(Colors.Crimson);

        SolidColorBrush onMoveBrush;

        AxisPosition[] limitPosition;

        public float ActualPositionX { get => actualPositionX; }

        public float ActualPositionY { get => actualPositionY; }

        public float[] CamTopLeftPosition
        {
            get
            {
                if (limitPosition == null)
                    return null;

                return new float[] { (limitPosition[1].Position[0] + limitPosition[0].Position[0] - actualPositionX) - 10000.0f, actualPositionY + 10000 };
            }
        }

        public bool OnMove
        {
            get => onMove;
            set
            {
                if (onMove != value)
                {
                    onMove = value;
                    onMoveBrush = onMove == true ? greenBrush : greyBrush;
                }
            }
        }
        
        public SolidColorBrush OnMoveBrush { get => onMoveBrush; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MotionBox()
        {
            handler = SystemManager.Instance().DeviceController.RobotStage;
            if (handler != null)
            {
                limitPosition = handler?.GetLimitPos();
            }
            else
            {
                limitPosition = new AxisPosition[2];
                limitPosition[0] = new AxisPosition(2);
                limitPosition[1] = new AxisPosition(2);
            }
            onMoveBrush = greyBrush;
        }

        public void Read()
        {
            if (handler != null)
            {
                bool onMove = false;
                foreach (bool move in handler.IsMoveOn())
                {
                    if (move)
                    {
                        onMove = true;
                        break;
                    }
                }

                this.OnMove = onMove;
                float[] pos = handler.GetActualPos().Position;
                if (pos != null)
                {
                    actualPositionX = pos[0];
                    actualPositionY = pos[1];
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
