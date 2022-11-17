using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConditionEditor.Condition
{
    public partial class ConditionEditForm : Form
    {
        Expression prevCondition = null;
        Expression curCondition = null;

        public Expression Condition { get => this.condition; }
        Expression condition = null;

        public ConditionEditForm()
        {
            InitializeComponent();
        }

        internal void Initialize(Expression prevCondition, Expression curCondition)
        {
            this.prevCondition = prevCondition;
            this.curCondition = curCondition;
            this.condition = curCondition?.Clone();
        }
    }
}
