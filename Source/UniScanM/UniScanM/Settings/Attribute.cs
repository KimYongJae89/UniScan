using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.Settings
{
    internal class LocalizedCategoryAttributeUniScanM : LocalizedCategoryAttribute
    {
        public LocalizedCategoryAttributeUniScanM(string value) : base("LocalizedCategoryAttributeUniScanM", value)
        {
        }
    }

    internal class LocalizedDisplayNameAttributeUniScanM : LocalizedDisplayNameAttribute
    {
        public LocalizedDisplayNameAttributeUniScanM(string value) : base("LocalizedDisplayNameAttributeUniScanM", value)
        {
        }
    }

    internal class LocalizedDescriptionAttributeUniScanM : LocalizedDescriptionAttribute
    {
        public LocalizedDescriptionAttributeUniScanM(string value) : base("LocalizedDescriptionAttributeUniScanM", value)
        {
        }
    }
}
