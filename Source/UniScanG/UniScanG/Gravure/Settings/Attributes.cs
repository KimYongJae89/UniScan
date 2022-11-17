using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Settings
{
    public class LocalizedCategoryAttributeUniScanG : LocalizedCategoryAttribute
    {
        public LocalizedCategoryAttributeUniScanG(string value) : base("LocalizedCategoryAttributeUniScanG", value)
        {
        }
    }

    public class LocalizedDisplayNameAttributeUniScanG : LocalizedDisplayNameAttribute
    {
        public LocalizedDisplayNameAttributeUniScanG(string value) : base("LocalizedDisplayNameAttributeUniScanG", value)
        {
        }
    }

    public class LocalizedDescriptionAttributeUniScanG : LocalizedDescriptionAttribute
    {
        public LocalizedDescriptionAttributeUniScanG(string value) : base("LocalizedDescriptionAttributeUniScanG", value)
        {
        }
    }
}
