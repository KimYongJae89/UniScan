using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Xml;
using DynMvp.Base;

namespace DynMvp.Vision
{
    public class AlgorithmPool
    {
        public static AlgorithmPool Instance()
        {
            if (_instance == null)
            {
                _instance = new AlgorithmPool();
            }

            return _instance;
        }

        List<Algorithm> algorithmList = new List<Algorithm>();

        static AlgorithmPool _instance = null;
        
        AlgorithmArchiver algorithmArchiver = null;
        
        public void Dispose()
        {
            foreach (Algorithm algorithm in algorithmList)
                algorithm.Dispose();

            algorithmList.Clear();
        }

        public virtual void Initialize(AlgorithmArchiver algorithmArchiver)
        {
            this.algorithmArchiver = algorithmArchiver;
        }

        public void BuildAlgorithmPool()
        {
            algorithmList.Clear();

            if (algorithmArchiver != null)
                algorithmList.AddRange(this.algorithmArchiver.GetDefaultAlgorithm());
        }

        public Algorithm GetAlgorithm(string algorithmType)
        {
            return algorithmList.Find(algorithm => { return algorithm.GetAlgorithmType() == algorithmType; });
        }

        public void RemoveAlgorithm(Algorithm algorithm)
        {
            algorithmList.Remove(algorithm);
        }

        public void Clear()
        {
            algorithmList.Clear();
        }

        public void Load(string fileName)
        {
            algorithmList.Clear();
            algorithmList.AddRange(this.algorithmArchiver.GetDefaultAlgorithm());

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
                return;

            XmlElement algorithmPoolElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = algorithmPoolElement.GetElementsByTagName("Algorithm");
            foreach (XmlElement algorithmElement in xmlNodeList)
            {
                Algorithm algorithm = null;
                algorithmArchiver?.LoadAlgorithm(algorithmElement, out algorithm);

                if (algorithm != null)
                {
                    Algorithm oldAlgorithm = GetAlgorithm(algorithm.GetAlgorithmType());
                    if (oldAlgorithm != null)
                        RemoveAlgorithm(oldAlgorithm);

                    algorithmList.Add(algorithm);
                    algorithm.IsAlgorithmPoolItem = true;
                }
            }
        }

        public void Save(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement algorithmPoolElement = xmlDocument.CreateElement("", "AlgorithmPool", "");
            xmlDocument.AppendChild(algorithmPoolElement);

            algorithmList.ForEach(algorithm =>
            {
                XmlElement algorithmElement = algorithmPoolElement.OwnerDocument.CreateElement("", "Algorithm", "");
                algorithmPoolElement.AppendChild(algorithmElement);

                AlgorithmArchiver.SaveAlgorithm(algorithmElement, algorithm, true);
            }
            );

            XmlHelper.Save(xmlDocument, fileName);
        }
    }
}
