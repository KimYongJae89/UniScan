using System;
using System.IO;
using System.Xml;
using DynMvp.Data;
using DynMvp.UI;

namespace UniScan.Data
{
    class Model : DynMvp.Data.Model
    {
        //    public string Name
        //    {
        //        get { return ModelDescription.Name; }
        //    }

        //    private Production production = new Production();
        //    public Production Production
        //    {
        //        get { return production; }
        //    }

        //    ModelDescription modelDescription;
        //    public DynMvp.Data.ModelDescription ModelDescription
        //    {
        //        get { return modelDescription; }
        //        set { modelDescription = (ModelDescription)value; }
        //    }

        //    bool modified;
        //    public bool Modified
        //    {
        //        get { return modified; }
        //        set { modified = value; }
        //    }

        //    private string modelPath;
        //    public string ModelPath
        //    {
        //        get { return modelPath; }
        //        set { modelPath = Path.GetFullPath(value); }
        //    }

        //    public void Clear()
        //    {

        //    }

        //    public bool IsEmpty()
        //    {
        //        return true;
        //    }

        //public override bool LoadModel(IReportProgress reportProgress, CreateCustomInfoDelegate CreateCustomInfo = null)
        //{
        //    return base.LoadModel(reportProgress, CreateCustomInfo);
        //}

        //public override void SaveModel(IReportProgress reportProgress = null)
        //{
        //    base.SaveModel(reportProgress);
        //}

        //    public void CloseModel()
        //    {

        //    }

        //    public void LoadProduction()
        //    {
        //        string filePath = String.Format("{0}\\Production.xml", modelPath);
        //        if (File.Exists(filePath))
        //        {
        //            production.Load(filePath);
        //        }
        //    }

        //    public void SaveProduction()
        //    {
        //        string filePath = String.Format("{0}\\Production.xml", modelPath);
        //        if (File.Exists(filePath))
        //        {
        //            production.Save(filePath);
        //        }
        //    }

        //    internal bool IsTaught()
        //    {
        //        return true;
        //    }
    }
}
