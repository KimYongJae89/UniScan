using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniView.Client.Protocol
{
    [Serializable()]
    public class UniViewResultProtocol : UniViewProtocolBase
    {
        #region 생성자
        public UniViewResultProtocol()
        {
            SetDefault(UniViewProtocolType.Result);
        }

        public UniViewResultProtocol(string productName, string lotNo, JudgementType result, string barcode = "")
        {
            SetDefault(UniViewProtocolType.Result);
            Judgement = result;
            ProductName = productName;
            LotNo = lotNo;
            Judgement = result;
            Barcode = barcode;
        }

        public UniViewResultProtocol(string productName, string lotNo, JudgementType result, InspectResultExtendedInfo inspectResultExtendedInfo)
        {
            SetDefault(UniViewProtocolType.Result);
            Judgement = result;
            ProductName = productName;
            LotNo = lotNo;
            Judgement = result;
            InspectResultExtendedInfo = inspectResultExtendedInfo;
        }
        #endregion


        #region 속성
        /// <summary>
        /// 검사된 제품의 명칭 (없을 시 null)
        /// </summary>
        public string ProductName { get; set; } = "";

        /// <summary>
        /// 검사된 제품의 로트번호 (없을시 null)
        /// </summary>
        public string LotNo { get; set; } = "";

        /// <summary>
        /// 검사된 제품의 결과 
        /// </summary>
        public JudgementType Judgement { get; set; } = JudgementType.Unknown;

        /// <summary>
        /// 검사된 제품의 바코드(없을시 null)
        /// </summary>
        public string Barcode { get; set; } = "";

        /// <summary>
        /// 만약에 상기 지정된 추가 코드가 있다면 
        /// InspectResultExtendedInfo 를 상속 받아 만든다.
        /// </summary>
        public InspectResultExtendedInfo InspectResultExtendedInfo { get; set; } = null;

        /// <summary>
        /// 추가 표기 결과 리스트
        /// </summary>
        public List<AdditionalResultData> AddtionalResultDatas { get; set; } = new List<AdditionalResultData>();
        #endregion
    }
}
