using System.Collections.Generic;
using DynMvp.Vision;
using System.IO;
using DynMvp.Base;

namespace UniScanM.ColorSens.Algorithm
{
    class DebugImageBuffer
    {
        public int MaxImageCount = 10;
        List<AlgoImage> listDebugImage = new List<AlgoImage>();

        object lockobj = new object();
        public void AddImage(AlgoImage lastImage)
        {
            lock (lockobj)
            {
                listDebugImage.Add(lastImage);
                while (listDebugImage.Count > MaxImageCount)
                {
                    //listDebugImage[0].Dispose();
                    listDebugImage.RemoveAt(0);
                }
            }
        }

        public void SaveAllDebugImage(string path)
        {
            lock (lockobj)
            {
                foreach (var image in listDebugImage)
                {
                    int index = listDebugImage.IndexOf(image);
                    string filename = Path.Combine(path, string.Format("DebugImage_{0}.bmp", index));
                    image.Save(filename);
                }
            }
        }


        public void ClearAllDebugImage()
        {
            lock (lockobj)
            {
                listDebugImage.Clear(); 
            }
        }
    }//class DebugImageBuffer
}
