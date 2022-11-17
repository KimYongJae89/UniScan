using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Base;
using DynMvp.Device.FrameGrabber;
using System.IO;
using DynMvp.Devices.FrameGrabber;
using System.Threading;

namespace DynMvp.Data.UI
{
    enum TargetShape
    {
        Standard, Rectangle, Circle
    }

    public partial class SchemaEditor : Form
    {
        private DrawBox cameraImage;
        private SchemaViewer schemaViewer;
        private Schema schema = new Schema();
        private ImageAcquisition imageAcquisition;
        private Model model;
        private FigureType addFigureType;
        private bool modified = false;
        private ObjectTree objectTree;

        bool showInspectionStep;
        public bool ShowInspectionStep
        {
            get { return showInspectionStep; }
            set { showInspectionStep = value; }
        }

        bool lockUpdate = false;

        public SchemaEditor()
        {
            schemaViewer = new SchemaViewer();
            cameraImage = new DrawBox();
            this.objectTree = new ObjectTree();

            InitializeComponent();

            this.SuspendLayout();

            this.KeyPreview = true;

            // 
            // objectTree
            // 
            this.objectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTree.Location = new System.Drawing.Point(0, 0);
            this.objectTree.Margin = new System.Windows.Forms.Padding(5);
            this.objectTree.Name = "objectTree";
            this.objectTree.Size = new System.Drawing.Size(294, 468);
            this.objectTree.TabIndex = 0;
            this.objectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectTree_AfterSelect);
            this.objectTree.DoubleClick += new System.EventHandler(this.objectTree_DoubleClick);

            this.panelObjectTree.Controls.Add(this.objectTree);
            // 
            // schemaViewer
            // 
            this.schemaViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemaViewer.Location = new System.Drawing.Point(246, 0);
            this.schemaViewer.Name = "schemaViewer";
            this.schemaViewer.Size = new System.Drawing.Size(511, 507);
            this.schemaViewer.TabIndex = 1;
            this.schemaViewer.TabStop = false;
            this.schemaViewer.Enable = true;
            this.schemaViewer.AddRegionCaptured = new AddRegionCapturedDelegate(schemaViewer_AddRegionCaptured);
            this.schemaViewer.FigureMoved = new FigureMovedDelegate(schemaViewer_FigureMoved);
            this.schemaViewer.FigureSelected = new FigureSelectedDelegate(schemaViewer_FigureSelected);
            this.schemaViewer.FigureCopy = new FigureCopyDelegate(schemaViewer_FigureCopy);

            this.splitContainer.Panel1.Controls.Add(this.cameraImage);

            // 
            // cameraImage
            // 
            this.cameraImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cameraImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cameraImage.Location = new System.Drawing.Point(3, 3);
            this.cameraImage.Name = "cameraImage";
            this.cameraImage.Size = new System.Drawing.Size(409, 523);
            this.cameraImage.TabIndex = 8;
            this.cameraImage.TabStop = false;
            this.cameraImage.Enable = true;

            this.schemaViewPanel.Controls.Add(this.schemaViewer);
            this.ResumeLayout(false);

            // change language
            this.Text = StringManager.GetString(this.GetType().FullName,this.Text);
            toolTip.SetToolTip(addImageButton, StringManager.GetString(this.GetType().FullName,toolTip.GetToolTip(addImageButton)));
            toolTip.SetToolTip(addLineButton, StringManager.GetString(this.GetType().FullName,toolTip.GetToolTip(addLineButton)));
            toolTip.SetToolTip(addCircleButton, StringManager.GetString(this.GetType().FullName,toolTip.GetToolTip(addCircleButton)));
            toolTip.SetToolTip(addRectangleButton, StringManager.GetString(this.GetType().FullName,toolTip.GetToolTip(addRectangleButton)));
            toolTip.SetToolTip(saveButton, StringManager.GetString(this.GetType().FullName,toolTip.GetToolTip(saveButton)));
            toolTip.SetToolTip(refreshButton, StringManager.GetString(this.GetType().FullName,toolTip.GetToolTip(refreshButton)));
        }

