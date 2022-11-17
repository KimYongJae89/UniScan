using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GpuTransfer.Form1;

namespace GpuTransfer
{
    class TransferTest
    {
        private AppendLogDelegate AppendLog;

        public TransferTest(AppendLogDelegate appendLog)
        {
            this.AppendLog = appendLog;
        }

        public async Task Test(AlgoImage[] algoImages, Param param)
        {
            await Task.Run(() =>
            {
                using (Image2D image2D = new Image2D(param.Width, param.Heigth, 1))
                {
                    AppendLog?.Invoke($"== UPLOAD ==");
                    for (int i = 0; i < algoImages.Length; i++)
                    {
                        Stopwatch sw = Stopwatch.StartNew();
                        image2D.ConvertFromData();

                        if (param.UseIntPtr)
                            algoImages[i].PutByte(image2D.DataPtr, image2D.Pitch);
                        else
                            algoImages[i].PutByte(image2D.Data);

                        sw.Stop();
                        AppendLog?.Invoke($"{i:00}: Transfer End.", sw.ElapsedMilliseconds);
                    }
                }
            });
        }
    }
}
