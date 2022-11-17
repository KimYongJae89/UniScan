using AutoParamTuner.Model;
using AutoParamTuner.UI;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Util;
using UniScanG.Common;
using UniScanG.Common.Settings;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Trainer;
using UniScanG.Module.Inspector;
using UniScanG.Module.Inspector.Settings.Inspector;

namespace AutoParamTuner
{
    static class Program
    {
        public static MainForm MainForm { get; private set; }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ApplicationHelper.InitLogSystem();
            ApplicationHelper.LoadSettings();

            InspectorConfigHelper.SetInstance();
            InspectorConfigHelper.Instance().BuildSystemManager();

            InspectorConfigHelper.Instance().SetupAlgorithmStrategyAction(null);

            TunerModel model = new TunerModel();
            MainFormViewModel mainFormViewModel = new MainFormViewModel(model);
            MainForm = new MainForm(mainFormViewModel);

            Application.Run(MainForm);
            MatroxHelper.FreeApplication();
        }
    }
}
