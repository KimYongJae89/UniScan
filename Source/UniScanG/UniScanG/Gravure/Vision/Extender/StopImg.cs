using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Data;

namespace UniScanG.Gravure.Vision.Extender
{
    public class StopImg : ClipAndSave
    {
        public StopImg() : base(ExtType.StopIMG, DynMvp.Base.LicenseManager.ELicenses.ExtStopImg) { }

        protected override ExtItem CloneItem()
        {
            return new StopImg();
        }
    }

    public class StopImgCollection : ClipAndSaveCollection
    {
        public StopImgCollection() : base(ExtType.StopIMG, false, DynMvp.Base.LicenseManager.ELicenses.ExtStopImg) { }

        public override ExtItem CreateItem()
        {
            return new StopImg();
        }
        
        public override void Train(ExtCollectionTrainParam trainParam, Action ProgressUpdated, DebugContext debugContext)
        {
            List<RegionInfoG> useRegionInfoList = trainParam.RegionInfoList.FindAll(f => f.Use);
            int count = this.Param.Active ? this.Param.Count : 0;

            Clear();
            for (int i = 0; i < Math.Min(useRegionInfoList.Count, count); i++)
            {
                RegionInfoG regionInfoG = useRegionInfoList[i];
                DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("MonitoringItems_{0}", i)));

                ExtItem watchItem = CreateItem();
                if (watchItem.BuildItem(trainParam.TrainImage, regionInfoG, new PointF(0.3f, 0.3f), trainParam.Calibration, newDebugContext))
                {
                    watchItem.Index = i;
                    watchItem.Name = string.Format("PT.{0}", i);
                    watchItem.ContainerIndex = trainParam.RegionInfoList.IndexOf(regionInfoG);
                    Add(watchItem);
                }

                ProgressUpdated?.Invoke();
            }

            RemoveAll(f => f == null);
        }

        public override ExtCollection Clone()
        {
            StopImgCollection stopImgCollection = new StopImgCollection();
            stopImgCollection.param = this.param.Clone();
            this.items.ForEach(f => stopImgCollection.Add(f.Clone()));
            return stopImgCollection;
        }
    }

}
