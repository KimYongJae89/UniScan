using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Base
{
    public class Configuration
    {
        //private static int numAxis;
        //public static int NumAxis
        //{
        //    get { return Configuration.numAxis; }
        //set { Configuration.numAxis = value; }
        //}

        private static string configFolder;
        public static string ConfigFolder
        {
            get { return Configuration.configFolder; }
            set { Configuration.configFolder = value; }
        }

        private static string tempFolder;
        public static string TempFolder
        {
            get { return Configuration.tempFolder; }
            //set { Configuration.tempFolder = value; }
        }

        public static int TrackerSize { get; set; } = 14;

        private static bool autoResetProductionCount;
        public static bool AutoResetProductionCount
        {
            get { return Configuration.autoResetProductionCount; }
            //set { Configuration.autoResetProductionCount = value; }
        }

        private static bool showAngleMarker = true;
        public static bool ShowAngleMarker
        {
            get { return Configuration.showAngleMarker; }
            //set { Configuration.showAngleMarker = value; }
        }

        private static int numSerialReading;
        public static int NumSerialReading
        {
            get { return Configuration.numSerialReading; }
            //set { Configuration.numSerialReading = value; }
        }

        public static void Initialize(/*int numAxis, */string configFolder, string tempFolder, int trackerSize, bool autoResetProductionCount, bool showAngleMarker, int numSerialReading)
        {
            //Configuration.numAxis = numAxis;
            Configuration.configFolder = configFolder;
            Configuration.tempFolder = tempFolder;
            Configuration.TrackerSize = trackerSize;
            Configuration.autoResetProductionCount = autoResetProductionCount;
            Configuration.showAngleMarker = showAngleMarker;
            Configuration.numSerialReading = numSerialReading;
        }
    }
}
