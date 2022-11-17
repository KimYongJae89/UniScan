using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanX.MPAlignment.Algo.UI;
using DynMvp.Data;
using DynMvp.Vision;
using DynMvp.Vision.OpenCv;
using DynMvp.Base;
using ZedGraph;
using System.IO;
using DynMvp.UI;

namespace UniScanX.MPAlignment.Algo
{
    public partial class TestAlgoForm : Form
    {
        double LensCalibration = 0.8622625;
        TargetParamControl targetParamControl = null;
        Bitmap loaded_image;
        Bitmap loaded_image2;

        MPAlgorithm algorithm = new MPAlgorithm();
        CanvasPanel canvasPanel;
        Target visionProbe= null;
        public TestAlgoForm()
        {
            InitializeComponent();

            //this.targetParamControl = new TargetParamControl();
            //this.targetParamControl.Location = new System.Drawing.Point(3, 150);
            //this.Controls.Add(targetParamControl);


        }

        private void TestAlgoForm_Load(object sender, EventArgs e)
        {
            canvasPanel = new CanvasPanel();
            this.canvasPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.canvasPanel.Location = new System.Drawing.Point(0, 0);
            this.canvasPanel.Name = "ImageViewer";
            this.canvasPanel.Size = new System.Drawing.Size(10, 10);
            this.canvasPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasPanel.TabIndex = 26;
            this.canvasPanel.TabStop = false;
            //this.canvasPanel.Enable = true;
            this.canvasPanel.RotationLocked = false;

            this.panel_Canvas.Controls.Add(this.canvasPanel);
        }

        private void button_LoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdFile = new OpenFileDialog();
            ofdFile.ShowHelp = true;
            ofdFile.AutoUpgradeEnabled = false;
            ofdFile.Title = "IR 파일 오픈";
            ofdFile.FileName = "IR";
            ofdFile.Filter = "비트맵(*.BMP)|*.BMP|모든 파일(*.*)|*.*";
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                string ResultPath = ofdFile.FileName;
                this.Text = ofdFile.FileName;
                //this.Visible = false; //폼 감추기 
                if (loaded_image != null)
                {
                    loaded_image.Dispose();
                    loaded_image = null;
                }
                loaded_image =(Bitmap) Image.FromFile(ResultPath); //얘는 메모리 문제가 없네.

                canvasPanel.Clear();
                canvasPanel.UpdateImage(loaded_image);
                canvasPanel?.ZoomFit();
                //targetParamControl.UpdateTargetImage(loaded_image);
            }

            ofdFile.ShowHelp = true;
            ofdFile.AutoUpgradeEnabled = false;
            ofdFile.Title = "BLue 파일 오픈";
            ofdFile.FileName = "BLUE";
            ofdFile.Filter = "비트맵(*.BMP)|*.BMP|모든 파일(*.*)|*.*";
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                string ResultPath = ofdFile.FileName;
                this.Text = ofdFile.FileName;
                //this.Visible = false; //폼 감추기 
                if (loaded_image2 != null)
                {
                    loaded_image2.Dispose();
                    loaded_image2 = null;
                }
                loaded_image2 = (Bitmap)Image.FromFile(ResultPath); //얘는 메모리 문제가 없네.

