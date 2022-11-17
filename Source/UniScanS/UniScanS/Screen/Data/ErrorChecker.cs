using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanS.Screen.Data
{
    public enum SheetErrorType
    {
        None, InvalidInspect, DifferenceModel, InvalidPoleParam, InvalidDielectricParam, FiducialNG, Error
    }

    public class ErrorChecker
    {
        List<Tuple<int, SheetErrorType>> errorList = new List<Tuple<int, SheetErrorType>>();
        public List<Tuple<int, SheetErrorType>> ErrorList
        {
            get { return errorList; }
        }

        public SheetErrorType CheckError(MergeSheetResult mergeSheetResult)
        {
            if (MonitorSetting.Instance().ErrorNum == 0)
                return SheetErrorType.None;

            if (errorList.Count >= MonitorSetting.Instance().ErrorNum)
                errorList.RemoveAt(0);

            errorList.Add(new Tuple<int, SheetErrorType>(mergeSheetResult.Index, mergeSheetResult.SheetErrorType));

            if (errorList.Count < MonitorSetting.Instance().ErrorNum)
                return SheetErrorType.None;
            
            int noneNum = errorList.FindAll(error => error.Item2 == SheetErrorType.None).Count();

            SheetErrorType errorType = SheetErrorType.None;
            if (noneNum == 0)
            {
                List<Tuple<int, SheetErrorType>> tupleList = new List<Tuple<int, SheetErrorType>>();

                int errorNum = errorList.FindAll(error => error.Item2 == SheetErrorType.Error).Count();
                int fidNum = errorList.FindAll(error => error.Item2 == SheetErrorType.FiducialNG).Count();
                int diffNum = errorList.FindAll(error => error.Item2 == SheetErrorType.DifferenceModel).Count();
                int invalidNum = errorList.FindAll(error => error.Item2 == SheetErrorType.InvalidInspect).Count();
                int invalidPoleNum = errorList.FindAll(error => error.Item2 == SheetErrorType.InvalidPoleParam).Count();
                int invalidDielectricNum = errorList.FindAll(error => error.Item2 == SheetErrorType.InvalidDielectricParam).Count();

                errorType = errorList.Max(tuple => tuple.Item2);
            }

            return errorType;
        }

        public void Reset()
        {
            ErrorList.Clear();
        }
    }
}