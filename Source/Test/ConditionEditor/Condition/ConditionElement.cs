using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConditionEditor.Condition
{
    enum Operators
    {
        Over, OverEqual, Equal, LessEqual, Less, NotEqual,
        Add, Sub, Mul, Div, Mod,
        And, Or, Not
    };

    public class ConditionManager
    {
        string[] inputs = new string[] { "Pinholes", "Noprints", "Coatings", "Sheetattacks" };
        List<List<Expression>> conditions = new List<List<Expression>>();

        public int LineCount { get => conditions.Count; }

        public Size maxSize { get => new Size(conditions.Count, conditions.Max(f => f.Count)); }

        public bool IsExistCondition(int line, int step)
        {
            if (line >= conditions.Count)
                return false;

            if (step >= conditions[line].Count)
                return false;

            return true;
        }

        public List<Expression> GetConditions(int line)
        {
            return conditions[line];
        }

        public List<Expression> AddConditions()
        {
            List<Expression> newConditionList = new List<Expression>();
            conditions.Add(newConditionList);
            return newConditionList;
        }
    }


    public class ValueOperator
    {
    }

    public abstract class Value
    {

    }

    public class NumericValue : Value
    {
        public decimal Numeric { get => numeric; set => numeric = value; }
        decimal numeric;

        public NumericValue(decimal numeric)
        {
            this.numeric = numeric;
        }

        public static implicit operator decimal(NumericValue value) => value.numeric;
    }

    public class BooleanValue : Value
    {
        public bool Boolean { get => boolean; set => boolean = value; }
        bool boolean;

        public BooleanValue(bool boolean)
        {
            this.boolean = boolean;
        }

        public static implicit operator bool(BooleanValue value) => value.boolean;
    }

    public delegate Value CalculaterDelegate(Value[] inputs);
    public class Expression
    {
        public CalculaterDelegate Calculater { set => calculater = value; }
        CalculaterDelegate calculater = null;

        public Expression()
        {
        }

        public Expression Clone()
        {
            Expression newExpression = new Expression();
            newExpression.CopyFrom(this);
            return newExpression;
        }

        public void CopyFrom(Expression from)
        {
            this.calculater = from.calculater;
        }

        public virtual Value Calculate(params Value[] inputs)
        {
            return this.calculater(inputs);
        }
    }

    //public class ExpressionGroup : Expression
    //{
    //    Expression[] expression = new Expression[2];
    //    public Value Calculate()
    //    {
    //        Value[] values = Array.ConvertAll(expression, f => f.Calculate());
    //        return this.Calculater(values);
    //    }
    //}
}