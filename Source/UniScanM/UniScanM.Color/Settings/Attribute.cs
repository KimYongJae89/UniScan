using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.ColorSens.Settings
{
    internal class LocalizedCategoryAttributeColor : LocalizedCategoryAttribute
    {
        public LocalizedCategoryAttributeColor(string value) : base("LocalizedCategoryAttributeColor", value)
        {
        }
    }

    internal class LocalizedDisplayNameAttributeColor : LocalizedDisplayNameAttribute
    {
        public LocalizedDisplayNameAttributeColor(string value) : base("LocalizedDisplayNameAttributeColor", value)
        {
        }
    }

    internal class LocalizedDescriptionAttributeColor : LocalizedDescriptionAttribute
    {
        public LocalizedDescriptionAttributeColor(string value) : base("LocalizedDescriptionAttributeColor", value)
        {
        }
    }
}
