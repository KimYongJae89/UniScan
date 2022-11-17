using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace DynMvp.UI
{
    public class STADialog
    {
        private CommonDialog commonDialog;

        private DialogResult result;
        public DialogResult Result
        {
            get { return result; }
        }

        public STADialog(CommonDialog commonDialog)
        {
            this.commonDialog = commonDialog;
        }

        public void ThreadProcShowDialog()
        {
            result = commonDialog.ShowDialog();
        }
    }

    public class UiHelper
    {
        [STAThreadAttribute]
        public static DialogResult ShowSTADialog(CommonDialog commonDialog)
        {
            STADialog sTAFileDialog = new STADialog(commonDialog);
            System.Threading.Thread t = new System.Threading.Thread(sTAFileDialog.ThreadProcShowDialog);
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            return sTAFileDialog.Result;
        }

        [DllImport("user32.dll", EntryPoint = "SendMessageA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private const int WM_SETREDRAW = 0xB;

        delegate void SuspendDrawingDelegate(Control target);
        public static void SuspendDrawing(Control target)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(new SuspendDrawingDelegate(SuspendDrawing), target);
                return;
            }

            if (target.IsDisposed == false)
                SendMessage(target.Handle, WM_SETREDRAW, 0, 0);
        }

        delegate void ResumeDrawingDelegate(Control target);
        public static void ResumeDrawing(Control target)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(new ResumeDrawingDelegate(ResumeDrawing), target);
                return;
            }

            if (target.IsDisposed == false)
                SendMessage(target.Handle, WM_SETREDRAW, 1, 0);

            //if (redraw)
            //{
            //    target.Refresh();
            //}
        }

        public static void AutoFontSize(Control control)
        {
            Font font;
            Graphics gp;
            Single factor, factorX, factorY;
            gp = control.CreateGraphics();
            SizeF size = gp.MeasureString(control.Text, control.Font);
            gp.Dispose();

            factorX = (control.Width) / size.Width;
            factorY = (control.Height) / size.Height;
            if (factorX > factorY)
                factor = factorY;
            else
                factor = factorX;
            font = control.Font;

            if (factor < 1)
                factor = 1;

            control.Font = new Font(font.Name, font.SizeInPoints * (factor) - 1);
        }

        public static Font AutoFontSize(Label label, String text)
        {
            Graphics gp = label.CreateGraphics();
            SizeF size = gp.MeasureString(text, label.Font);
            gp.Dispose();

            float factorX = (label.Width - label.Margin.Horizontal) / size.Width;
            float factorY = (label.Height - label.Margin.Vertical) / size.Height;

            float factor = 0;
            if (factorX > factorY)
                factor = factorY;
            else
                factor = factorX;

            return new Font(label.Font.Name, label.Font.SizeInPoints * factor, label.Font.Style);
        }

        public static void ExportCsv(DataTable dataTable)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV File (.csv)|*.csv|All Files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            try
            {
                StreamWriter csvFileWriter = new StreamWriter(dialog.FileName, false, Encoding.Default);

                string oneLine = "";
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (oneLine != "")
                        oneLine += ",";
                    oneLine += column.ColumnName;
                }
                csvFileWriter.WriteLine(oneLine);

                foreach (DataRow row in dataTable.Rows)
                {
                    oneLine = "";
                    foreach (object obj in row.ItemArray)
                    {
                        if (oneLine != "")
                            oneLine += ",";
                        oneLine += "\"" + obj.ToString() + "\"";
                    }
                    csvFileWriter.WriteLine(oneLine);
                }

                csvFileWriter.Flush();
                csvFileWriter.Close();
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());
            }
        }

        public static void ExportCsv(DataGridView dataGridView)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV File (.csv)|*.csv|All Files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            StreamWriter csvFileWriter = null;

            try
            {
                csvFileWriter = new StreamWriter(dialog.FileName, false);

                string oneLine = "";
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (oneLine != "")
                        oneLine += ",";
                    oneLine += column.HeaderText;
                }
                csvFileWriter.WriteLine(oneLine);

                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    oneLine = "";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (oneLine != "")
                            oneLine += ",";
                        oneLine += cell.Value.ToString();
                    }
                    csvFileWriter.WriteLine(oneLine);
                }

                csvFileWriter.Flush();
                csvFileWriter.Close();
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());

                if (csvFileWriter != null)
                {
                    csvFileWriter.Flush();
                    csvFileWriter.Close();
                }
            }
        }

        public static void ExportCsv(DataGridView dataGridView, string filePath)
        {
            StreamWriter csvFileWriter = null;

            try
            {
                csvFileWriter = new StreamWriter(filePath, false);

                string oneLine = "";
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (oneLine != "")
                        oneLine += ",";
                    oneLine += column.HeaderText;
                }
                csvFileWriter.WriteLine(oneLine);

                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    oneLine = "";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (oneLine != "")
                            oneLine += ",";
                        oneLine += cell.Value.ToString();
                    }
                    csvFileWriter.WriteLine(oneLine);
                }

                csvFileWriter.Flush();
                csvFileWriter.Close();
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());

                if (csvFileWriter != null)
                {
                    csvFileWriter.Flush();
                    csvFileWriter.Close();
                }
            }
        }

        public static void MoveUp(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
                return;

            int rowIndex = dataGridView.SelectedRows[0].Index;
            if (rowIndex <= 0)
                return;

            DataGridViewRow selectedRow = dataGridView.Rows[rowIndex];
            dataGridView.Rows.Remove(selectedRow);
            dataGridView.Rows.Insert(rowIndex - 1, selectedRow);
            dataGridView.ClearSelection();
            dataGridView.Rows[rowIndex - 1].Selected = true;
        }

        public static void MoveDown(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
                return;

            int rowIndex = dataGridView.SelectedRows[0].Index;
            if (rowIndex >= (dataGridView.Rows.Count - 1))
                return;

            DataGridViewRow selectedRow = dataGridView.Rows[rowIndex];
            dataGridView.Rows.Remove(selectedRow);
            dataGridView.Rows.Insert(rowIndex + 1, selectedRow);
            dataGridView.ClearSelection();
            dataGridView.Rows[rowIndex + 1].Selected = true;
        }

        delegate void SetControlVisibleDelegate(Control control, bool visible);
        public static void SetControlVisible(Control control, bool visible)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new SetControlVisibleDelegate(SetControlVisible), control, visible);
                return;
            }
            control.Visible = visible;
        }

        delegate void SetControlTextDelegate(Control control, string value);
        public static void SetControlText(Control control, string text)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new SetControlTextDelegate(SetControlText), control, text);
                return;
            }
            control.Text = text;
        }

        delegate void SetUltraProgreessBarMinMaxDelegate(Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar, int min, int max);
        public static void SetUltraProgreessBarMinMax(Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar, int min, int max)
        {
            if (ultraProgressBar.InvokeRequired)
            {
                ultraProgressBar.BeginInvoke(new SetUltraProgreessBarMinMaxDelegate(SetUltraProgreessBarMinMax), ultraProgressBar, min, max);
                return;
            }

            if (ultraProgressBar.Value > max)
            {
                ultraProgressBar.Minimum = min;
                ultraProgressBar.Value = max;
                ultraProgressBar.Maximum = max;
            }
            else if (ultraProgressBar.Value < min)
            {
                ultraProgressBar.Maximum = max;
                ultraProgressBar.Value = min;
                ultraProgressBar.Minimum = min;
            }
            else
            {
                ultraProgressBar.Minimum = min;
                ultraProgressBar.Maximum = max;
            }
        }

        delegate void SetUltraProgreessBarValueDelegate(Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar, int value, bool showFraction);
        public static void SetUltraProgreessBarValue(Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar, int value, bool showFraction)
        {
            if (ultraProgressBar.InvokeRequired)
            {
                ultraProgressBar.BeginInvoke(new SetUltraProgreessBarValueDelegate(SetUltraProgreessBarValue), ultraProgressBar, value, showFraction);
                return;
            }

            ultraProgressBar.Value = Math.Min(Math.Max(ultraProgressBar.Minimum, value), ultraProgressBar.Maximum);

            float rate = 0;
            int valueRange = (value - ultraProgressBar.Minimum);
            int wholeRange = (ultraProgressBar.Maximum - ultraProgressBar.Minimum);
            if (valueRange >= 0 && wholeRange > 0)
                rate = (valueRange) * 100.0f / (wholeRange);

            if (showFraction)
                ultraProgressBar.Text = $"{rate:0.0}% ( {valueRange} / {wholeRange} )";
            else
                ultraProgressBar.Text = $"{rate:0.0}%";
        }

        delegate void SetPictureboxImageDelegate(PictureBox pictureBox, Bitmap bitmap);
        public static void SetPictureboxImage(PictureBox pictureBox, Bitmap bitmap)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.BeginInvoke(new SetPictureboxImageDelegate(SetPictureboxImage), pictureBox, bitmap);
                return;
            }
            pictureBox.Image = bitmap;
        }

        delegate bool GetCheckBoxCheckedDelegate(CheckBox checkBox);
        public static bool GetCheckBoxChecked(CheckBox checkBox)
        {
            if (checkBox.InvokeRequired)
            {
                return (bool)checkBox.Invoke(new GetCheckBoxCheckedDelegate(GetCheckBoxChecked), checkBox);
            }
            return checkBox.Checked;
        }

        delegate void SetCheckBoxCheckedDelegate(CheckBox checkBox, bool value);
        public static void SetCheckBoxChecked(CheckBox checkBox, bool value)
        {
            if (checkBox.InvokeRequired)
            {
                checkBox.BeginInvoke(new SetCheckBoxCheckedDelegate(SetCheckBoxChecked), checkBox, value);
                return;
            }
            checkBox.Checked = value;
        }

        delegate int GetComboBoxSelectedIndexDelegate(ComboBox comboBox);
        public static int GetComboBoxSelectedIndex(ComboBox comboBox)
        {
            if (comboBox.InvokeRequired)
            {
                return (int)comboBox.Invoke(new GetComboBoxSelectedIndexDelegate(GetComboBoxSelectedIndex), comboBox);
            }

            return comboBox.SelectedIndex;
        }

        delegate void SetComboBoxSelectedIndexDelegate(ComboBox comboBox, int selectedIndex);
        public static void SetComboBoxSelectedIndex(ComboBox comboBox, int selectedIndex)
        {
            if (comboBox.InvokeRequired)
            {
                comboBox.BeginInvoke(new SetComboBoxSelectedIndexDelegate(SetComboBoxSelectedIndex), comboBox, selectedIndex);
                return;
            }

            comboBox.SelectedIndex = selectedIndex;
        }

        public static void SetNumericValue(NumericUpDown numericUpDown, int value) => SetNumericValue(numericUpDown, (decimal)value);
        public static void SetNumericValue(NumericUpDown numericUpDown, float value) => SetNumericValue(numericUpDown, (decimal)value);

        delegate void SetNumericValueDelegate(NumericUpDown numericUpDown, decimal value);
        public static void SetNumericValue(NumericUpDown numericUpDown, decimal value)
        {
            if (numericUpDown.InvokeRequired)
            {
                numericUpDown.BeginInvoke(new SetNumericValueDelegate(SetNumericValue), numericUpDown, value);
                return;
            }

            numericUpDown.Value = Base.MathHelper.Clipping(value, numericUpDown.Minimum, numericUpDown.Maximum);
        }

        delegate decimal GetNumericValueDelegate(NumericUpDown numericUpDown);
        public static decimal GetNumericValue(NumericUpDown numericUpDown)
        {
            if (numericUpDown.InvokeRequired)
            {
                return (decimal)numericUpDown.Invoke(new GetNumericValueDelegate(GetNumericValue), numericUpDown);
            }

            return numericUpDown.Value;
        }

        delegate void SetNumericMinMaxDelegate(NumericUpDown numericUpDown, decimal min, decimal max);
        public static void SetNumericMinMax(NumericUpDown numericUpDown, decimal min, decimal max)
        {
            if (numericUpDown.InvokeRequired)
            {
                numericUpDown.BeginInvoke(new SetNumericMinMaxDelegate(SetNumericMinMax), numericUpDown, min, max);
                return;
            }

            decimal value = Base.MathHelper.Clipping(numericUpDown.Value, min, max);
            numericUpDown.Value = value;
            numericUpDown.Minimum = min;
            numericUpDown.Maximum = max;
        }

        delegate void SetProgressBarMinMaxDelegate(ProgressBar progressBar, int min, int max);
        public static void SetProgressBarMinMax(ProgressBar progressBar, int min, int max)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.BeginInvoke(new SetProgressBarMinMaxDelegate(SetProgressBarMinMax), progressBar, min, max);
                return;
            }

            progressBar.Minimum = min;
            progressBar.Maximum = max;
        }

        delegate void SetProgressBarValueDelegate(ProgressBar progressBar, int value);
        public static void SetProgressBarValue(ProgressBar progressBar, int value)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.BeginInvoke(new SetProgressBarValueDelegate(SetProgressBarValue), progressBar, value);
                return;
            }

            progressBar.Value = Base.MathHelper.Clipping(value, progressBar.Minimum, progressBar.Maximum);
            progressBar.Invalidate();

        }

        public static void SetAttributeValue(PropertyDescriptor descriptor, Type attributeType, string fieldName, object value)
        {
            System.Reflection.FieldInfo fieldInfo = attributeType.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Attribute attrib = descriptor.Attributes[attributeType];
            fieldInfo.SetValue(attrib, value);
        }

        public static void SetBrowsableAttributeValue(PropertyDescriptor descriptor, bool browsable)
        {
            SetAttributeValue(descriptor, typeof(BrowsableAttribute), "browsable", browsable);
            //BrowsableAttribute attrib = (BrowsableAttribute)descriptor.Attributes[typeof(BrowsableAttribute)];
            //System.Reflection.FieldInfo fieldInfo = typeof(BrowsableAttribute).GetField("browsable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //fieldInfo.SetValue(attrib, browsable);
        }
    }
}
