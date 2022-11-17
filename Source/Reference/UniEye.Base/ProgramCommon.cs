using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniEye.Base
{
    public static class ProgramCommon
    {
        public static LockFile CreateLockFile(string tempFIlder)
        {
            // 다운이 발생했을 때, 이전 디버그 로그를 저장
            string lockFilePath = Path.Combine(tempFIlder, "~UniEye.lock");

            return new LockFile(lockFilePath);
        }
    }
}
