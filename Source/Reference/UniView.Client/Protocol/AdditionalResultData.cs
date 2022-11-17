using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniView.Client.Protocol
{
    [Serializable()]
    public class AdditionalResultData
    {
        #region 속성
        /// <summary>
        /// 값의 이름 : 없어도 된다.(없을 시 null)
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// 검사 결과 값: 검사 결과 값을 넣으면 된다. 
        /// 예) value = 99.6
        /// </summary>
        public object Value { get; set; } = null; // 값 

        /// <summary>
        /// 단위 : 검사 결과의 단위 m든 px이든 검사 결과의 단위값을 입력하고, 이 내용이 있을시에 결과 페이지에 추가로 표시 된다
        /// </summary>
        public string Unit { get; set; } = null; // 단위

        /// <summary>
        /// 검사 결과가 여러개 있을 경우 각 개소의 검사 결과를 저장한다.
        /// </summary>
        public JudgementType Judgement { get; set; } = JudgementType.Unknown;

        /// <summary>
        /// 대표 하는 값(단일 값일 수도 있고 복수 값 일 수도 있으며, 이 값의 경우 별도의 테이블에 저장 되며, 실제로 표시 된다.)
        /// </summary>
        public bool IsRepresent { get; set; } = false;

        /// <summary>
        /// 검사 위치값
        /// </summary>
        public PointF Position { get; set; } = new PointF();
        #endregion
    }
}
