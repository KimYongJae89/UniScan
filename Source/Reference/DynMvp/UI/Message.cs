using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.UI
{
    public abstract class MessageElement
    {
        public abstract MessageElement Clone();
    }

    public class TextElement : MessageElement
    {
        Font font;
        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public TextElement(string text, Font font)
        {
            this.text = text;
            this.font = font;
        }

        public override MessageElement Clone()
        {
            return new TextElement(text, font);
        }
    }

    public class TextBlockElement : MessageElement
    {
        List<TextElement> elementList = new List<TextElement>();
        public List<TextElement> ElementList
        {
            get { return elementList; }
            set { elementList = value; }
        }

        public override MessageElement Clone()
        {
            TextBlockElement textBlockElement = new TextBlockElement();
            foreach (TextElement element in elementList)
                textBlockElement.ElementList.Add((TextElement)element.Clone());

            return textBlockElement;
        }
    }

    public class TableCellElement : MessageElement
    {
        TextBlockElement textBlockElement = new TextBlockElement();
        public TextBlockElement TextBlockElement
        {
            get { return textBlockElement; }
            set { textBlockElement = value; }
        }

        int colSpan;
        public int ColSpan
        {
            get { return colSpan; }
            set { colSpan = value; }
        }

        int rowSpan;
        public int RowSpan
        {
            get { return rowSpan; }
            set { rowSpan = value; }
        }

        Color backgroundColor = Color.Transparent;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public TableCellElement()
        {
        }

        public TableCellElement(string text, Font font, Color backgroundColor)
        {
            this.backgroundColor = backgroundColor;
            textBlockElement.ElementList.Add(new TextElement(text, font));
        }

        public override MessageElement Clone()
        {
            TableCellElement cellElement = new TableCellElement();
            cellElement.ColSpan = colSpan;
            cellElement.RowSpan = rowSpan;
            cellElement.BackgroundColor = backgroundColor;
            cellElement.TextBlockElement = (TextBlockElement)textBlockElement.Clone();

            return cellElement;
        }
    }

    public class TableRowElement : MessageElement
    {
        List<TableCellElement> elementList = new List<TableCellElement>();
        public List<TableCellElement> ElementList
        {
            get { return elementList; }
            set { elementList = value; }
        }

        public override MessageElement Clone()
        {
            TableRowElement rowElement = new TableRowElement();
            foreach (TableCellElement element in elementList)
                rowElement.ElementList.Add((TableCellElement)element.Clone());

            return rowElement;
        }
    }

    public class TableElement : MessageElement
    {
        List<TableRowElement> elementList = new List<TableRowElement>();
        public List<TableRowElement> ElementList
        {
            get { return elementList; }
            set { elementList = value; }
        }

        public override MessageElement Clone()
        {
            TableElement tableElement = new TableElement();
            foreach (TableRowElement element in elementList)
                tableElement.ElementList.Add((TableRowElement)element.Clone());

            return tableElement;
        }
    }

    public class MessageBuilder
    {
        List<MessageElement> elementList = new List<MessageElement>();
        public List<MessageElement> ElementList
        {
            get { return elementList; }
            set { elementList = value; }
        }

        TableElement tableElement;
        Font tableFont;

        public MessageBuilder Clone()
        {
            MessageBuilder cloneMessage = new MessageBuilder();

            foreach (MessageElement element in elementList)
                cloneMessage.ElementList.Add(element.Clone());

            return cloneMessage;
        }

        public void Append(MessageBuilder message)
        {
            foreach (MessageElement element in message.ElementList)
                elementList.Add(element.Clone());
        }

        public void AddLine()
        {
            elementList.Add(new TextElement("\n", null));
        }

        public void AddText(string text, Font font = null)
        {
            elementList.Add(new TextElement(text, font));
        }

        public void AddTextLine(string text, Font font = null)
        {
            elementList.Add(new TextElement(text, font));
            elementList.Add(new TextElement("\n", null));
        }

        public void AddTextLine(List<string> textList, Font font = null)
        {
            foreach (string text in textList)
            {
                elementList.Add(new TextElement(text, font));
                elementList.Add(new TextElement("\n", null));
            }
        }

        public void BeginTable(Font font, params string[] headerText)
        {
            tableElement = new TableElement();
            elementList.Add(tableElement);
            tableFont = font;

            AddTableRow(headerText);
        }

        public void AddTableRow(params string[] cellTexts)
        {
            AddTableRow(tableFont, Color.Transparent, cellTexts);
        }

        public void AddTableRow(Color backgroundColor, params string[] cellTexts)
        {
            AddTableRow(tableFont, backgroundColor, cellTexts);
        }

        public void AddTableRow(Font font, Color backgroundColor, params string[] cellTexts)
        {
            if (tableElement == null)
                return;

            TableRowElement tableRowElement = new TableRowElement();

            foreach (string cellText in cellTexts)
            {
                tableRowElement.ElementList.Add(new TableCellElement(cellText, font, backgroundColor));
            }

            tableElement.ElementList.Add(tableRowElement);
        }

        public void EndTable()
        {
            tableElement = null;
        }
    }
}
