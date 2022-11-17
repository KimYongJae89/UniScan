using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Cad.Gerber
{
    enum SectionType
    {
        Unknown, MSYSHEADER, BOARD, ARRAY, FIDUCIAL, PATTERN, PAD, FOV, PATTERNEDGE, END
    }

    delegate void ParseLineDelegate(string line);

    public class GerberLoader : CadLoader
    {
        float unitScale = 1000;

        GerberData gerberData;
        public GerberData GerberData
        {
            get { return gerberData; }
        }

        ParseLineDelegate ParseLine = null;

        float GetUnitFactor()
        {
            if (gerberData.Unit == Unit.Inch)
                return 2450;
            else  // MM
                return 1000;
        }

        public override bool Load(string fileName)
        {
            gerberData = new GerberData();

            string[] lines = File.ReadAllLines(fileName, Encoding.Default);

            SectionType sectionType = SectionType.Unknown;

            foreach (string line in lines)
            {
                if (String.IsNullOrEmpty(line))
                    continue;

                if (line[0] == '@')
                {
                    sectionType = (SectionType)Enum.Parse(typeof(SectionType), line.Substring(1));
                    switch(sectionType)
                    {
                        case SectionType.MSYSHEADER:    ParseLine = ParseLineHeader;        break;
                        case SectionType.BOARD:         ParseLine = ParseLineBoard;         break;
                        case SectionType.ARRAY:         ParseLine = ParseLineArray;         break;
                        case SectionType.FIDUCIAL:      ParseLine = ParseLineFiducial;      break;
                        case SectionType.PATTERN:       ParseLine = ParseLinePattern;       break;
                        case SectionType.PAD:           ParseLine = ParseLinePad;           break;
                        case SectionType.FOV:           ParseLine = ParseLineFov;           break;
                        case SectionType.PATTERNEDGE:   ParseLine = ParseLinePatternEdge;   break;
                    }
                }
                else
                {
                    if (ParseLine != null)
                        ParseLine(line);
                }
            }

            return true;
        }

        private void ParseLineHeader(string line)
        {
            string[] tokens = line.Split('=');
            if (tokens.Count() != 2)
                return;

            switch(tokens[0])
            {
                case "Version":     gerberData.Version = Convert.ToInt32(tokens[1]); break;
                case "Unit":
                    gerberData.Unit = (Unit)Enum.Parse(typeof(Unit), tokens[1]);
                    if (gerberData.Unit == Unit.Inch)
                        unitScale = 2450;
                    else  // MM
                        unitScale = 1000;
                    break;
                case "Coordinate":  gerberData.Coornidate = (Coornidate)Enum.Parse(typeof(Coornidate), tokens[1]); break;
                case "OffsetX":     gerberData.OffsetX = Convert.ToSingle(tokens[1]); break;
                case "OffsetY":     gerberData.OffsetY = Convert.ToSingle(tokens[1]); break;
                case "OffsetXFromRightBottom": gerberData.OffsetXFromRightBottom = Convert.ToSingle(tokens[1]); break;
                case "OffsetYFromRightBottom": gerberData.OffsetYFromRightBottom = Convert.ToSingle(tokens[1]); break;
                case "CombineArray": gerberData.CombineArray = Convert.ToInt32(tokens[1]) == 1; break;
                case "Arrays":      gerberData.NumModule = Convert.ToInt32(tokens[1]); break;
                case "Fiducials":   gerberData.NumFiducial = Convert.ToInt32(tokens[1]); break;
                case "Patterns":    gerberData.NumPattern = Convert.ToInt32(tokens[1]); break;
                case "Pads":        gerberData.NumPad = Convert.ToInt32(tokens[1]); break;
                case "Fovs":        gerberData.NumFov = Convert.ToInt32(tokens[1]); break;
            }
        }

        private void ParseLineBoard(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 2)
                return;

            float boardSizeX = Convert.ToSingle(tokens[0]) * unitScale;
            float boardSizeY = Convert.ToSingle(tokens[1]) * unitScale;

            gerberData.BoardSize = new SizeF(boardSizeX, boardSizeY);
        }

        private void ParseLineArray(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 4)
                return;

            int moduleNo = Convert.ToInt32(tokens[0]);
            float posX = Convert.ToSingle(tokens[1]) * unitScale;
            float posY = Convert.ToSingle(tokens[2]) * unitScale;
            int angle = Convert.ToInt32(tokens[3]);

            Module module = new Module(moduleNo, posX, posY, angle);
            gerberData.AddModule(module);
        }

        private FiducialType GetFiducialType(string typeStr)
        {
            switch (typeStr)
            {
                default:
                case "G": return FiducialType.Global;
                case "A": return FiducialType.Module;
                case "L": return FiducialType.Local;
            }
        }

        private FigureShape GetFigureShape(string shapeStr)
        {
            switch (shapeStr)
            {
                default:
                case "R": return FigureShape.Rectangle;
                case "C": return FigureShape.Circle;
                case "U": return FigureShape.Undifined;
                case "O": return FigureShape.Oblong;
                case "S": return FigureShape.Sloped;
            }
        }

        private void ParseLineFiducial(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 11)
                return;

            int fidNo = Convert.ToInt32(tokens[0]);
            FiducialType fiducialType = GetFiducialType(tokens[1]);
            FigureShape fiducialShape = GetFigureShape(tokens[2]);
            float posX = Convert.ToSingle(tokens[3]) * unitScale;
            float posY = Convert.ToSingle(tokens[4]) * unitScale;
            float width = Convert.ToSingle(tokens[5]) * unitScale;
            float height = Convert.ToSingle(tokens[6]) * unitScale;
            float offsetX = Convert.ToSingle(tokens[7]) * unitScale;
            float offsetY = Convert.ToSingle(tokens[8]) * unitScale;
            string refCode = tokens[9];
            int moduleNo = Convert.ToInt32(tokens[10]);

            Fiducial fiducial = new Fiducial(fidNo, fiducialType, fiducialShape, posX, posY, width, height, offsetX, offsetY, refCode, moduleNo);
            gerberData.AddFiducial(fiducial);
        }

        private void ParseLinePattern(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 8)
                return;

            int patternNo = Convert.ToInt32(tokens[0]);
            FigureShape patternShape = GetFigureShape(tokens[1]);
            float width = Convert.ToSingle(tokens[2]) * unitScale;
            float height = Convert.ToSingle(tokens[3]) * unitScale;
            float centroidX = Convert.ToSingle(tokens[4]) * unitScale;
            float centroidY = Convert.ToSingle(tokens[5]) * unitScale;
            float area = Convert.ToSingle(tokens[6]) * unitScale * unitScale;
            float angle = Convert.ToSingle(tokens[7]);

            Pattern pattern = new Pattern(patternNo, patternShape, width, height, centroidX, centroidY, area, angle);
            gerberData.AddPattern(pattern);
        }

        private void ParseLinePad(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 12)
                return;

            int padNo = Convert.ToInt32(tokens[0]);
            int patternNo = Convert.ToInt32(tokens[1]);
            float posX = Convert.ToSingle(tokens[2]) * unitScale;
            float posY = Convert.ToSingle(tokens[3]) * unitScale;
            float left = Convert.ToSingle(tokens[4]) * unitScale;
            float top = Convert.ToSingle(tokens[5]) * unitScale;
            float right = Convert.ToSingle(tokens[6]) * unitScale;
            float bottom = Convert.ToSingle(tokens[7]) * unitScale;
            string refCode = tokens[8].Trim().Trim('\"');
            int pinNo = Convert.ToInt32(tokens[9]);
            int moduleNo = Convert.ToInt32(tokens[10]);
            int fovNo = Convert.ToInt32(tokens[11]);

            Pad pad = new Pad(padNo, patternNo, posX, posY, left, top, right, bottom, refCode, pinNo, moduleNo, fovNo);
            gerberData.AddPad(pad);
        }

        private void ParseLineFov(string line)
        {
            string[] tokens = line.Split('\t');
            if (tokens.Count() != 3)
                return;

            int fovNo = Convert.ToInt32(tokens[0]);
            float posX = Convert.ToSingle(tokens[1]) * unitScale;
            float posY = Convert.ToSingle(tokens[2]) * unitScale;

            Fov fov = new Fov(fovNo, posX, posY);
            gerberData.AddFov(fov);
        }

        private void ParseLinePatternEdge(string line)
        {
            /*
            string[] tokens = line.Split('\t');
            if (tokens.Count() == 2 && tokens[0] == "Pattern")
            {
                int patternNo = Convert.ToInt32(tokens[1]);
                PatternEdge patternEdge = new PatternEdge(patternNo);

                gerberData.AddPatternEdge(patternEdge);
            }
            else if (tokens.Count() == 3)
            {
                PatternEdge patternEdge = gerberData.GetLastPatternEdge();
                if (patternEdge != null)
                {
                    char drawType = tokens[0][0];
                    float posX = Convert.ToSingle(tokens[1]) * unitScale;
                    float posY = Convert.ToSingle(tokens[2]) * unitScale;

                    if (drawType == 'M')
                        patternEdge.AddNewList(new PointF(posX, posY));
                    else
                        patternEdge.AddPoint(new PointF(posX, posY));
                }
            }
            */
            int index = line.IndexOf("Pattern");
            if (index > -1)
            {
                int patternNo = Convert.ToInt32(line.Substring(7));
                PatternEdge patternEdge = new PatternEdge(patternNo);

                gerberData.AddPatternEdge(patternEdge);
            }
            else if (index == -1)
            {
                string[] tokens = line.Split('\t');
                if (tokens.Count() == 3)
                {
                    PatternEdge patternEdge = gerberData.GetLastPatternEdge();
                    if (patternEdge != null)
                    {
                        char drawType = tokens[0][0];
                        float posX = Convert.ToSingle(tokens[1]) * unitScale;
                        float posY = Convert.ToSingle(tokens[2]) * unitScale;

                        if (drawType == 'M')
                            patternEdge.AddNewList(new PointF(posX, posY));
                        else
                            patternEdge.AddPoint(new PointF(posX, posY));
                    }
                }
            }

        }
    }
}
