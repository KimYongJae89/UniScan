using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Operation.Operators;
using UniScanWPF.Table.Settings;

namespace UniScanWPF.Table.Operation
{
    public class ResultKey
    {
        DateTime dateTime;
        Model model;
        Production production;

        public DateTime DateTime { get => dateTime; }
        public Model Model { get => model; }
        public string LotNo { get => production?.LotNo; }
        public Production Production { get => production; }

        public ResultKey(DateTime dateTime, Model model,Production production)
        {
            this.dateTime = dateTime;
            this.model = model;
            this.production = production;
        }
    }

    public class ResultDictinary : Dictionary<ResultType, List<OperatorResult>>
    {
        public void AddResult(OperatorResult operatorResult)
        {
            lock (this)
            {
                if (this.ContainsKey(operatorResult.Type))
                    this[operatorResult.Type].Add(operatorResult);
                else
                    this.Add(operatorResult.Type, new List<OperatorResult>() { operatorResult });
            }
        }
    }

    public class ResultStorage : Dictionary<ResultKey, ResultDictinary>
    {
        ResultKey lastResultKey;

        public ResultKey LastResultKey { get => lastResultKey; }
        
        public void AddResult(OperatorResult operatorResult)
        {
            lock (this)
            {
                lastResultKey = operatorResult.ResultKey;

                if (!this.ContainsKey(operatorResult.ResultKey))
                    this.Add(operatorResult.ResultKey, new ResultDictinary());

                this[operatorResult.ResultKey].AddResult(operatorResult);
            }
        }
    }
}