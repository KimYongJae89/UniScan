using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaMotionMove
{
    class _Position
    {
        string name;

        public int PositionMm { get => this.positionMm; set => this.positionMm = value; }
        int positionMm;

        public int DelayMs { get => this.delayMs; set => this.delayMs = value; }
        int delayMs;

        public bool IoActive { get => this.ioActive; set => this.ioActive = value; }
        bool ioActive;

        public _Position(string name)
        {
            this.name = name;
        }

        internal void Load(AppSettingsReader ar)
        {
            this.positionMm = (int)ar.GetValue(string.Format("{0}_PositionMm", this.name), typeof(int));
            this.delayMs = (int)ar.GetValue(string.Format("{0}_DelayMs", this.name), typeof(int));
            this.ioActive = (bool)ar.GetValue(string.Format("{0}_IoActive", this.name), typeof(bool));
        }

        internal void Save(KeyValueConfigurationCollection cfgCollection)
        {
            cfgCollection.Add(string.Format("{0}_PositionMm", this.name), this.positionMm.ToString());
            cfgCollection.Add(string.Format("{0}_DelayMs", this.name), this.delayMs.ToString());
            cfgCollection.Add(string.Format("{0}_IoActive", this.name), this.ioActive.ToString());
        }
    }

    class _Direction
    {
        string name;

        public int VelocityMmps { get => this.velocityMmps; set => this.velocityMmps = value; }
        int velocityMmps;

        public int AccelationMs { get => this.accelationMs; set => this.accelationMs = value; }
        int accelationMs;

        public bool IoActive { get => this.ioActive; set => this.ioActive = value; }
        bool ioActive;

        public _Direction(string name)
        {
            this.name = name;
        }

        public MovingParam GetMovingParam()
        {
            return new MovingParam("", 10, AccelationMs, AccelationMs, VelocityMmps * 1000, 0);
        }

        internal void Load(AppSettingsReader ar)
        {
            this.velocityMmps = (int)ar.GetValue(string.Format("{0}_VelocityMmps", this.name), typeof(int));
            this.accelationMs = (int)ar.GetValue(string.Format("{0}_AccelationMs", this.name), typeof(int));
            this.ioActive = (bool)ar.GetValue(string.Format("{0}_IoActive", this.name), typeof(bool));
        }

        internal void Save(KeyValueConfigurationCollection cfgCollection)
        {
            cfgCollection.Add(string.Format("{0}_VelocityMmps", this.name), this.velocityMmps.ToString());
            cfgCollection.Add(string.Format("{0}_AccelationMs", this.name), this.accelationMs.ToString());
            cfgCollection.Add(string.Format("{0}_IoActive", this.name), this.ioActive.ToString());
        }
    }

    class _Time
    {
        string name;

        public bool Use { get => this.use; set => this.use = value; }
        bool use;
        public float TimeSec { get => this.timeSec; set => this.timeSec = value; }
        float timeSec;

        public _Time(string name)
        {
            this.name = name;
        }

        internal void Load(AppSettingsReader ar)
        {
            this.use = (bool)ar.GetValue(string.Format("{0}_Use", this.name),  typeof(bool));
            this.timeSec = (float)ar.GetValue(string.Format("{0}_TimeSec", this.name), typeof(float));
        }

        internal void Save(KeyValueConfigurationCollection cfgCollection)
        {
            cfgCollection.Add(string.Format("{0}_Use", this.name), this.use.ToString());
            cfgCollection.Add(string.Format("{0}_TimeSec", this.name), this.timeSec.ToString());
        }
    }

    class Settings
    {        
        public MotionInfo MotionInfo { get; }

        public _Position Departure { get; }
        public _Position Arrival { get; }

        public _Direction Forward { get; }
        public _Direction Backward { get;}

        public _Time StopIn { get; }
        public _Time RunIn { get; }

        public Settings()
        {

            this.MotionInfo = MotionInfoFactory.CreateMotionInfo(MotionType.AlphaMotionBx);
            this.MotionInfo.NumAxis = 1;
                //this.MotionInfo = MotionInfoFactory.CreateMotionInfo(MotionType.Virtual);

            this.Departure = new _Position("Departure");
            this.Arrival = new _Position("Arrival");

            this.Forward = new _Direction("Forward");
            this.Backward = new _Direction("Backward");

            this.StopIn = new _Time("StopIn");
            this.RunIn = new _Time("RunIn");
        }

        internal void Load()
        {
            AppSettingsReader ar = new AppSettingsReader();
            try
            {
                this.Departure.Load(ar);
                this.Arrival.Load(ar);

                this.Forward.Load(ar);
                this.Backward.Load(ar);

                this.StopIn.Load(ar);
                this.RunIn.Load(ar);
            }
            catch { }
        }

        internal void Save()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection cfgCollection = config.AppSettings.Settings;

            cfgCollection.Clear();

            this.Departure.Save(cfgCollection);
            this.Arrival.Save(cfgCollection);

            this.Forward.Save(cfgCollection);
            this.Backward.Save(cfgCollection);

            this.StopIn.Save(cfgCollection);
            this.RunIn.Save(cfgCollection);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
