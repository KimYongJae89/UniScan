using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.Pinhole.Settings
{
    internal class LocalizedCategoryAttributePinhole : LocalizedCategoryAttribute
    {
        public LocalizedCategoryAttributePinhole(string value) : base("LocalizedCategoryAttributePinhole", value)
        {
        }
    }

    internal class LocalizedDisplayNameAttributePinhole : LocalizedDisplayNameAttribute
    {
        public LocalizedDisplayNameAttributePinhole(string value) : base("LocalizedDisplayNameAttributePinhole", value)
        {
        }
    }

    internal class LocalizedDescriptionAttributePinhole : LocalizedDescriptionAttribute
    {
        public LocalizedDescriptionAttributePinhole(string value) : base("LocalizedDescriptionAttributePinhole", value)
        {
        }
    }
}
