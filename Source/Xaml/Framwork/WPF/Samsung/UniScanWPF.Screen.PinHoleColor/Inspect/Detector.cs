using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanWPF.Screen.PinHoleColor.Color.Inspect;
using UniScanWPF.Screen.PinHoleColor.PinHole.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.Inspect
{
    public abstract class Detector
    {
        public abstract Tuple<int, ConcurrentStack<AlgoImage>> GetBufferStack(ImageDevice targetDevice);
        public abstract DetectorResult Detect(AlgoImage targetImage, AlgoImage[] buffers, DetectorParam detectorParam);
        
        public static Detector Create(DetectorParam detectorParam)
        {
            if (detectorParam is PinHoleDetectorParam)
                return new PinHoleDetector();

            if (detectorParam is ColorDetectorParam)
                return new ColorDetector();

            return null;
        }
    }
}
