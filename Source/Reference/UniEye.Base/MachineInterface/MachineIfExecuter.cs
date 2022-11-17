using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniEye.Base.MachineInterface
{

    public class ExecuteResult
    {
        /// <summary>
        /// 명령 수신, 유효성 검사 성공, 명령 처리 시작시 TRUE.
        /// </summary>
        bool ack = false;
        public bool Ack
        {
            get { return this.ack; }
            set { this.ack = value; }
        }

        /// <summary>
        /// 명령 처리가 끝나면 TRUE
        /// </summary>
        bool operation = false;
        public bool Operation
        {
            get { return this.operation; }
            set { this.operation = value; }
        }

        /// <summary>
        /// 처리 성공?
        /// </summary>
        bool success = false;
        public bool Success
        {
            get { return this.success; }
            set { this.success = value; }
        }

        /// <summary>
        ///  코멘트.
        /// </summary>
        string message = "";
        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }
    }

    public abstract class MachineIfExecuter
    {
        protected bool isBusy = false;
        public bool IsBusy
        {
            get { return this.isBusy; }
        }

        protected abstract bool Execute(MachineIfProtocolWithArguments command);

        public MachineIfExecuter()
        {

        }

        public virtual ExecuteResult ExecuteCommand(MachineIfProtocolWithArguments command)
        {
            ExecuteResult executeResult = new ExecuteResult();

            //if (String.IsNullOrEmpty(command) == true)
            //    return executeResult;
            
            executeResult.Operation = Execute(command);
            return executeResult;
        }
    }

}
