using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace DynMvp.Base
{
    public class LockFile
    {
        public bool IsLocked
        {
            get { return lockFile!=null; }
        }

        FileStream lockFile = null;
        string lockFilePath;

        public LockFile(string lockFilePath)
        {
            LogHelper.Debug(LoggerType.StartUp, "LockFile::LockFile");
            this.lockFilePath = lockFilePath;
            
            if (File.Exists(lockFilePath) == true)
            {
                //LogHelper.Warn(LoggerType.StartUp, "Abnormal program termination is detected.");
            }

            try
            {
                lockFile = File.Open(lockFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format(StringManager.GetString("Error on Create lock file. [{0}]"), e.Message));
            }
        }

        public void Dispose()
        {
            LogHelper.Debug(LoggerType.StartUp, "LockFile::Dispose");
            lockFile?.Close();
            if (File.Exists(lockFilePath))
                File.Delete(lockFilePath);
        }
    }
}
