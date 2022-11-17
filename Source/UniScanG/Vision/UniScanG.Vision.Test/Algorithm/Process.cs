using DynMvp.Base;
using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Vision.Test.Algorithm
{
    abstract class Process : IDisposable
    {
        public Process()
        {
            MatroxHelper.InitApplication(false, false);
        }

        public static Process CreateProcess(AlgoLibrary algoLibrary)
        {
            Process process = null;
            switch (algoLibrary)
            {
                case AlgoLibrary.MIL:
                    process = new MilProcess();
                    break;
                case AlgoLibrary.SIMD:
                    process = new SIMDProcess();
                    break;
            }
            return process;
        }

        public Image2D DoProcess(int v, Class1 myClass,bool parallel, DebugContext debugContext)
        {
            switch (v)
            {
                case 1:
                    return this.Process1(myClass.Image2D.DataPtr, myClass.Image2D.Size, myClass.Image2D.Pitch, parallel, debugContext);
                    break;
                case 2:
                    return this.Process2(myClass.Image2D.DataPtr, myClass.Image2D.Size, myClass.Image2D.Pitch, parallel, debugContext);
                    break;
                case 3:
                    return this.Process3(myClass.Image2D.DataPtr, myClass.Image2D.Size, myClass.Image2D.Pitch, parallel, debugContext);
                    break;
            }
            throw new NotImplementedException();
        }

        public Tuple<Size, Rectangle[]> GetRoi(Size size)
        {
            int count = 100;
            Size subSize = new Size(size.Width / count, size.Height);
            Rectangle[] rectangles = new Rectangle[count];
            for (int i = 0; i < count; i++)
                rectangles[i] = new Rectangle(new Point(i * subSize.Width, 0), subSize);
            return new Tuple<Size, Rectangle[]>(subSize, rectangles);
        }

        public virtual void Dispose()
        {
            MatroxHelper.FreeApplication();
        }
        public abstract Image2D Process1(IntPtr intPtr, Size size, int pitch, bool parallel, DebugContext debugContext);
        public abstract Image2D Process2(IntPtr intPtr, Size size, int pitch, bool parallel, DebugContext debugContext);
        public abstract Image2D Process3(IntPtr intPtr, Size size, int pitch, bool parallel, DebugContext debugContext);
    }
}
