using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageConverter
{
    class BindingSource : System.Windows.Forms.BindingSource
    {
        public string CommonPath { get; private set; }
        public new BindingList<PathState> List { get => (BindingList<PathState>)base.List; }

        public BindingSource()
        {
            this.CommonPath = "";
            this.DataSource = new BindingList<PathState>();
        }

        public override int Add(object value)
        {
            if (base.List.Contains(value))
                return -1;

            int index = base.Add(value);
            this.CommonPath = GetCommonPath(this.List.Select(f => System.IO.Path.GetDirectoryName(f.FullPath)).ToArray());
            UpdateDisplayPath();
            return index;
        }

        private void UpdateDisplayPath()
        {
            this.List.ToList().ForEach(f =>
            {
                f.DisplayPath = new string(f.FullPath.Skip(this.CommonPath.Length).ToArray()).TrimStart('\\');
            });
        }

        private string GetCommonPath(string[] v)
        {
            if (v.Count() == 0)
                return "";

            int lcs = GetLCS(v);
            return v[0].Substring(0, lcs);
        }

        private int GetLCS(string[] v)
        {
            if (v.Count() == 0)
                return 0;

            int idx = 0;
            int lim = v.Min(f => f.Length);
            bool good = false;

            do
            {
                char c = v[0][idx];
                good = Array.TrueForAll(v, f => f[idx] == c);
                if (good)
                    idx++;
            } while (good && (idx < lim));

            return idx;
        }
    }

    enum ConvertState { Idle, Pass, Wait, Working, Done, Error }
    class PathState : INotifyPropertyChanged
    {
        string displayPath = "";
        public string DisplayPath
        {
            get => this.displayPath;
            set
            {
                if (this.displayPath != value)
                {
                    this.displayPath = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayPath"));
                }
            }
        }

        string fullPath = "";
        public string FullPath
        {
            get => this.fullPath;
            set
            {
                if (this.fullPath != value)
                {
                    this.fullPath = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FullPath"));
                }
            }
        }

        string state = "";
        public string State { get => this.state; }

        public bool IsDone => (state == ConvertState.Done.ToString());
        public PathState(string path)
        {
            //this.DisplayPath = path;
            this.FullPath = path;
            SetState(ConvertState.Idle);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetState(ConvertState state, string message = "")
        {
            if (string.IsNullOrEmpty(message))
                this.state = $"{state}";
            else
                this.state = $"{state}: {message}";

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("State"));
        }

        public override bool Equals(object obj)
        {
            PathState pathState = obj as PathState;
            if (pathState == null)
                return false;
            return this.FullPath == pathState.FullPath;
        }

        public override int GetHashCode()
        {
            return 2018552787 + EqualityComparer<string>.Default.GetHashCode(FullPath);
        }
    }
}
