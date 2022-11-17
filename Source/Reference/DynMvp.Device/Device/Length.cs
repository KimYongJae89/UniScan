using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Device
{
    public enum LengthUnit
    {
        um, mm, mil, inch, pixel
    }

    public class Length
    {
        static LengthUnit defaultLengthUnit;
        public static LengthUnit DefaultLengthUnit
        {
            get { return defaultLengthUnit; }
            set { defaultLengthUnit = value; }
        }

        static float pixelSize;
        public static float PixelSize
        {
            get { return pixelSize; }
            set { pixelSize = value; }
        }

        float value;

        LengthUnit unit;
        public LengthUnit Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        private Length()
        {

        }

        public float GetExternalValue()
        {
            return value;
        }

        public float GetInternalValue()
        {
            return GetInternalValue(value, unit);
        }

        public static float GetInternalValue(float value)
        {
            return GetInternalValue(value, defaultLengthUnit);
        }

        public static float GetInternalValue(float value, LengthUnit lengthUnitType)
        {
            float convertedValue = value;
            switch (lengthUnitType)
            {
                default:
                case LengthUnit.um:
                    return value;
                case LengthUnit.mm:
                    return value * 1000;
                case LengthUnit.inch:
                    return (value * 10000) * 2.54f;
                case LengthUnit.mil:
                    return (value * 10) * 2.54f;
                case LengthUnit.pixel:
                    return value * pixelSize;
            }
        }

        public static float GetExternalValue(float value)
        {
            return GetExternalValue(value, defaultLengthUnit);
        }

        public static float GetExternalValue(float value, LengthUnit lengthUnitType)
        {
            float convertedValue = value;
            switch (lengthUnitType)
            {
                default:
                case LengthUnit.um:
                    return value;
                case LengthUnit.mm:
                    return value / 1000;
                case LengthUnit.inch:
                    return (value / 10000) / 2.54f;
                case LengthUnit.mil:
                    return (value / 10) / 2.54f;
                case LengthUnit.pixel:
                    return value / pixelSize;
            }
        }
    }
}
