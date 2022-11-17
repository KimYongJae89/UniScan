using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StringManager
{
    class MyTreeNode: TreeNode
    {
        public void Update()
        {
            if(this.Tag is StringTable)
            {

            }
            else if(this.Tag is KeyValuePair<string,string>)
            {

            }
        }
    }
}
