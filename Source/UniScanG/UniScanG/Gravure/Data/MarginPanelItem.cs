using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.Data
{
    public class MarginPanelItem
    {
        public class _Item : INotifyPropertyChanged, IComparable
        {
            public int PatternNo => this.patternNo + 1;
            int patternNo;

            public SizeF[] MarginSizesUm => this.marginSizesUm;
            SizeF[] marginSizesUm;

            Size count;

            public SizeF Min => this.min;
            SizeF min;
            public SizeF Max => this.max;
            SizeF max;

            public SizeF Mean => this.mean;
            SizeF mean;

            public event PropertyChangedEventHandler PropertyChanged;

            // CM
            public SizeF MarginSizeUm0 => this.marginSizesUm[0];
            //LT
            public SizeF MarginSizeUm1 => this.marginSizesUm[1];
            //RT
            public SizeF MarginSizeUm2 => this.marginSizesUm[2];
            //LB
            public SizeF MarginSizeUm3 => this.marginSizesUm[3];
            //RB
            public SizeF MarginSizeUm4 => this.marginSizesUm[4];

            public bool IsInvalid => this.patternNo < 0;

            public _Item()
            {
                this.patternNo = -1;
                this.marginSizesUm = new SizeF[5];
            }

            public void Update(int patternNo, List<Data.MarginObj> aa)
            {
                this.patternNo = patternNo;
                //OnPropertyChanged("PatternNo");

                aa.ForEach(f =>
                {
                    int itemIndex = f.GetDisplayIndex();
                    if (itemIndex < this.marginSizesUm.Length)
                    {
                        this.marginSizesUm[itemIndex] = f.MarginSizeUm;
                        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MarginSizeUm"));
                    }
                });

                Update();

            }

            private void Update()
            {
                this.count.Width = this.marginSizesUm.Count(f => f.Width > 0);
                this.count.Height = this.marginSizesUm.Count(f => f.Height > 0);

                this.mean.Width = this.count.Width == 0 ? 0 : this.marginSizesUm.Sum(f => f.Width) / this.count.Width;
                this.mean.Height = this.count.Height == 0 ? 0 : this.marginSizesUm.Sum(f => f.Height) / this.count.Height;

                List<float> widthList = this.marginSizesUm.Select(f => f.Width).ToList();
                widthList.RemoveAll(f => f == 0);
                this.min.Width = widthList.Count == 0 ? 0 : widthList.Min();
                this.max.Width = widthList.Count == 0 ? 0 : widthList.Max();

                List<float> heightList = this.marginSizesUm.Select(f => f.Height).ToList();
                heightList.RemoveAll(f => f == 0);
                this.min.Height = heightList.Count == 0 ? 0 : heightList.Min();
                this.max.Height = heightList.Count == 0 ? 0 : heightList.Max();
            }

            public void Clear()
            {
                this.patternNo = -1;
                Array.Clear(this.marginSizesUm, 0, this.marginSizesUm.Length);
                Update();
            }

            protected void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    var e = new PropertyChangedEventArgs(propertyName);
                    foreach (PropertyChangedEventHandler h in handler.GetInvocationList())
                    {
                        var synch = h.Target as ISynchronizeInvoke;
                        if (synch != null && synch.InvokeRequired)
                            synch.Invoke(h, new object[] { this, e });
                        else
                            h(this, e);
                    }
                }
            }

            public int CompareTo(object obj)
            {
                _Item item = obj as _Item;
                if (obj == null)
                    return 0;
                return this.patternNo.CompareTo(item.patternNo);
            }
        }
        _Item[] item = null;

        public bool ClearAlarm { get => this.clearAlarm; set => this.clearAlarm = value; }
        bool clearAlarm = false;

        public _Item First => item[0];
        public _Item Last => item[1];

        public MarginPanelItem()
        {
            this.item = new _Item[2] { new _Item(), new _Item() };
        }

        public void Clear()
        {
            Array.ForEach(this.item, f => f.Clear());
            this.clearAlarm = false;
        }

        public void Update(int patternNo, List<Data.MarginObj> aa)
        {
            _Item item = this.item[1];
            if (this.item[0].IsInvalid)
                item = this.item[0];

            item.Update(patternNo, aa);
        }

        public bool IsAlarm(AbsoluteAlarmSetting marginLengthAlarm)
        {
            if (Array.Exists(item, f => f == null))
                return false;

            if (item[1].PatternNo - item[0].PatternNo < marginLengthAlarm.Count)
                return false;

            float maxMargin = 0;
            for (int i = 0; i < 5; i++)
            {
                SizeF marginDiff = DrawingHelper.Subtract(item[1].MarginSizesUm[i], item[0].MarginSizesUm[i]);
                float maxMarginDiff = Math.Max(Math.Abs(marginDiff.Width), Math.Abs(marginDiff.Height));
                maxMargin = Math.Max(maxMargin, maxMarginDiff);
            }

            return (maxMargin >= marginLengthAlarm.Value) && !this.clearAlarm;
        }
    }
}
