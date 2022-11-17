using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.Data.UI;
using DynMvp.InspData;

namespace DynMvp.Data.UI
{
    public partial class TargetView : DrawBox
    {
        private InspectionResult targetInspectionResult = new InspectionResult();

        private Target linkedTarget = null;
        public Target LinkedTarget
        {
          get { return linkedTarget; }
          set { linkedTarget = value; }
        }

        private int targetIndex;
        public int TargetIndex
        {
            get { return targetIndex; }
            set { targetIndex = value; }
        }

        public TargetView()
        {
            InitializeComponent();
        }

        public void SetTarget(Target target, bool fUpdate = true)
        {
            linkedTarget = target;
            if (Image != null)
                Image.Dispose();

            UpdateImage(linkedTarget.Image.ToBitmap());

            FigureGroup figureGroup = new FigureGroup();
            target.AppendFigures(figureGroup, null, true);

            figureGroup.SetSelectable(false);

            figureGroup.Offset(-target.BaseRegion.Left, -target.BaseRegion.Top);

            FigureGroup = figureGroup;

            if (fUpdate)
                Invalidate();
        }

        public void UpdteTargetView(InspectionResult targetInspectionResult)
        {
            LogHelper.Debug(LoggerType.Grab, "Start UpdteTargetView");

            if (this.Image != null)
                this.Image.Dispose();

            Image2D targetImage = targetInspectionResult.GetTargetImage(linkedTarget.FullId);
            if (targetImage != null)
                this.UpdateImage(targetImage.ToBitmap());
            else
                this.UpdateImage(linkedTarget.Image.ToBitmap());

            targetInspectionResult.AppendResultFigures(TempFigureGroup);

            LogHelper.Debug(LoggerType.Grab, "End UpdteTargetView");

            Invalidate();
        }

        public void UpdateTargetView(ProbeResult probeResult, Image2D targetImage, bool useWholeImage = false)
        {
            LogHelper.Debug(LoggerType.Grab, "Start UpdteTargetView");

            if (this.Image != null)
                this.Image.Dispose();

            if (targetImage != null)
                this.UpdateImage(targetImage.ToBitmap());
            else
                this.UpdateImage(Properties.Resources.Image);

            TempFigureGroup.Clear();

            probeResult.AppendResultFigures(TempFigureGroup);

            if (probeResult.Probe != null)
            {
                linkedTarget = probeResult.Probe.Target;
                if (useWholeImage == false)
                    TempFigureGroup.Offset(-linkedTarget.BaseRegion.X, -linkedTarget.BaseRegion.Y);
            }

            LogHelper.Debug(LoggerType.Grab, "End UpdteTargetView");

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Call base class, invoke Paint handlers
            base.OnPaint(e);

            if (linkedTarget != null)
            {
                Font font = new Font("Arial", 10);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;

                Brush fontBrush = new SolidBrush(Color.Green);

                e.Graphics.DrawString(linkedTarget.Name, font, fontBrush, (float)5, (float)5, stringFormat);
            }
        }
    }
}