        public void Initialize(Model model, ImageAcquisition imageSource)
        {
            this.model = model;
            this.imageAcquisition = imageSource;
        }

        private void ModelSchemaEditor_Load(object sender, EventArgs e)
        {
            model.GetInspectionStep(0).UpdateImageBuffer(imageAcquisition.ImageBuffer);
            imageAcquisition.Acquire(0, 0);

            schema = model.ModelSchema.Clone();
            //previousSchema = schema;
            lockUpdate = true; 

            if (schema.Region == RectangleF.Empty)
                schema.Region = new RectangleF(0, 0, schemaViewer.Width, schemaViewer.Height);
            autoFit.Checked = schema.AutoFit;
            
            scale.Text = Convert.ToInt32(schema.ViewScale * 100).ToString();
            scale.Enabled = (autoFit.Checked == false);

            schema.ViewScale = 1;
            targetShape.SelectedIndex = 0;

            this.schemaViewer.Schema = schema;
            //schemaViewer.Gradient(BackColor);
            if (showInspectionStep == true)
                objectTree.InspectionStepName = "FOV";

            objectTree.Initialize(model);

            lockUpdate = false;
        }

        private void objectTree_DoubleClick(object sender, EventArgs e)
        {
            if (objectTree.SelectedNode == null)
                return;

            object obj = objectTree.SelectedNode.Tag;
            if (obj != null)
            {
                if (obj is TargetGroup)
                {

                }
                else if (obj is Target)
                {
                    Target target = (Target)obj;
                    InsertTarget(target);
                }
                else if (obj is Probe)
                {
                    Probe probe = (Probe)obj;
                    InsertProbe(probe);
                }
                else
                {                    
                    CaseByTypeOfObj(obj);
                    //Type type = obj.GetType();
                    //string test = obj.GetType().ToString();
                    //string tagName = (string)obj;
                    //TagTypeIsString(tagName);
                }
            }
        }

        private void CaseByTypeOfObj(object obj)
        {
            //if (obj.GetType().Name == "string")
            TagTypeIsString((string)obj);
            //else
            //{
            //    TagTypeIsBitmap(obj);
            //}
                
        }

        private void TagTypeIsString(string tagName)
        {
            if (tagName.Contains("ResultValue."))
            {
                string probeResultName = tagName.Substring("ResultValue.".Count());
                Probe probe = (Probe)objectTree.SelectedNode.Parent.Tag;
                InsertProbeResult(probe, probeResultName);
            }
            else if (tagName.Contains("TargetType."))
            {
                string targetType = tagName.Substring("TargetType.".Count());
                InsertTargetType(targetType);
            }
            else if(tagName.Contains(tagName))
            {
                //string targetType = tagName.Substring("Image.".Count());
                Target target = (Target)objectTree.SelectedNode.Parent.Tag;
                InsertTargetImage(target);
            }
        }

        private void TagTypeIsBitmap(object obj)
        {            
            //InsertTargetImage(obj);
        }

        private void InsertTargetImage(Target target)
        {
            if (target.Image == null)
                return;

            Image image = target.Image.ToBitmap();
            Figure figure = null;

            int maxWidth = (int)(schemaViewer.Width * 0.3);
            int maxHeight = (int)(schemaViewer.Height * 0.3);
            int centerX = schemaViewer.Width / 2;
            int centerY = schemaViewer.Height / 2;

            // 이미지 크기가 화면의 1/4보다 작으면 1:1크기로 표시하고, 그 이상이면 1/4크기로 크기를 제한한다.
            int width = image.Width;
            int height = image.Height;
            if (maxWidth < width)
                width = maxWidth;
            if (maxHeight < height)
                height = maxHeight;

            RotatedRect rectangle = new RotatedRect(centerX - width / 2, centerY - height / 2, width, height, 0);
            figure = new ImageFigure(image, null, rectangle);
            figure.Id = "Image";
            figure.Tag = target.FullId;
            ImageFigure imageFigure = figure as ImageFigure;
            
            FigureGroup figureGroup = new FigureGroup();
            SchemaFigure schemaFigure = new SchemaFigure();
            if (imageFigure != null)
                schemaFigure.AddFigure(imageFigure);
            schemaFigure.Tag = imageFigure.Tag;
            schemaViewer.AddFigure(schemaFigure);
            modified = true;
        }

