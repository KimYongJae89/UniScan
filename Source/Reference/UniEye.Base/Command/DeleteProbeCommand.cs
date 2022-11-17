using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniEye.Base.Command
{
    public class DeleteProbeCommand : ICommand
    {
        Target target;
        Probe probe;
        PositionAligner positionAligner;

        public DeleteProbeCommand(Target target, Probe probe, PositionAligner positionAligner)
        {
            this.target = target;
            this.probe = probe;
            this.positionAligner = positionAligner;
        }

        void ICommand.Execute()
        {
            target.RemoveProbe(probe);
            target.UpdateRegion(positionAligner);
        }

        void ICommand.Redo()
        {
            target.RemoveProbe(probe);
            target.UpdateRegion(positionAligner);
        }

        void ICommand.Undo()
        {
            target.AddProbe(probe);
            target.UpdateRegion(positionAligner);
        }
    }
}
