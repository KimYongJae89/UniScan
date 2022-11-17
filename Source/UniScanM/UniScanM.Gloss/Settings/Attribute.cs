using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.Gloss.Settings
{
    public class Attribute
    {
        internal class LocalizedCategoryAttributeGloss : LocalizedCategoryAttribute
        {
            public LocalizedCategoryAttributeGloss(string value) : base("LocalizedCategoryAttributeGloss", value)
            {
            }
        }

        internal class LocalizedDisplayNameAttributeGloss : LocalizedDisplayNameAttribute
        {
            public LocalizedDisplayNameAttributeGloss(string value) : base("LocalizedDisplayNameAttributeGloss", value)
            {
            }
        }

        internal class LocalizedDescriptionAttributeGloss : LocalizedDescriptionAttribute
        {
            public LocalizedDescriptionAttributeGloss(string value) : base("LocalizedDescriptionAttributeGloss", value)
            {
            }
        }
    }
}
