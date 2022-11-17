using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniScanWPF.Table.Operation.Operators
{
    public abstract class TaskOperator : Operator
    {

        List<Task> taskList;   

        public TaskOperator()
        {
            this.taskList = new List<Task>();
        }

        public override bool Initialize(ResultKey resultKey, int totalProgressSteps, CancellationTokenSource cancellationTokenSource)
        {
            bool ok = base.Initialize(resultKey, totalProgressSteps, cancellationTokenSource);
            taskList.Clear();

            return ok;
        }

        public void AddTask(Task task)
        {
            lock (taskList)
                taskList.Add(task);
        }

        public void WaitDone()
        {
            List<Task> workingTaskList = null;
            do
            {
                lock (taskList)
                    workingTaskList = taskList.FindAll(f => !f.IsCompleted);

                Task.WaitAll(workingTaskList.ToArray());
            }
            while (workingTaskList.Count > 0);

            System.Diagnostics.Debug.Assert(this.Progress == 100);
            taskList.Clear();
        }

    }
}
