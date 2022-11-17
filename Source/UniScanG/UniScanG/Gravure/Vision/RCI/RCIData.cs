using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Gravure.Vision.RCI
{
    internal class RCIBlobRect
    {
        public BlobRect BlobRect { get; private set; }
        public float GreyMeanLT { get; set; }

        public RCIBlobRect(BlobRect blobRect)
        {
            this.BlobRect = blobRect;
            this.GreyMeanLT = -1;
        }
    }


    public class SpikeCollection
    {
        public bool IsValid => Spikes != null && Spikes.Length > 0;

        public string Name { get; private set; }

        public float[] Datas { get; private set; } = new float[0];

        public Tuple<float, bool>[] Spikes { get; private set; } = new Tuple<float, bool>[0];

        public float[] PositiveSpikes { get; private set; }
        public double PositiveSpikesAverageDist { get; private set; }

        public float[] NegativeSpikes { get; private set; }
        public double NegativeSpikesAverageDist { get; private set; }


        public SpikeCollection(string name)
        {
            this.Name = name;
        }

        public SpikeCollection(string name, float[] datas, Tuple<float, bool>[] positions)
        {
            this.Name = name;
            this.Datas = datas;
            this.Spikes = positions;
            Update();
        }

        public void Save(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement, null);
                return;
            }

            XmlHelper.SetValue(xmlElement, "Name", this.Name);
            XmlHelper.SetValue(xmlElement, "Datas", string.Join(",", this.Datas.Select(f => f.ToString()).ToArray()));
            XmlHelper.SetValue(xmlElement, "Spikes", string.Join(",", this.Spikes.Select(f => $"<{f.Item1} {f.Item2}>").ToArray()));
        }

        public static SpikeCollection Load(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return Load(xmlElement[key], null);
            }

            SpikeCollection collection = new SpikeCollection("");
            if (xmlElement == null)
                return collection;

            collection.Name = XmlHelper.GetValue(xmlElement, "Name", collection.Name);
            string[] dataTokens = XmlHelper.GetValue(xmlElement, "Datas", "").Split(',');
            collection.Datas = Array.FindAll(dataTokens, f => !string.IsNullOrEmpty(f)).Select(f => float.Parse(f)).ToArray();

            string[] spikeTokens = XmlHelper.GetValue(xmlElement, "Spikes", "").Split(',');
            collection.Spikes = Array.FindAll(spikeTokens, f => !string.IsNullOrEmpty(f)).Select(f =>
            {
                int begin = f.IndexOf('<');
                int space = f.IndexOf(' ');
                int end = f.IndexOf('>');
                float.TryParse(f.Substring(begin + 1, space - begin), out float item1);
                bool.TryParse(f.Substring(space + 1, end - space), out bool item2);
                return new Tuple<float, bool>(item1, item2);
            }).ToArray();

            collection.Update();

            return collection;
        }

        private void Update()
        {
            List<Tuple<float, bool>> list = this.Spikes.ToList();
            List<float> diffList = new List<float>();

            this.PositiveSpikes = list.FindAll(f => f.Item2).Select(f => f.Item1).ToArray();
            if (this.PositiveSpikes.Length > 0)
            {
                this.PositiveSpikes.Aggregate((f, g) =>
                {
                    diffList.Add(g - f);
                    return g;
                });
                this.PositiveSpikesAverageDist = diffList.Average();
            }
            diffList.Clear();

            this.NegativeSpikes = list.FindAll(f => !f.Item2).Select(f => f.Item1).ToArray();
            if (this.NegativeSpikes.Length > 0)
            {
                this.NegativeSpikes.Aggregate((f, g) =>
                {
                    diffList.Add(g - f);
                    return g;
                });

                this.NegativeSpikesAverageDist = diffList.Average();
            }
            diffList.Clear();
        }
        
        internal Tuple<int, int>[] GetRanges(bool splitBigRange, out int margin)
        //internal int[] GetRanges(bool splitBigRange, out int margin)
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>(this.NegativeSpikes.Length - 1);

            if (false)
            {
                int src = -1;
                this.Spikes.Aggregate((f, g) =>
                {
                    if (f.Item2 && !g.Item2)
                    {
                        int pos = (int)Math.Round((f.Item1 + g.Item1) / 2);
                        if (src >= 0 && pos > src)
                            list.Add(new Tuple<int, int>(src, pos));
                        src = pos;
                    }
                    return g;
                });
            }
            else if (true)
            {
                List<int> marginList = new List<int>();
                this.Spikes.Aggregate((f, g) =>
                {
                    if (f.Item2 && !g.Item2)
                        marginList.Add((int)Math.Round(g.Item1 - f.Item1));
                    return g;
                });

                margin = (int)Math.Round(marginList.Average());

                // falling Edte -> 전극 시작부분부터.
                this.NegativeSpikes.Aggregate((f, g) =>
                {
                    list.Add(new Tuple<int, int>((int)Math.Round(f), (int)Math.Round(g)));
                    return g;
                });

                if (this.Spikes.Last().Item2 == false)
                    list.Remove(list.Last());

                // 마지막 전극은 포함 안됨 -> 강제로 넣어줌.
                int lastPos = (int)Math.Round(this.PositiveSpikes.Last());
                while (list.Last().Item2 >= lastPos)
                    list.Remove(list.Last());

                list.Add(new Tuple<int, int>(list.Last().Item2, lastPos));

                if (splitBigRange)
                {
                    float mean = (float)list.Average(f => f.Item2 - f.Item1);
                    int idx;
                    while ((idx = list.FindIndex(f => (f.Item2 - f.Item1) > mean * 1.3f)) >= 0)
                    {
                        Tuple<int, int> tuple = list[idx];
                        int mid = (tuple.Item1 + tuple.Item2) / 2;
                        list.RemoveAt(idx);
                        list.Insert(idx, new Tuple<int, int>(mid, tuple.Item2));
                        list.Insert(idx, new Tuple<int, int>(tuple.Item1, mid));
                        list.Sort((f, g) => f.Item1.CompareTo(g.Item2));
                    }
                }
            }
            else
            {
                this.NegativeSpikes.Aggregate((f, g) =>
                {
                    list.Add(new Tuple<int, int>((int)Math.Round(f), (int)Math.Round(g)));
                    return g;
                });

                if (this.PositiveSpikes.Last() > list.Last().Item2)
                    list.Add(new Tuple<int, int>(list.Last().Item2, (int)Math.Round(this.PositiveSpikes.Last())));
            }
            Debug.Assert(list.TrueForAll(f => f.Item2 > f.Item1));

            //List<int> intList = new List<int>(list.Select(f => f.Item1));
            //intList.Add(list.Last().Item2);
            //return intList.ToArray();

            return list.ToArray();
        }
    }

    public class ProjectionData : IStoring
    {
        public Size PrjSize => new Size(PrjH.Length - Inflate.Width * 2, PrjV.Length - Inflate.Height * 2);

        public Size Inflate { get; private set; }

        public float[] PrjH { get; private set; }
        public float ScoreH { get; private set; }
        public bool CanPTMReferenceH => ScoreH >= 15;

        public float[] PrjV { get; private set; }
        public float ScoreV { get; private set; }
        public bool CanPTMReferenceV => ScoreV >= 15;

        public ProjectionData()
        {
            this.Inflate = Size.Empty;
            this.PrjH = new float[0];
            this.PrjV = new float[0];
            this.ScoreH = 0;
            this.ScoreV = 0;
        }

        public bool CanPTMReference(Direction direction)
        {
            if (direction == Direction.Horizontal)
                return CanPTMReferenceH;

            if (direction == Direction.Vertical)
                return CanPTMReferenceV;

            return false;
        }

        public ProjectionData(Size inflate, float[] prjH, float[] prjV)
        {
            this.Inflate = inflate;
            this.PrjH = prjH;
            this.PrjV = prjV;

            if (inflate.Width > 0)
                prjH = prjH.ToList().GetRange(inflate.Width, prjH.Length - 2 * inflate.Width).ToArray();
            this.ScoreH = GetScore(prjH);

            if (inflate.Height > 0)
                prjV = prjV.ToList().GetRange(inflate.Height, prjV.Length - 2 * inflate.Height).ToArray();
            this.ScoreV = GetScore(prjV);
        }

        private float GetScore(float[] prj)
        {
            float max = prj.Length == 0 ? 0 : prj.Max();
            float mean = prj.Length == 0 ? 0 : prj.Average();
            
            return MathHelper.StdDev(prj);
        }

        public static ProjectionData Build(AlgoImage algoImage, Size inflate)
        {
            float[] prjH, prjV;
            using (AlgoImage W = ImageBuilder.Build(algoImage.LibraryType, ImageType.Grey, algoImage.Width, 1))
            {
                using (AlgoImage WTemp = ImageBuilder.Build(algoImage.LibraryType, ImageType.Depth, algoImage.Width, 1))
                    RCIHelper.BuildSoblePrj(algoImage, W, WTemp, inflate, Direction.Horizontal);
                prjH = W.GetByte().Select(f => (float)f).ToArray();
            }

            using (AlgoImage H = ImageBuilder.Build(algoImage.LibraryType, ImageType.Grey, algoImage.Height, 1))
            {
                using (AlgoImage HTemp = ImageBuilder.Build(algoImage.LibraryType, ImageType.Depth, algoImage.Height, 1))
                    RCIHelper.BuildSoblePrj(algoImage, H, HTemp, inflate, Direction.Vertical);
                prjV = H.GetByte().Select(f => (float)f).ToArray();
            }
            return new ProjectionData(inflate, prjH, prjV);
        }

        public void Save(XmlElement xmlElement, string key)
        {
            XmlHelper.SetValue(xmlElement, "Inflate", this.Inflate);

            string prjH = string.Join(",", this.PrjH.Select(f => f.ToString()));
            XmlHelper.SetValue(xmlElement, "PrjH", prjH);

            string prjV = string.Join(",", this.PrjV.Select(f => f.ToString()));
            XmlHelper.SetValue(xmlElement, "PrjV", prjV);

            XmlHelper.SetValue(xmlElement, "ScoreH", this.ScoreH);
            XmlHelper.SetValue(xmlElement, "ScoreV", this.ScoreV);
        }

        public void Load(XmlElement xmlElement, string key)
        {
            this.Inflate = XmlHelper.GetValue(xmlElement, "Inflate", this.Inflate);

            string[] prjHTokens = XmlHelper.GetValue(xmlElement, "PrjH", "").Split(',');
            this.PrjH = Array.FindAll(prjHTokens, f => !string.IsNullOrEmpty(f)).Select(f => float.Parse(f)).ToArray();

            string[] prjVTokens = XmlHelper.GetValue(xmlElement, "PrjV", "").Split(',');
            this.PrjV = Array.FindAll(prjVTokens, f => !string.IsNullOrEmpty(f)).Select(f => float.Parse(f)).ToArray();

            this.ScoreH = XmlHelper.GetValue(xmlElement, "ScoreH", this.ScoreH);
            this.ScoreV = XmlHelper.GetValue(xmlElement, "ScoreV", this.ScoreV);
        }

        public object Clone()
        {
            ProjectionData projectionData = new ProjectionData(this.Inflate, (float[])this.PrjH.Clone(), (float[])this.PrjV.Clone());
            return projectionData;
        }

        //private static float[] GetSoblePrj(AlgoImage algoImage, Size inflate, Direction direction)
        //{
        //    ImageProcessing ip = Program.ImageProcessing;
        //    Rectangle prjRect = new Rectangle(Point.Empty, algoImage.Size);
        //    switch (direction)
        //    {
        //        case Direction.Horizontal:
        //            prjRect.Inflate(0, -inflate.Height);
        //            break;
        //        case Direction.Vertical:
        //            prjRect.Inflate(-inflate.Width, 0);
        //            break;
        //    }

        //    using (AlgoImage prjImage = algoImage.GetSubImage(prjRect))
        //    {
        //        float[] prj = ip.Projection(algoImage, direction);
        //        using (AlgoImage prjImg = ImageBuilder.Build(algoImage.LibraryType, ImageType.Depth, prj.Length, 1))
        //        {
        //            prjImg.SetByte(RCIHelper.GetBytes(prj));
        //            ip.Average(prjImg);
        //            ip.Sobel(prjImg, prjImg, Direction.Horizontal);
        //            float[] data = RCIHelper.GetSingles(prjImg.GetByte());
        //            return data;
        //        }
        //    }
        //}
    }

    public class WorkPoint: IStoring
    {
        public bool IsReference => IsReferenceX>=0 || IsReferenceY;

        public bool Use { get; set; } = true;
        public int IsReferenceX { get; set; } = -1;
        public bool IsReferenceY { get; set; } = false;
        public float MeanBgGv { get; set; }

        public int Index { get; private set; }
        public int Column { get; private set; }
        public int Row { get; private set; }
        public bool AnchorRight { get; private set; }
        public Point Point { get; private set; }
        public Size BlockSize { get; private set; }

        public ProjectionData Projection { get; private set; }

        public WorkPoint(int index, int column, int row, /*bool anchorRight,*/ Point point, Size blockSize)
        {
            this.Index = index;
            this.Column = column;
            this.Row = row;

            //this.AnchorRight = anchorRight;
            this.Point = point;
            this.BlockSize = blockSize;
        }

        public Rectangle GetPrjRectangle() => DrawingHelper.FromCenterSize(this.Point, this.Projection == null ? Size.Empty : this.Projection.PrjSize);
        public Rectangle GetPrjInflatedRectangle() => Rectangle.Inflate(GetPrjRectangle(), this.Projection == null ? 0 : this.Projection.Inflate.Width, this.Projection == null ? 0 : this.Projection.Inflate.Height);
        public Rectangle GetTeachRectangle() => new Rectangle(this.Point, this.BlockSize);

        public void UpdateProjection(AlgoImage algoImage, Size inflate)
        {
            if (algoImage == null)
                this.Projection = new ProjectionData(inflate, new float[0], new float[0]);
            else
                this.Projection = ProjectionData.Build(algoImage, inflate);
        }

        public object Clone()
        {
            WorkPoint workRect = new WorkPoint(this.Index, this.Column, this.Row, this.Point, this.BlockSize);
            workRect.Use = this.Use;
            workRect.IsReferenceX = this.IsReferenceX;
            workRect.IsReferenceY = this.IsReferenceY;
            workRect.MeanBgGv = this.MeanBgGv;
            workRect.Projection = (ProjectionData)this.Projection?.Clone();

            return workRect;
        }

        public void Save(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement, null);
                return;
            }

            XmlHelper.SetValue(xmlElement, "Use", this.Use);
            XmlHelper.SetValue(xmlElement, "IsReferenceX", this.IsReferenceX);
            XmlHelper.SetValue(xmlElement, "IsReferenceY", this.IsReferenceY);
            XmlHelper.SetValue(xmlElement, "MeanBgGv", this.MeanBgGv);
            XmlHelper.SetValue(xmlElement, "Index", this.Index);
            XmlHelper.SetValue(xmlElement, "Column", this.Column);
            XmlHelper.SetValue(xmlElement, "Row", this.Row);
            XmlHelper.SetValue(xmlElement, "Point", this.Point);
            XmlHelper.SetValue(xmlElement, "BlockSize", this.BlockSize);
            XmlHelper.SetValue(xmlElement, "Projection", this.Projection);
        }

        public void Load(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Load(xmlElement[key], null);
                return;
            }

            if (xmlElement == null)
                return;

            this.Use = XmlHelper.GetValue(xmlElement, "Use", this.Use);
            this.IsReferenceX = XmlHelper.GetValue(xmlElement, "IsReferenceX", this.IsReferenceX);
            this.IsReferenceY = XmlHelper.GetValue(xmlElement, "IsReferenceY", this.IsReferenceY);
            this.MeanBgGv = XmlHelper.GetValue(xmlElement, "MeanBgGv", this.MeanBgGv);
            this.Index = XmlHelper.GetValue(xmlElement, "Index", this.Index);
            this.Column = XmlHelper.GetValue(xmlElement, "Column", this.Column);
            this.Row = XmlHelper.GetValue(xmlElement, "Row", this.Row);
            this.Point = XmlHelper.GetValue(xmlElement, "Point", this.Point);
            this.BlockSize = XmlHelper.GetValue(xmlElement, "BlockSize", this.BlockSize);
            this.Projection = (ProjectionData)XmlHelper.GetValue(xmlElement, "Projection", (IStoring)new ProjectionData());
        }
    }
}
