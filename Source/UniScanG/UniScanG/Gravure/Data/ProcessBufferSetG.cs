using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Operation.Inspect
{
    public class ProcessBufferSetG : UniScanG.Operation.ProcessBufferSet
    {
        public ProcessBufferSetG(string algorithmTypeName, int width, int height) : base(algorithmTypeName, width, height)
        {
        }

        protected override void BuildBuffers(string algorithmTypeName, int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}
