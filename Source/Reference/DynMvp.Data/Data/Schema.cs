using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

using DynMvp.Base;
using DynMvp.UI;
using System.Drawing.Drawing2D;

namespace DynMvp.Data
{
	public enum SchemaFigureType
	{
		Value, Judgment
	}

	public class SchemaFigure : FigureGroup
	{
		SchemaFigureType schemaFigureType;
		public SchemaFigureType SchemaFigureType
		{
			get { return schemaFigureType; }
			set { schemaFigureType = value; }
		}
	}

	public class Schema
	{
        FigureProperty defaultFigureProperty = new FigureProperty();
        public FigureProperty DefaultFigureProperty
        {
            get { return defaultFigureProperty; }
            set { defaultFigureProperty = value; }
        }

        private RectangleF region;
		public RectangleF Region
        {
			get { return region; }
			set { region = value; }
		}

        private bool invertY = false;
        public bool InvertY
        {
            get { return invertY; }
            set { invertY = value; }
        }

        private bool autoFit = false;
        public bool AutoFit
        {
            get { return autoFit; }
            set { autoFit = value; }
        }

        private float viewScale = 1;
        public float ViewScale
        {
            get { return viewScale; }
            set { viewScale = value; }
        }

		private FigureGroup figureGroup = new FigureGroup();
		public FigureGroup FigureGroup
		{
			get { return figureGroup; }
			set { figureGroup = value; }
		}

        private FigureGroup tempFigureGroup = null;
        public FigureGroup TempFigureGroup
        {
            get { return tempFigureGroup; }
            set { tempFigureGroup = value; }
        }

        private Color backColor;
        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }


        public IEnumerator<Figure> GetEnumerator()
		{
			return figureGroup.GetEnumerator();
		}

		public Schema Clone()
		{
			Schema schema = new Schema();

			schema.Region = new RectangleF(region.X, region.Y, region.Width, region.Height);
            schema.ViewScale = viewScale;
            schema.AutoFit = autoFit;
            schema.BackColor = backColor;
            schema.InvertY = invertY;

            schema.figureGroup = (FigureGroup)figureGroup.Clone();

			return schema;
		}

		public void ResetTempProperty()
		{
			figureGroup.ResetTempProperty();
		}

		public void AddFigure(Figure figure)
		{
			figureGroup.AddFigure(figure);
			figure.Id = CreateFigureId();
		}

		public void RemoveFigure(Figure figure)
		{
			figureGroup.RemoveFigure(figure);
		}

		private string CreateFigureId()
		{
			string newFigureId;
			for (int i = 1; i < int.MaxValue; i++)
			{
				newFigureId = String.Format("figure{0}", i);
				if (GetFigure(newFigureId) == null)
					return newFigureId;
			}

			throw new TooManyItemsException();
		}

		public Figure GetFigure(string id)
		{
			foreach (Figure figure in figureGroup)
			{
				if (figure.Id == id)
					return figure;
			}

			return null;
		}

		public List<Figure> GetFigureByTag(string tag, bool wildSearch = false)
		{
			List<Figure> figureList = new List<Figure>();

			foreach (Figure figure in figureGroup)
			{
				string figureString = figure.Tag as string;

				if (wildSearch == true)
				{
					if (figureString.Contains(tag))
					{
						figureList.Add(figure);
					}
				}
				else
				{
					if (figureString == tag)
					{
						figureList.Add(figure);
					}
				}
			}

			return figureList;
		}

		public Figure GetFigure(Point point)
		{
			return figureGroup.GetFigure(point);
		}

		public void MoveUp(Figure figure)
		{
			figureGroup.MoveUp(figure);
		}

		public void MoveTop(Figure figure)
		{
			figureGroup.MoveTop(figure);
		}

		public void MoveDown(Figure figure)
		{
			figureGroup.MoveDown(figure);
		}

		public void MoveBottom(Figure figure)
		{
			figureGroup.MoveBottom(figure);
		}

		public void Draw(Graphics g, CoordTransformer coordTransformer, bool editable)
		{
			figureGroup.Draw(g, coordTransformer, editable);
            if (tempFigureGroup != null)
            {
                tempFigureGroup.Draw(g, coordTransformer, editable);
                tempFigureGroup = null;
            }
        }

        public void Load(string fileName)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(fileName);

            XmlElement schemaElement = xmlDocument.DocumentElement;

            region.X = Convert.ToSingle(XmlHelper.GetValue(schemaElement, "X", "0"));
            region.Y = Convert.ToSingle(XmlHelper.GetValue(schemaElement, "Y", "0"));
            region.Width = Convert.ToSingle(XmlHelper.GetValue(schemaElement, "Width", "0"));
            region.Height = Convert.ToSingle(XmlHelper.GetValue(schemaElement, "Height", "0"));
            viewScale = Convert.ToSingle(XmlHelper.GetValue(schemaElement, "ViewScale", "1"));
            autoFit = Convert.ToBoolean(XmlHelper.GetValue(schemaElement, "AutoFit", "False"));
            invertY = Convert.ToBoolean(XmlHelper.GetValue(schemaElement, "InvertY", "False"));

            defaultFigureProperty.Load(schemaElement);

			figureGroup = new FigureGroup();
            figureGroup.Load(schemaElement["FigureGroup"]);
		}

		public bool Save(string fileName)
		{
            try
            {
                XmlDocument xmlDocument = new XmlDocument();

                XmlElement schemaElement = xmlDocument.CreateElement("", "Schema", "");
                xmlDocument.AppendChild(schemaElement);

                XmlHelper.SetValue(schemaElement, "X", region.X.ToString());
                XmlHelper.SetValue(schemaElement, "Y", region.Y.ToString());
                XmlHelper.SetValue(schemaElement, "Width", region.Width.ToString());
                XmlHelper.SetValue(schemaElement, "Height", region.Height.ToString());
                XmlHelper.SetValue(schemaElement, "ViewScale", viewScale.ToString());
                XmlHelper.SetValue(schemaElement, "AutoFit", autoFit.ToString());
                XmlHelper.SetValue(schemaElement, "InvertY", invertY.ToString());

                defaultFigureProperty.Save(schemaElement);

                XmlHelper.SetValue(schemaElement, "BackColor", backColor);

                XmlElement figureGroupElement = xmlDocument.CreateElement("", "FigureGroup", "");
                schemaElement.AppendChild(figureGroupElement);

                figureGroup.Save(figureGroupElement);

                xmlDocument.Save(fileName);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("Schema::Save Fail : {0}", ex.Message));
                return false;
            }
        }
    }
}
