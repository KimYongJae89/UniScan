using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanM.Operation
{
    public class OperationOption
    {
        bool onTune = false;
        public bool OnTune { get => onTune; set => onTune = value; }

        static OperationOption _instance;
        public static OperationOption Instance()
        {
            if (_instance == null)
            {
                _instance = new OperationOption();
            }

            return _instance;
        }

        protected OperationOption()
        {

        }

        public void Save()
        {
         
        }

        public void Load()
        {
           
        }
    }
}
