using DynMvp.Base;
using DynMvp.Data.DatabaseManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.Data
{
    public class DBDataExporter : DynMvp.Data.DataExporter
    {
        #region 생성자
        public DBDataExporter()
        {
            InitializeColumnNames();
        }
        #endregion


        #region 속성
        //DB Info
        public string DbIpAddress { get; set; } = "127.0.0.1";
        public string DbName { get; set; } = "UniScan";
        public string DbUserName { get; set; } = "postgres";
        public string DbPassword { get; set; } = "masterkey";

        //Lot Info
        public string LotName { get; private set; }

        public NetworkDrive NetworkDrive { get; set; } = new NetworkDrive();
        public string NetworkDriveIpAddress { get; set; } = "127.0.0.1";
        public string NetworkDriveUserName { get; set; }
        public string NetworkDrivePassword { get; set; }

        //Table Info
        private List<string> GlossTraverseColumnNames { get; set; }
        private List<string> GlossDataColumnNames { get; set; }
        private List<string> GlossZoneDataColumnNames { get; set; }

        // Getter
        //private ModelDescription ModelDescription { get => ModelManager.Instance().CurrentModel?.ModelDescription as ModelDescription; }
        //private SystemConfig SystemConfig { get => SystemConfig.Instance(); }
        #endregion


        #region 메서드
        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            var InspectionResult = inspectionResult as InspectionResult;
            if (inspectionResult != null)
            {
                ExportTraverseData(InspectionResult.GlossScanData);
            }
        }

        public void ExportTraverseData(GlossScanData scanData)
        {
            if (scanData == null)
            {
                return;
            }

            GlossScanWidth modelWidth = GlossSettings.Instance().SelectedGlossScanWidth;

            float start = modelWidth.Start;
            float validStart = modelWidth.ValidStart;
            float validEnd = modelWidth.ValidEnd;
            float end = modelWidth.End;

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);

            if (!dbManager.ConnectDatabase())
            {
                LogHelper.Error(LoggerType.Error, "DataExporter::ExprotDefectData - ConnectDatabase false");
                return;
            }

            dbManager.BeginTransaction();

            var traverseDatas = new List<object>();
            traverseDatas.Add(LotName);
            traverseDatas.Add(scanData.RollPosition);
            traverseDatas.Add(start);
            traverseDatas.Add(validStart);
            traverseDatas.Add(validEnd);
            traverseDatas.Add(end);
            traverseDatas.Add(scanData.MinGloss);
            traverseDatas.Add(scanData.MaxGloss);
            traverseDatas.Add(scanData.AvgGloss);
            traverseDatas.Add(scanData.DevGloss);
            traverseDatas.Add(scanData.MinDistance);
            traverseDatas.Add(scanData.MaxDistance);
            traverseDatas.Add(scanData.AvgDistance);
            traverseDatas.Add(scanData.DevDistance);

            dbManager.InsertData("GM_Traverse", GlossTraverseColumnNames, traverseDatas);

            for (int i = 0; i < scanData.Count; i++)
            {
                var thicknessDatas = new List<object>();
                thicknessDatas.Add(LotName);
                thicknessDatas.Add(scanData.RollPosition);
                thicknessDatas.Add(scanData.GlossDatas[i].X);
                thicknessDatas.Add(scanData.GlossDatas[i].Y);
                thicknessDatas.Add(scanData.GlossDatas[i].Distance);

                dbManager.InsertData("GM_Data", GlossDataColumnNames, thicknessDatas);
            }

            dbManager.EndTransaction();
            dbManager.DisconnectDatabase();
        }

        private void InitializeColumnNames()
        {
            GlossTraverseColumnNames = new List<string>() { "lot_name", "roll_position",
                                                            "start_position", "valid_start_position", "valid_end_position", "end_position",
                                                            "gloss_min", "gloss_max", "gloss_avg", "gloss_dev",
                                                            "distance_min", "distance_max", "distance_avg", "distance_dev" };
            GlossDataColumnNames = new List<string>() { "lot_name", "roll_position", "traverse_position", "gloss_data", "distance_data" };
        }

        private void InitializeNetworkDrive()
        {
            string remotePath = $@"\\{Path.Combine(NetworkDriveIpAddress, "Result")}";
            if (NetworkDrive.ConnectNetworkDrive("", remotePath, NetworkDriveUserName, NetworkDrivePassword) != 1)
            {
                MessageBox.Show($"네트워크 접근을 실패 했습니다.\n네트워크 경로 : {remotePath}\n유저 이름 : {NetworkDriveUserName}\n비밀번호 : {NetworkDrivePassword}");
            }
        }

        public void SetDataBaseInfo(string dbIpAddress, string dbName, string dbUserName, string dbPassword)
        {
            DbIpAddress = dbIpAddress;
            DbName = dbName;
            DbUserName = dbUserName;
            DbPassword = dbPassword;
        }

        public void SetNetworkDriveInfo(string networkDriveIpAddress, string networkDriveUserName, string networkDrivePassword)
        {
            NetworkDriveIpAddress = networkDriveIpAddress;
            NetworkDriveUserName = networkDriveUserName;
            NetworkDrivePassword = networkDrivePassword;

            if (NetworkDriveIpAddress != "127.0.0.1")
            {
                InitializeNetworkDrive();
            }
        }

        public void SetLotInfo(string lotName)
        {
            LotName = lotName;
        }
        #endregion
    }
}
