using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.UI.EditorAttribute
{
    //public class EditorPath : UITypeEditor
    //{
    //    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    //    {
    //        return UITypeEditorEditStyle.Modal;
    //    }

    //    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    //    {
    //        using (FolderBrowserDialog dlg = new FolderBrowserDialog())
    //        {
    //            dlg.SelectedPath = (string)value;
    //            if (dlg.ShowDialog() == DialogResult.OK)
    //                return dlg.SelectedPath;
    //        }

    //        return base.EditValue(context, provider, value);
    //    }
    //}

    public class MyFileNameEditor : UITypeEditor
    {
        public string Filter { get; set; } = "All Files(*.*)|*.*";

        public MyFileNameEditor() { }

        public MyFileNameEditor(string filter)
        {
            this.Filter = string.Join("|", filter, this.Filter);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.FileName = (string)value;
                dlg.Filter = this.Filter;

                if (dlg.ShowDialog() == DialogResult.OK)
                    return dlg.FileName;

                return value;
            }
        }
    }

    public class XmlFileNameEditor : MyFileNameEditor
    {
        public XmlFileNameEditor() : base("XML Files(*.xml)|*.xml") { }
    }

}
