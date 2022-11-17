using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniScanG.Common.Data;
using UniScanG.Gravure.UI.Setting;
using UniScanG.Module.Controller.Settings.Monitor;
using UniScanG.Module.Controller.UI.Setting;

namespace UniScanG.Module.Controller.UI.Settings
{
    class SettingPageExtender : ISettingPageExtender
    {
        public bool IsVisibleCollectLogButton => true;

        public ISettingSubPage CreateAlarmPage()
        {
            return new SettingAlarmPage();
        }

        public ISettingSubPage CreateCommPage()
        {
            return new SettingCommPage()
            {
                ShowEncoderButton = true,
                ShowImsPowControlButton = MonitorSystemSettings.Instance().EnableImsPowControl,
            };
        }

        public ISettingSubPage CreateGradePage()
        {
            if (DynMvp.Base.LicenseManager.Exist("DecGrade"))
                return new SettingGradePage();
            return null;
        }

        public ISettingSubPage CreateGeneralPage()
        {
            return new SettingGeneralPage();
        }


        public void CollectLog(IWin32Window parent)
        {
            (new CollectLogForm()).ShowDialog(parent);
            return;

            FolderBrowserDialog dlg = new FolderBrowserDialog() {
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                //SelectedPath = @"D:\새 폴더",
                ShowNewFolderButton = true,
                Description = "Select Folder",
            };

            if (dlg.ShowDialog(parent) == DialogResult.Cancel)
                return;

            DirectoryInfo targetInfo = new DirectoryInfo(Path.Combine(dlg.SelectedPath, $"CollectLog_{DateTime.Now.ToString("yyyy_MM_dd")}"));
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            list.Add(new Tuple<string, string>(PathSettings.Instance().Log, Path.Combine(targetInfo.FullName, "CM")));

            List<InspectorObj> inspectorObjList = ((Common.IServerExchangeOperator)SystemManager.Instance().ExchangeOperator).GetInspectorList();
            list.AddRange(inspectorObjList.Select(f =>
            {
                string srcPath = Path.Combine(f.Info.Path, "Log");
                string dstPath = Path.Combine(targetInfo.FullName, f.Info.GetName());
                return new Tuple<string, string>(srcPath, dstPath);
            }));

            new SimpleProgressForm("Copying...").Show(parent, () =>
            {
                try
                {
                    //list.ForEach(f =>
                    list.AsParallel().ForAll(f =>
                    {
                        DirectoryInfo src = new DirectoryInfo(f.Item1);
                        if (src.Exists)
                        {
                            DirectoryInfo dst = new DirectoryInfo(f.Item2);
                            if (!dst.Exists)
                                dst.Create();

                            FileHelper.CopyDirectory(f.Item1, f.Item2, true, true, cancellationTokenSource.Token);
                        }
                    });
                }
                catch (OperationCanceledException) { }
                catch (Exception ex) { LogHelper.Error(LoggerType.Error, $"SettingPageExtender::CollectLog - Copy, {ex.GetType().Name}: {ex.Message}"); }
            });

            if (!cancellationTokenSource.IsCancellationRequested)
            {
                new SimpleProgressForm("Compressing...").Show(() =>
                {
                    try
                    {
                        FileInfo zipInfo = new FileInfo($"{targetInfo.FullName}.zip");
                        FileHelper.CompressZip(targetInfo, zipInfo, cancellationTokenSource);

                        FileInfo zipInfo2 = new FileInfo(Path.Combine(targetInfo.FullName, $"{targetInfo.Name}.zip"));
                        FileHelper.Move(zipInfo.FullName, zipInfo2.FullName);

                    }
                    catch (OperationCanceledException) { }
                    catch (Exception ex) { LogHelper.Error(LoggerType.Error, $"SettingPageExtender::CollectLog - Compress, {ex.GetType().Name}: {ex.Message}"); }
                }, cancellationTokenSource);
            }

            System.Diagnostics.Process.Start(targetInfo.FullName);
        }
    }
}
