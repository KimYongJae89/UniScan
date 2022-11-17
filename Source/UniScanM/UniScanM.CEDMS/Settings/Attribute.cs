using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.CEDMS.Settings
{
    internal class LocalizedCategoryAttributeCEDMS : LocalizedCategoryAttribute
    {
        public LocalizedCategoryAttributeCEDMS(string value) : base("LocalizedCategoryAttributeCEDMS", value)
        {
        }
    }

    internal class LocalizedDisplayNameAttributeCEDMS : LocalizedDisplayNameAttribute
    {
        public LocalizedDisplayNameAttributeCEDMS(string value) : base("LocalizedDisplayNameAttributeCEDMS", value)
        {
        }
    }

    internal class LocalizedDescriptionAttributeCEDMS : LocalizedDescriptionAttribute
    {
        public LocalizedDescriptionAttributeCEDMS(string value) : base("LocalizedDescriptionAttributeCEDMS", value)
        {
        }
    }
}
