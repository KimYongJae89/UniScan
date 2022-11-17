using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Inspect
{
    public delegate void AddDataDelegate(int index, int subIndex, int sheetNo);
    public delegate void ClearDelegate();

    public interface IInspectObserver
    {
        void AddData(int index,int subIndex, int sheetNo);
        void Clear();
    }
}