        private void InsertImage()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                int maxWidth = (int)(schemaViewer.Width * 0.8);
                int maxHeight = (int)(schemaViewer.Height * 0.8);
                int centerX = schemaViewer.Width / 2;
                int centerY = schemaViewer.Height / 2;

                Image image = Image.FromFile(dialog.FileName);

                // 이미지 크기가 화면의 1/4보다 작으면 1:1크기로 표시하고, 그 이상이면 1/4크기로 크기를 제한한다.
                int width = image.Width;
                int height = image.Height;
                if (maxWidth < width)
                    width = maxWidth;
                if (maxHeight < height)
                    height = maxHeight;

                string extension = Path.GetExtension(dialog.FileName).TrimStart('.').ToLower();

                RotatedRect rectangle = new RotatedRect(centerX - width / 2, centerY - height / 2, width, height, 0);
                ImageFigure imageFigure = new ImageFigure(image, extension, rectangle); 

                schemaViewer.AddFigure(imageFigure);

                modified = true;
            }
        }

        

        private void InsertTarget(Target target)
        {
            int centerX = schemaViewer.Width / 2;
            int centerY = schemaViewer.Height / 2;

            TextFigure textFigure = null;
            RotatedRect rectangle = new RotatedRect((float)(centerX - shapeSize.Value/2), (float)(centerY - shapeSize.Value/2), (float)shapeSize.Value, (float)shapeSize.Value, 0);
            if (targetShape.SelectedIndex == (int)TargetShape.Standard)
            {
                Font font = (Font)schema.DefaultFigureProperty.Font.Clone();

                textFigure = new TextFigure(target.Name, new Point(centerX, centerY), font, schema.DefaultFigureProperty.TextColor);
                textFigure.Id = "text";

                rectangle = textFigure.GetRectangle();
                rectangle.Inflate(5, 5);
            }

            Figure figure = null;

            if (targetShape.SelectedIndex == (int)TargetShape.Circle)
            {
                figure = new EllipseFigure(rectangle.ToRectangleF(), (Pen)schema.DefaultFigureProperty.Pen.Clone(), (Brush)schema.DefaultFigureProperty.Brush.Clone());
                figure.Id = "rectangle";
            }
            else
            {
                figure = new RectangleFigure(rectangle, (Pen)schema.DefaultFigureProperty.Pen.Clone(), (Brush)schema.DefaultFigureProperty.Brush.Clone());
                figure.Id = "rectangle";
            }

            FigureGroup figureGroup = new FigureGroup();
            figureGroup.AddFigure(figure);
            if (textFigure != null)
                figureGroup.AddFigure(textFigure);
            figureGroup.Tag = target.FullId;
                        
            schemaViewer.AddFigure(figureGroup);

            modified = true;
        }

        private void InsertProbe(Probe probe)
        {
            int centerX = schemaViewer.Width / 2;
            int centerY = schemaViewer.Height / 2;

            Font font = (Font)schema.DefaultFigureProperty.Font.Clone();

            string probeName = probe.Name;

            TextFigure textFigure = new TextFigure(probeName, new Point(centerX, centerY), font, schema.DefaultFigureProperty.TextColor);
            textFigure.Id = "text";

            RotatedRect rectangle = textFigure.GetRectangle();
            rectangle.Inflate(5, 5);

            RectangleFigure rectangleFigure = new RectangleFigure(rectangle, (Pen)schema.DefaultFigureProperty.Pen.Clone(), (Brush)schema.DefaultFigureProperty.Brush.Clone());
            rectangleFigure.Id = "rectangle";

            SchemaFigure schemaFigure = new SchemaFigure();
            schemaFigure.AddFigure(rectangleFigure);
            schemaFigure.AddFigure(textFigure);
            schemaFigure.Tag = probe.FullId;

            schemaViewer.AddFigure(schemaFigure);

            modified = true;
        }

        private void InsertProbeResult(Probe probe, string probeResultName)
        {
            int centerX = schemaViewer.Width / 2;
            int centerY = schemaViewer.Height / 2;

            Font font = (Font)schema.DefaultFigureProperty.Font.Clone();

            TextFigure textFigure = new TextFigure(probeResultName, new Point(centerX, centerY), font, schema.DefaultFigureProperty.TextColor);
            textFigure.Id = "text";

            RotatedRect rectangle = textFigure.GetRectangle();

            centerY += (int)rectangle.Height / 2;
            TextFigure valueFigure = new TextFigure("0", new Point(centerX, centerY + (int)rectangle.Height), font, schema.DefaultFigureProperty.TextColor);
            valueFigure.Id = "value";

            RotatedRect valueRectangle = valueFigure.GetRectangle();

            rectangle = RotatedRect.Union(rectangle, valueRectangle);
            rectangle.Inflate(5, 5);

            RectangleFigure rectangleFigure = new RectangleFigure(rectangle, (Pen)schema.DefaultFigureProperty.Pen.Clone(), (Brush)schema.DefaultFigureProperty.Brush.Clone());
            rectangleFigure.Id = "rectangle";

            SchemaFigure schemaFigure = new SchemaFigure();
            schemaFigure.AddFigure(rectangleFigure);
            schemaFigure.AddFigure(textFigure);
            schemaFigure.AddFigure(valueFigure);
            schemaFigure.Tag = probe.FullId + "." + probeResultName;

            schemaViewer.AddFigure(schemaFigure);

            modified = true;
        }

        private void InsertTargetType(string targetType)
        {
            int centerX = schemaViewer.Width / 2;
            int centerY = schemaViewer.Height / 2;

            Font font = (Font)schema.DefaultFigureProperty.Font.Clone();

            TextFigure textFigure = new TextFigure(targetType, new Point(centerX, centerY), font, schema.DefaultFigureProperty.TextColor);
            textFigure.Id = "text";

            RotatedRect rectangle = textFigure.GetRectangle();

            centerY += (int)rectangle.Height / 2;
            TextFigure valueFigure = new TextFigure("0", new Point(centerX, centerY + (int)rectangle.Height), font, schema.DefaultFigureProperty.TextColor);
            valueFigure.Id = "value";

            RotatedRect valueRectangle = valueFigure.GetRectangle();

            rectangle = RotatedRect.Union(rectangle, valueRectangle);
            rectangle.Inflate(5, 5);

            RectangleFigure rectangleFigure = new RectangleFigure(rectangle, (Pen)schema.DefaultFigureProperty.Pen.Clone(), (Brush)schema.DefaultFigureProperty.Brush.Clone());
            rectangleFigure.Id = "rectangle";

            SchemaFigure schemaFigure = new SchemaFigure();
            schemaFigure.AddFigure(rectangleFigure);
            schemaFigure.AddFigure(textFigure);
            schemaFigure.AddFigure(valueFigure);
            schemaFigure.Tag = "TargetType." + targetType;

            schemaViewer.AddFigure(schemaFigure);

            modified = true;
        }

        private void ShowCameraImage(Target target)
        {
            FigureGroup figureGroup = new FigureGroup();
            target.AppendFigures(figureGroup, null);
            figureGroup.SetSelectable(false);

            cameraImage.FigureGroup = figureGroup;
            cameraImage.Invalidate();

            Bitmap bitmap = imageAcquisition.GetGrabbedImage(target.TargetGroup.GroupId).ToBitmap();
            cameraImage.UpdateImage(bitmap);
        }

        private void objectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            schemaViewer.ResetSelection();

            object obj = objectTree.SelectedNode.Tag;
            if (obj != null)
            {
                string fullId = "";
                if (obj is TargetGroup)
                {

                }
                else if (obj is Target)
                {
                    Target target = (Target)obj;
                    ShowCameraImage(target);

                    fullId = target.FullId;
                }
                else if (obj is Probe) 
                {
                    Probe probe = (Probe)obj;
                    ShowCameraImage(probe.Target);

                    fullId = probe.FullId;
                }
                else if (obj is String)
                {
                    String objectName = (String)obj;
                    if (objectName.Contains("ResultValue.") == true)
                    {
                        Probe probe = (Probe)objectTree.SelectedNode.Parent.Tag;
                        if (probe != null)
                        {
                            ShowCameraImage(probe.Target);

                            string probeResultName = (string)obj;

                            fullId = probe.FullId + "." + probeResultName;
                        }
                    }
                    else if (objectName.Contains("TargetType.") == true)
                    {
                        fullId = objectName;
                    }
                }

                if (String.IsNullOrEmpty(fullId) == false)
                {
                    List<Figure> figureList = schemaViewer.Schema.GetFigureByTag(fullId);
                    schemaViewer.SelectFigure(figureList);
                }
            }
            else
            {
                cameraImage.UpdateImage(null);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveSchema();
        }

        private void SaveSchema()
        {
//            schema.Region = new RectangleF(0, 0, schemaViewer.Width, schemaViewer.Height);
            
            model.ModelSchema = schema.Clone();
            model.ModelSchema.ViewScale = Convert.ToSingle(scale.Text) / 100;
            model.ModelSchema.AutoFit = autoFit.Checked;

            model.SaveModelSchema();

            modified = false;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in schemaViewer.Schema)
            {
                if (figure.Tag != null)
                {
                    string targetFullId = figure.Tag as string;
                    Target target = model.GetTarget(targetFullId);
                    if (target != null)
                    {
                        FigureGroup figureGroup = figure as FigureGroup;
                        if (figureGroup != null)
                        {
                            TextFigure textFigure = figureGroup.GetFigure("text") as TextFigure;
                            if (textFigure != null)
                            {
                                textFigure.Text = target.Name;

                                RectangleFigure rectangleFigure = figureGroup.GetFigure("rectangle") as RectangleFigure;
                                if (rectangleFigure != null)
                                {
                                    RotatedRect rectangle = textFigure.GetRectangle();
                                    rectangle.Inflate(5, 5);

                                    rectangleFigure.Rectangle = rectangle;
                                    rectangleFigure.FigureProperty.Brush = new SolidBrush(Color.Ivory);
                                }
                            }
                        }
                    }
                }
            }

            schemaViewer.Invalidate();

            modified = true;
        }

        private void addLineButton_Click(object sender, EventArgs e)
        {
            addFigureType = FigureType.Line;
            schemaViewer.AddFigureMode = true;
            this.Cursor = Cursors.Cross;
        }

        private void addImageButton_Click(object sender, EventArgs e)
        {
            InsertImage();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void schemaViewer_FigureMoved(List<Figure> figureList)
        {
            modified = true;
        }

        private void schemaViewer_FigureCopy(List<Figure> figureList)
        {
            schemaViewer.ResetSelection();

            foreach (Figure figure in figureList)
            {
                Figure newFigure = (Figure)figure.Clone();

                RotatedRect rectangle = figure.GetRectangle();
                newFigure.SetRectangle(rectangle);

                schemaViewer.AddFigure(newFigure);
                schemaViewer.SelectFigure(newFigure);
            }

            modified = true;
        }

        private void schemaViewer_FigureSelected(Figure figure, bool select)
        {
        }

        private void schemaViewer_AddRegionCaptured(Rectangle rectangle, Point startPos, Point endPos)
        {
            schemaViewer.AddFigureMode = false ;
            this.Cursor = Cursors.Default;

            if (rectangle.IsEmpty)
                return;

            switch (addFigureType)
            {
                case FigureType.Line:
                    schemaViewer.AddFigure(new LineFigure(startPos, endPos, new Pen(Color.Blue)));
                    break;
                case FigureType.Rectangle:
                    schemaViewer.AddFigure(new RectangleFigure(rectangle, new Pen(Color.Blue)));
                    break;
                case FigureType.Ellipse:
                    schemaViewer.AddFigure(new EllipseFigure(rectangle, new Pen(Color.Blue)));
                    break;
            }

            modified = true;
        }

        private void addCircleButton_Click(object sender, EventArgs e)
        {
            addFigureType = FigureType.Ellipse;
            schemaViewer.AddFigureMode = true;
            this.Cursor = Cursors.Cross;
        }

        private void addRectangleButton_Click(object sender, EventArgs e)
        {
            addFigureType = FigureType.Rectangle;
            schemaViewer.AddFigureMode = true;
            this.Cursor = Cursors.Cross;
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoveHelper.MouseDown(this);
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void maximizeButton_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;
            else
                WindowState = FormWindowState.Maximized;
        }

        private void ModelSchemaEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modified)
            {
                if (MessageForm.Show(this, StringManager.GetString(this.GetType().FullName, "Model Schema was changed. Do you want save the changes?"),
                                        StringManager.GetString(this.GetType().FullName, "Warning"), MessageFormType.YesNo) == DialogResult.Yes)
                {
                    SaveSchema();
                }
            }
        }

        private void ModelSchemaEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.KeyCode == Keys.C)
                {
                    Copy();
                }
                else if (e.KeyCode == Keys.V)
                {
                    Paste();
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                Delete();
            }
        }

        private void Copy()
        {
            schemaViewer.Copy();
        }

        private void Paste()
        {
            schemaViewer.Paste();
            modified = true;
        }

        private void Delete()
        {
            if (MessageForm.Show(this, StringManager.GetString(this.GetType().FullName, "Do you want to delete the figure(s)?"),
                            "SchemaEditor", MessageFormType.YesNo) == DialogResult.No)
                return;

            schemaViewer.DeleteAll();

            modified = true;
        }

        private void scale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lockUpdate)
                return;

            modified = true;
        }

        private void addTextButton_Click(object sender, EventArgs e)
        {
            InputForm form = new InputForm("Input Text");
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                Font font = (Font)schema.DefaultFigureProperty.Font.Clone();

                TextFigure textFigure = new TextFigure(form.InputText, new Point(0, 0), font, schema.DefaultFigureProperty.TextColor);
                textFigure.Tag = form.InputText;
                textFigure.FigureProperty.Alignment = StringAlignment.Near;

                schemaViewer.AddFigure(textFigure);

                modified = true;
            }
        }

        private void autoFit_CheckedChanged(object sender, EventArgs e)
        {
            if (lockUpdate)
                return;

            scale.Enabled = (autoFit.Checked == false);
            modified = true;
        }

        private void scale_TextChanged(object sender, EventArgs e)
        {
            if (lockUpdate)
                return;

            modified = true;
        }

        private void autoSchemaButton_Click(object sender, EventArgs e)
        {
            RectangleF unionRect = RectangleF.Empty;

            schema = new Schema();

            this.Enabled = false;

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            SimpleProgressForm loadingForm = new SimpleProgressForm("Generating Schema");
            loadingForm.Show(new Action(() =>
            {
                try
                {
                    for (int i = 0; i < model.InspectionStepList.Count; i++)
                    {
                        InspectionStep inspectionStep = model.InspectionStepList[i];
                        RectangleF rectangle = inspectionStep.FovRect;

                        if (unionRect == RectangleF.Empty)
                            unionRect = rectangle;
                        else
                            unionRect = RectangleF.Union(rectangle, unionRect);

                        inspectionStep.AddSchemaFigure(schema);

                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    }
                }
                catch(OperationCanceledException)
                {
                   
                }
            }), cancellationTokenSource);

            this.Enabled = true;
            loadingForm.Close();

            schema.Region = unionRect;
            schema.InvertY = true;
            schemaViewer.Schema = schema;
            modified = true;
        }
    }
}
