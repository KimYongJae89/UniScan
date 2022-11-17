using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Vision.RCI.Trainer;

namespace UniScanG.Gravure.Vision.RCI.Calculator
{
    public class CalculatorResultV3 : Vision.Calculator.CalculatorResult
    {
        public Rectangle FoundRoi { get; set; }
        public Dictionary<WorkPoint, Rectangle> PTMResults { get; set; }
        public long ElapsedMs { get; set; }

        public BlockResult[] BlockResults { get; set; }
        public HighDiffBlock[] HighDiffBlocks { get; set; }
        public PTMLogger[] PTMLoggers { get; set; }

        public  void SaveForDebug(string path)
        {
            Tuple<RCIDataWriter, string, Func<BlockResult, object>>[] tuples = new Tuple<RCIDataWriter, string, Func<BlockResult, object>>[]
                {
                        new Tuple<RCIDataWriter, string, Func<BlockResult, object>>(new RCIDataWriter(null,$"{this.ElapsedMs}[ms]"),Path.Combine(path, $"Comapre_V.csv"),
                            new Func<BlockResult, object>(f=>f.MaxValue)),
                        new Tuple<RCIDataWriter, string, Func<BlockResult, object>>(new RCIDataWriter(null,$"{this.ElapsedMs}[ms]"),Path.Combine(path, $"Comapre_X.csv"),
                            new Func<BlockResult, object>(f=>f.Offset.X)),
                        new Tuple<RCIDataWriter, string, Func<BlockResult, object>>(new RCIDataWriter(null,$"{this.ElapsedMs}[ms]"),Path.Combine(path, $"Comapre_Y.csv"),
                            new Func<BlockResult, object>(f=>f.Offset.Y))
                };

            Task task = Task.Run(() =>
            {
                System.IO.Directory.CreateDirectory(path);
                this.DebugImageD?.SaveImage(Path.Combine(path, $"DebugImageD.png"));

                string blocksPath = Path.Combine(path, $"Blocks");
                System.IO.Directory.CreateDirectory(blocksPath);
                FileHelper.ClearFolder(blocksPath);
                if (this.HighDiffBlocks != null)
                    Array.ForEach(this.HighDiffBlocks, f => f.Save(blocksPath));
            });

            Task task2 = Task.Run(() =>
            {
                string ptmLogPath = Path.Combine(path, "PTMLog");
                System.IO.Directory.CreateDirectory(ptmLogPath);
                FileHelper.ClearFolder(ptmLogPath);
                if (this.PTMLoggers != null)
                {
                    //PTMLogger[] rowPTMLogger = Array.FindAll(this.PTMLoggers, f => f.Direction == DynMvp.Vision.Direction.Vertical);
                    //Array.ForEach(rowPTMLogger, f => f?.Save(ptmLogPath));
                    Array.ForEach(this.PTMLoggers, f => f?.Save(ptmLogPath));

                }
            });

            Array.ForEach(tuples, f =>
            {
                Array.ForEach(this.BlockResults, g => f.Item1.Add(g.Column, g.Row, f.Item3((g))));
                f.Item1.Write(f.Item2);
            });

            task.Wait();
            task2.Wait();
        }
    }
}
