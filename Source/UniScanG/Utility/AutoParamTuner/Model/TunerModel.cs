using AutoParamTuner.Base;
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data.Inspect;
using UniScanG.Data.Model;

namespace AutoParamTuner.Model
{

    internal class TunerModel : AutoParamTuner.Base.Model
    {
        public UniScanG.Data.Model.Model ModelG { get; set; }
        public Dictionary<ISwapParamItem, TunerResult> ResultDictionary { get; set; }

        public string ModelPath { get; set; }
        public Image2D Image { get; set; }

        public List<SwapParamItem> ParamItemList { get; set; }

        public TunerModel()
        {
            this.ParamItemList = new List<SwapParamItem>
            {
                new SwapParamItem(ParamName.SensitivityMax, 20),
                new SwapParamItem(ParamName.SensitivityMax, 30),
            };
        }

        public override void Save(string xmlFile)
        {
            throw new NotImplementedException();
        }

        public override void Load(string xmlFile)
        {
            throw new NotImplementedException();
        }
    }

    internal interface ISwapParamItem
    {
        bool Use { get; set; }
        ParamName Name { get; set; }
        string Unit { get; }

        object Min { get; set; }
        object Max { get; set; }

        object Value { get; set; }

        void Apply(UniScanG.Data.Model.Model model);
    }

    internal class SwapParamItem : ISwapParamItem
    {
        Type type;

        public bool Use { get; set; } = true;
        public string Unit { get; private set; }

        public ParamName Name { get => this.name; set => this.name = UpdateName(value); }
        ParamName name;

        public object Min { get => (object)this.min; set => min = Convert.ChangeType(value, this.type); }
        object min;

        public object Max { get => (object)this.max; set => max = Convert.ChangeType(value, this.type); }
        object max;

        public object Value { get => (object)this.value; set => this.value = Convert.ChangeType(value, this.type); }
        object value;

        private static string GetPath(ParamName name)
        {
            switch (name)
            {
                case ParamName.SensitivityMax:
                    return "CalculatorModelParam.SensitiveParam.Max";
            }
            throw new NotImplementedException();
        }

        public SwapParamItem()
        {
            UpdateName(ParamName.SensitivityMax);
            this.value = this.min;
        }

        public SwapParamItem(ParamName name, object value)
        {
            UpdateName(name);
            this.value = value;
        }

        private ParamName UpdateName(ParamName value)
        {
            switch (name)
            {
                case ParamName.SensitivityMax:
                    this.type = typeof(byte);
                    this.name = value;
                    this.min = byte.MinValue;
                    this.max = byte.MaxValue;
                    this.Unit = "DN";
                    break;

                default:
                    throw new NotImplementedException();
            }
            return value;

            throw new NotImplementedException();
        }


        public void Apply(UniScanG.Data.Model.Model model)
        {
            string path = GetPath(this.Name);
            string[] tokens = path.Split('.');
            System.Collections.IEnumerator enumerator = tokens.GetEnumerator();

            object obj = null;
            PropertyInfo pInfo = null;
            while (enumerator.MoveNext())
            {
                obj = (pInfo?.GetValue(obj)) ?? model;
                pInfo = obj.GetType().GetProperty(enumerator.Current.ToString());
            }

            object bb = Convert.ChangeType(value, this.type);
            pInfo.SetValue(obj, bb);
        }
    }
}
