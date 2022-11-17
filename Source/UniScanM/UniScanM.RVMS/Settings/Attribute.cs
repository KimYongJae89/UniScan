using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.RVMS.Settings
{
    internal class LocalizedCategoryAttributeRVMS : LocalizedCategoryAttribute
    {
        public LocalizedCategoryAttributeRVMS(string value) : base("LocalizedCategoryAttributeRVMS", value)
        {
        }
    }

    internal class LocalizedDisplayNameAttributeRVMS : LocalizedDisplayNameAttribute
    {
        public LocalizedDisplayNameAttributeRVMS(string value) : base("LocalizedDisplayNameAttributeRVMS", value)
        {
        }
    }

    internal class LocalizedDescriptionAttributeRVMS : LocalizedDescriptionAttribute
    {
        public LocalizedDescriptionAttributeRVMS(string value) : base("LocalizedDescriptionAttributeRVMS", value)
        {
        }
    }
}
