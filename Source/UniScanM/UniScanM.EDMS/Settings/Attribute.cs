using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.EDMS.Settings
{
    internal class LocalizedCategoryAttributeEDMS : LocalizedCategoryAttribute
    {
        public LocalizedCategoryAttributeEDMS(string value) : base("LocalizedCategoryAttributeEDMS", value)
        {
        }
    }

    internal class LocalizedDisplayNameAttributeEDMS : LocalizedDisplayNameAttribute
    {
        public LocalizedDisplayNameAttributeEDMS(string value) : base("LocalizedDisplayNameAttributeEDMS", value)
        {
        }
    }

    internal class LocalizedDescriptionAttributeEDMS : LocalizedDescriptionAttribute
    {
        public LocalizedDescriptionAttributeEDMS(string value) : base("LocalizedDescriptionAttributeEDMS", value)
        {
        }
    }
}
