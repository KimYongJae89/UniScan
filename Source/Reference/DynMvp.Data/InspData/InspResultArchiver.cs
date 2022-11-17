using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.InspData
{
    public enum InspResultArchiverType
    {
        Text, Database
    }

    public class InspResultArchiverFactory
    {
        public InspResultArchiver Create(InspResultArchiverType type)
        {
            switch (type)
            {
                case InspResultArchiverType.Database:
                    return new DbInspResultArchiver();
                default:
                case InspResultArchiverType.Text:
                    return new TextInspResultArchiver();
            }
        }
    }

    public interface InspResultArchiver
    {
        void Save(InspectionResult inspectionResult, CancellationToken cancellationToken);
        InspectionResult Load(string dataPath);
    }
}
