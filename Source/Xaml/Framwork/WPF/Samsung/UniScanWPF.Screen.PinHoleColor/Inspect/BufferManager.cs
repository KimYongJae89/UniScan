using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UniScanWPF.Screen.PinHoleColor.Inspect
{
    public interface IBufferViewer
    {
        void UpdateBuffers(List<InspectSet> inspectSetList);
    }

    public class BufferManager
    {
        IBufferViewer bufferViewer = null;
        
        List<InspectSet> inspectSetList = new List<InspectSet>();

        static BufferManager instance;
        public static BufferManager Instance()
        {
            if (instance == null)
                instance = new BufferManager();

            return instance;
        }

        private BufferManager() { }

        public void Connect()
        {
            if (bufferViewer != null)
            {
                bufferViewer.UpdateBuffers(this.inspectSetList);
            }
        }

        public void SetBufferViewer(IBufferViewer bufferViewer)
        {
            this.bufferViewer = bufferViewer;
        }

        public void AddInspectSet(InspectSet inspectSet)
        {
            inspectSetList.Add(inspectSet);
        }

        public void Clear()
        {
            inspectSetList.Clear();
        }
    }
}
