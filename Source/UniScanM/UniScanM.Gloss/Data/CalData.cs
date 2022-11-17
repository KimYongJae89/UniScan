using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.Data
{
    public static class CalData
    {
        #region 메서드
        public static AxisPosition GetOffsetMoveAxisPosition(float position)
        {
            return new AxisPosition(GetOffsetMovePosition(position));
        }

        public static float GetOffsetMovePosition(float position)
        {
            var glossSettings = GlossSettings.Instance();
            float positionOffset = glossSettings.PositionOffset;
            float newPosition;
            if (glossSettings.RevesePosition)
            {
                newPosition = (positionOffset - position) * 1000f;
            }
            else
            {
                newPosition = (positionOffset + position) * 1000f;
            }

            return newPosition;
        }

        public static float GetOffsetPositionFromActualPosition(float actualPosition)
        {
            var glossSettings = GlossSettings.Instance();
            float positionOffset = glossSettings.PositionOffset;
            float newPosition;
            if (glossSettings.RevesePosition)
            {
                newPosition = positionOffset - actualPosition;
            }
            else
            {
                newPosition = actualPosition - positionOffset;
            }

            return newPosition;
        }
        #endregion
    }
}
