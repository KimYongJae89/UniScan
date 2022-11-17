using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.Forms
{
    public interface IAlgorithmParamControl
    {
        string GetTypeName();

        void ClearSelectedProbe();
        void AddSelectedProbe(Probe probe);
        void UpdateProbeImage();
        void PointSelected(Point clickPos, ref bool processingCancelled);

        void SetValueChanged(AlgorithmValueChangedDelegate valueChanged);
        void SetTargetGroupImage(ImageD image);
    }

    public enum ValueChangedType
    {
        None, Position, ImageProcessing, Light
    }

    public delegate void ValueChangedDelegate(ValueChangedType valueChangedType, bool modified);
    public delegate void AlgorithmValueChangedDelegate(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam, bool modified);
    public delegate void FiducialChangedDelegate(bool useFiducialProbe);
}