                pictureBox1.Image = loaded_image2;
                //targetParamControl.UpdateTargetImage(loaded_image);
            }
        }

        private void UpdateParam()
        {
            MPAlgorithmParam param = this.algorithm.Param as MPAlgorithmParam;
            param.Force = checkBox_Force.Checked;
            param.ThresholdX = Decimal.ToSingle(nudThreshod_X1st.Value);
            param.ThresholdY = Decimal.ToSingle(nudThreshod_Y1st.Value);
            param.ThresholdX_2nd = Decimal.ToSingle(nudThreshod_X2nd.Value);
            param.ThresholdY_2nd = Decimal.ToSingle(nudThreshod_Y2nd.Value);

            param.HysterisisX = Decimal.ToSingle(nudHysterisisX.Value);
            param.HysterisisY = Decimal.ToSingle(nudHysterisisY.Value);

            param.Estimated_Margin = Decimal.ToInt32(nud_Margin.Value);
        }

        private void button_Capture_Click(object sender, EventArgs e)
        {
            // Total Screen Capture 
            //capureBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            //using (Graphics g = Graphics.FromImage(capureBitmap))
            //{
            //    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, capureBitmap.Size,
            //        CopyPixelOperation.SourceCopy);
            //    //Picture Box Display
            //    pictureBox1.Image = capureBitmap;
            //}
            var image   = CaptureFom(this);
  
        }

        public Bitmap CaptureFom(Form form)
        {
            Bitmap bitmap = new Bitmap(form.Width, form.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(form.PointToScreen(new Point(form.Left, form.Top)), new Point(0, 0), form.Size);
            graphics.Save();
            return bitmap;
        }

        private void button_Inspect_Click(object sender, EventArgs e)
        {
            UpdateParam();
            Image2D imageD = Image2D.FromBitmap((Bitmap)loaded_image);
            Image2D imageDBLUE = Image2D.FromBitmap((Bitmap)loaded_image2);

            AlgorithmInspectParam inspectParam = new AlgorithmInspectParam(imageD);
            inspectParam.ClipImage2 = imageDBLUE;

            MPAlgorithmResult result = algorithm.Inspect(inspectParam) as MPAlgorithmResult;
            String str = result.ToString();
            var margin = result.GetMargin();
            var offset = result.GetOffset2nd();

            string str2 = string.Format("\r\nMargin[um]=({0:F1},{1:F1})   nOffset[um]=({2:F1}, {3:F1})",
            margin.X * LensCalibration,
            margin.Y * LensCalibration,
            offset.X * LensCalibration,
            offset.Y * LensCalibration
            );

            string str3 = "", str4 = "";

            str3 = string.Format("\r\n\r\nX[{0}]=========================", result.XLinePairList.Count);
            if (result.XLinePairList.Count > 0)
            {
                int n = 0;
               foreach (var pair in result.XLinePairList)
                {
                    str3 += "\r\n[Pix]" + n.ToString()+ " = "+ PairDatatoString(pair);//, LensCalibration);
                    str3 += "\r\n[um]" + n.ToString() + " = " + PairDatatoString(pair, LensCalibration);
                    n++;
                }
            }

            str4 = string.Format("\r\n\r\nY[{0}]]==========================", result.YLinePairList.Count);
            if (result.YLinePairList.Count > 0)
            {
                int n = 0;
                foreach ( var pair in result.YLinePairList)
                {
                    str4 += "\r\n[Pix]" + n.ToString() + "=" + PairDatatoString(pair);//, LensCalibration);
                    str4 += "\r\n[um]" + n.ToString() + "=" + PairDatatoString(pair, LensCalibration);
                    n++;
                }
            }

            string PairDatatoString(MarginEdgePair pair, double lensCalibration =1.0)
            {
                if (pair == null) return "{Pair is Null}";
                return makeString(pair.Left1stRising) + "," + makeString(pair.Left2ndFalling) + "," +
                    makeString(pair.Right1stFalling) + "," + makeString(pair.Right2ndRising);

                string makeString(Peak peak)
                {
                    if (peak == null)
                        return string.Format("(null)");
                    else
                        return string.Format("({0:F1})", (double)peak.peakPos * lensCalibration);
                }
            }



            textBox1.Text = str + str2 + str3 + str4;

            result.ClipRect = new Rectangle(0, 0, imageD.Width, imageD.Height);
            canvasPanel.Clear();
            result.AppendResultFigures(canvasPanel.WorkingFigures, new PointF(0,0));
            canvasPanel.Update();

            DrawProfile(result.XLinePairList, result.YLinePairList, 
                result.EdgeProfileX, result.EdgeProfileY);
        }

        void DrawProfile(List<MarginEdgePair> XPairList , 
            List<MarginEdgePair> YPairList ,
            float[] XProfile, float[] YProfile )
        {
            //************************************************************//
   
            float[] indicateX = new float[XProfile.Length];
            foreach( var pair in  XPairList)
            {
                if (pair.Left1stRising != null)
                    indicateX[pair.Left1stRising.peakPos] = 100;
                if (pair.Left1stRising != null)
                    indicateX[pair.Left1stRising.peakPos] = 100;
                if (pair.Right1stFalling != null)
                    indicateX[pair.Right1stFalling.peakPos] = -100;
                if (pair.Left2ndFalling != null)
                    indicateX[pair.Left2ndFalling.peakPos] = -50;
                if (pair.Right2ndRising != null)
                    indicateX[pair.Right2ndRising.peakPos] = 50;
            }
            DrawGraph(zedGraphControl_X, XProfile, indicateX);
            //************************************************************//

            float[] indicateY = new float[YProfile.Length];
            foreach (var pair in YPairList)
            {
                if(pair.Left1stRising !=null)
                    indicateY[pair.Left1stRising.peakPos] = 100;
                if (pair.Left1stRising != null)
                    indicateY[pair.Left1stRising.peakPos] = 100;
                if (pair.Right1stFalling != null)
                    indicateY[pair.Right1stFalling.peakPos] = -100;
                if (pair.Left2ndFalling != null)
                    indicateY[pair.Left2ndFalling.peakPos] = -50;
                if (pair.Right2ndRising != null)
                    indicateY[pair.Right2ndRising.peakPos] = 50;
            }
            DrawGraph(zedGraphControl_Y, YProfile, indicateY);
        }

        void DrawGraph(ZedGraphControl zedG, float[] datas, float [] _indicate)
        {
            var myPane = zedG.GraphPane;
            myPane.CurveList.RemoveAll(f=> true);
            PointPairList list = new PointPairList();
            PointPairList list2 = new PointPairList();
            int x = 0;
            foreach(var data in datas)
            {
                list.Add(x, data);
                x++;
            }
            if (_indicate !=null)
            {
                x = 0;
                foreach (var data in _indicate)
                {
                    list2.Add(x, data);
                    x++;
                }
                LineItem myCurve2 = myPane.AddCurve("My Curve2", list2, Color.Red, SymbolType.None);
            }

            LineItem myCurve = myPane.AddCurve("My Curve", list, Color.Blue, SymbolType.None);
            // Fill the area under the curve with a white-red gradient at 45 degrees
            myCurve.Line.Fill = new Fill(Color.White, Color.Red, 45F);
            // Make the symbols opaque by filling them with white
            myCurve.Symbol.Fill = new Fill(Color.White);

            zedG.AxisChange();
            zedG.Refresh();
        }

        private void CreateGraph(ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = "My Test Graph";
            myPane.XAxis.Title.Text = "X Value";
            myPane.YAxis.Title.Text = "My Y Axis";

            // Make up some data points from the Sine function
            PointPairList list = new PointPairList();
            for (double x = 0; x < 36; x++)
            {
                double y = Math.Sin(x * Math.PI / 15.0);

                list.Add(x, y);
            }

            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve = myPane.AddCurve("My Curve", list, Color.Blue,
                                    SymbolType.Circle);
            // Fill the area under the curve with a white-red gradient at 45 degrees
            myCurve.Line.Fill = new Fill(Color.White, Color.Red, 45F);
            // Make the symbols opaque by filling them with white
            myCurve.Symbol.Fill = new Fill(Color.White);

            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);

            // Fill the pane background with a color gradient
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);

            // Calculate the Axis Scale Ranges
            zgc.AxisChange();
        }
        
        private void TestAlgoForm_SizeChanged(object sender, EventArgs e)
        {
            canvasPanel?.ZoomFit();
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            canvasPanel.Clear();
        }

        private void button_ZoomFit_Click(object sender, EventArgs e)
        {
            canvasPanel.ZoomFit();
        }

        private void button_ChangeImage_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == loaded_image2 )
            {
                pictureBox1.Image = loaded_image;
                canvasPanel.UpdateImage(loaded_image2);
                
            }
            else
            {
                pictureBox1.Image = loaded_image2;
                canvasPanel.UpdateImage(loaded_image);
            }
        }

        private void button_PairLoad_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog 

            OpenFileDialog ofdFile = new OpenFileDialog();
            ofdFile.ShowHelp = true;
            ofdFile.AutoUpgradeEnabled = false;
            ofdFile.Title = "IR 파일 오픈";
            ofdFile.FileName = "IR";
            ofdFile.Filter = "비트맵(*.jpeg)|*.jpeg|모든 파일(*.*)|*.*";
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                string ResultPath = ofdFile.FileName;
                this.Text = ofdFile.FileName;
                //this.Visible = false; //폼 감추기 
                if (loaded_image != null)
                {
                    loaded_image.Dispose();
                    loaded_image = null;
                }
                string filename = Path.GetFileNameWithoutExtension(ofdFile.FileName);
                string path = Path.GetDirectoryName(ofdFile.FileName);
                string fileExt = Path.GetExtension(ofdFile.FileName);
                var split = filename.Split('_');

                if (split.Length < 3)
                {
                    MessageBox.Show(this,"File name format invalid" );
                    return;
                }
                ////////////////////////////////////////////////////////////////////////////
                split[3] = "L02";
                string IRfullPath=Path.Combine(path, String.Join("_", split) + fileExt);
                if (File.Exists(IRfullPath))
                {
                    loaded_image = (Bitmap)Image.FromFile(IRfullPath);
                    canvasPanel.Clear();
                    canvasPanel.UpdateImage(loaded_image);
                    canvasPanel?.ZoomFit();
                }
                else canvasPanel.Clear();

                /////////////////////////////////////////////////////////////////////////////
                split[3] = "L01";
                string BLUEfullPath = Path.Combine(path, String.Join("_", split) + fileExt);
                if (File.Exists(IRfullPath))
                {
                    loaded_image2 = (Bitmap)Image.FromFile(BLUEfullPath); //얘는 메모리 문제가 없네.
                    pictureBox1.Image = loaded_image2;
                }
                else pictureBox1.Image = null;
            }

            //Path.GetFileName(fullName);                   //test.txt
            //Path.GetFileNameWithoutExtension(fullName);   //test
            //Path.GetExtension(fullName);                  //.txt
            //Path.GetPathRoot(fullName);                   //c:\
            //Path.GetDirectoryName(fullName);              //c:\\folder\\

        }
    }
}
