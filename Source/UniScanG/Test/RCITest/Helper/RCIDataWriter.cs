using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCITest.Helper
{
    struct RCIDataPoint
    {
        public IComparable X { get; private set; }
        public IComparable Y { get; private set; }
        public object D { get; private set; }

        public RCIDataPoint(IComparable x, IComparable y, object d)
        {
            this.X = x;
            this.Y = y;
            this.D = d;
        }
    }
    class RCIDataWriter
    {
        public IWin32Window Parent { get; private set; }

        string header = "";
        List<RCIDataPoint> list = new List<RCIDataPoint>();

        public RCIDataWriter(IWin32Window parent, string header = "")
        {
            this.Parent = parent;
            this.header = header;
        }

        public void Clear()
        {
            list.Clear();
        }

        public void Add(IComparable x, IComparable y, object d)
        {
            if (list.Exists(f => f.X == x && f.Y == y))
                throw new Exception("Duplicated.");

            list.Add(new RCIDataPoint(x, y, d));
        }

        public override string ToString()
        {
            var groupX = list.GroupBy(f => f.X).OrderBy(f => f.Key);
            var groupY = list.GroupBy(f => f.Y).OrderBy(f => f.Key);
            SortedList<IComparable, SortedList<IComparable, object>> table = new SortedList<IComparable, SortedList<IComparable, object>>();

            list.ForEach(f =>
            {
                if (!table.ContainsKey(f.Y))
                    table.Add(f.Y, new SortedList<IComparable, object>());

                if (!table[f.Y].ContainsKey(f.X))
                    table[f.Y].Add(f.X, null);
                table[f.Y][f.X] = f.D;
            });

            StringBuilder sb = new StringBuilder();
            sb.Append($"{this.header},");
            sb.AppendLine(string.Join(",", groupX.Select(f => f.Key.ToString())));
            foreach (var y in table)
            {
                sb.Append($"{y.Key.ToString()},");
                foreach (var x in groupX)
                {
                    var vv = y.Value.FirstOrDefault(f => f.Key.Equals(x.Key));
                    object data = vv.Value;
                    if (data != null)
                        sb.Append($"{data.ToString()},");
                    else
                        sb.Append($",");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public void Write(string fileName)
        {
            string str = this.ToString();

            bool trySave = true;
            while (trySave)
            {
                trySave = false;
                try
                {
                    System.IO.File.WriteAllText(fileName, str);
                }
                catch (Exception ex)
                {
                    trySave = (MessageBox.Show($"{ex.GetType().Name}: {ex.Message}", "", MessageBoxButtons.RetryCancel) == DialogResult.Retry);
                }
            }
        }
    }
}
