using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Vision;

namespace UniScanG.Gravure.Vision.Extender
{
    public class WatcherModelParam : AlgorithmModelParam
    {
        public ObserverChipCollection MonitorChipCollection => this.monitorChipCollection;
        public ObserverFPCollection MonitorFPCollection => this.monitorFPCollection;
        public ObserverIndexCollection MonitorIndexCollection => this.monitorIndexCollection;
        public StopImgCollection StopImgCollection => this.stopImgCollection;
        public TransformCollection TransformCollection => this.transformCollection;
        public MarginCollection MarginCollection => this.marginCollection;

        public ExtCollection[] Collections => (ExtCollection[])collections.Clone();
        ExtCollection[] collections;

        ObserverChipCollection monitorChipCollection;
        ObserverFPCollection monitorFPCollection;
        ObserverIndexCollection monitorIndexCollection;
        StopImgCollection stopImgCollection;
        TransformCollection transformCollection;
        MarginCollection marginCollection;

        public WatcherModelParam() : base()
        {
            this.collections = new ExtCollection[]
            {
                this.monitorChipCollection = new ObserverChipCollection(),
                this.monitorFPCollection = new ObserverFPCollection(),
                this.monitorIndexCollection = new ObserverIndexCollection(),

                this.stopImgCollection = new StopImgCollection(),
                this.transformCollection = new TransformCollection(),
                this.marginCollection = new MarginCollection()
            };
        }

        public WatcherModelParam(AlgorithmModelParam src) : base(src) { }

        public override void Clear()
        {
            this.monitorChipCollection?.Clear();
            this.monitorFPCollection?.Clear();
            this.monitorIndexCollection?.Clear();

            this.stopImgCollection?.Clear();
            this.transformCollection?.Clear();
            this.marginCollection?.Clear();
        }

        public override AlgorithmModelParam Clone()
        {
            return new WatcherModelParam(this);
        }

        public override void CopyFrom(AlgorithmModelParam algorithmModelParam)
        {
            WatcherModelParam watcherModelParam = (WatcherModelParam)algorithmModelParam;

            this.monitorChipCollection = (ObserverChipCollection)watcherModelParam.monitorChipCollection.Clone();
            this.monitorFPCollection = (ObserverFPCollection)watcherModelParam.monitorFPCollection.Clone();
            this.monitorIndexCollection = (ObserverIndexCollection)watcherModelParam.monitorIndexCollection.Clone();

            this.stopImgCollection = (StopImgCollection)watcherModelParam.stopImgCollection.Clone();
            this.transformCollection = (TransformCollection)watcherModelParam.transformCollection.Clone();
            this.marginCollection = (MarginCollection)watcherModelParam.marginCollection.Clone();
        }

        public override void Load(XmlElement xmlElement)
        {
            this.monitorChipCollection.Load(xmlElement, "MonitorChipCollection");
            this.monitorFPCollection.Load(xmlElement, "MonitorFPCollection");
            this.monitorIndexCollection.Load(xmlElement, "MonitorIndexCollection");

            this.stopImgCollection.Load(xmlElement, "StopImgCollection");
            this.transformCollection.Load(xmlElement, "TransformCollection");
            this.marginCollection.Load(xmlElement, "MarginCollection");
        }

        public override void Save(XmlElement xmlElement)
        {
            this.monitorChipCollection.Save(xmlElement, "MonitorChipCollection");
            this.monitorFPCollection.Save(xmlElement, "MonitorFPCollection");
            this.monitorIndexCollection.Save(xmlElement, "MonitorIndexCollection");

            this.stopImgCollection.Save(xmlElement, "StopImgCollection");
            this.transformCollection.Save(xmlElement, "TransformCollection");
            this.marginCollection.Save(xmlElement, "MarginCollection");
        }

        public List<ExtItem> GetItems()
        {
            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            List<ExtItem> itemList = new List<ExtItem>();
            itemList.AddRange(this.monitorChipCollection.Items);
            itemList.AddRange(this.monitorFPCollection.Items);
            itemList.AddRange(this.monitorIndexCollection.Items);

            itemList.AddRange(this.stopImgCollection.Items);
            itemList.AddRange(this.transformCollection.Items);
            itemList.AddRange(this.marginCollection.Items);
            return itemList;
        }

        public void SetItems(List<ExtItem> watchItemList)
        {
            Clear();
            watchItemList.ForEach(f => Array.Find(this.collections, g => g.ExtType == f.ExtType)?.Add(f));
            Array.ForEach(this.collections, f => f.UpdateIndex());
        }

        public Figure GetFigure(UniScanG.Data.OffsetStructSet offsetStructSet, Calibration calibration)
        {
            FigureGroup fg = new FigureGroup();
            //fg.AddFigure(this.stopImgCollection.GetFigure());
            //fg.AddFigure(this.stopImgCollection.GetFigure());
            //fg.AddFigure(this.transformCollection.GetFigure());
            //fg.AddFigure(this.marginCollection.GetFigure());
            Figure[] figures = this.collections.Select(f => f.GetFigure(offsetStructSet, calibration)).ToArray();
            fg.AddFigure(figures);
            return fg;
        }
    }

    public class WatcherParam : UniScanG.Vision.AlgorithmParamG
    {
        public WatcherModelParam ModelParam => this.modelParam == null ? SystemManager.Instance().CurrentModel.WatcherModelParam : this.modelParam;
        WatcherModelParam modelParam;

        public int MonitoringPeriod { get => this.monitoringPeriod; set => this.monitoringPeriod = value; }
        int monitoringPeriod;

        public WatcherParam(bool iscludeModelParam) : base()
        {
            if (iscludeModelParam)
                this.modelParam = new WatcherModelParam();

            this.monitoringPeriod = 5;
        }

        #region override
        public override AlgorithmParam Clone()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
        #endregion

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "MonitoringPeriod", this.monitoringPeriod);
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            this.monitoringPeriod = XmlHelper.GetValue(algorithmElement, "MonitoringPeriod", this.monitoringPeriod);
        }
    }
}
