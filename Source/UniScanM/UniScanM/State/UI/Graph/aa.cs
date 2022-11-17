using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.UI.Graph
{

    public abstract class PropertyCollection : CollectionBase, ICustomTypeDescriptor
    {
        public LineProperty this[int index] { get => (LineProperty)this.InnerList[index]; }

        public void Add(LineProperty lineProperty)
        {
            this.InnerList.Add(lineProperty);
        }

        public void Remove(LineProperty lineProperty)
        {
            this.InnerList.Remove(lineProperty);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);
            for (int i = 0; i < this.List.Count; i++)
            {
                PropertyDescriptor pd = BuildPropertyCollectionPropertyDescriptor(this, i);
                pds.Add(pd);
            }
            return pds;
        }

        public abstract PropertyDescriptor BuildPropertyCollectionPropertyDescriptor(PropertyCollection propertyCollection, int index);

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
    }


    public abstract class PropertyCollectionPropertyDescriptor : PropertyDescriptor
    {
        PropertyCollection collection = null;
        int index = -1;

        public PropertyCollectionPropertyDescriptor(PropertyCollection collection, int index)
            : base(collection[index].Name, null)
        {
            this.collection = collection;
            this.index = index;
        }
        public override Type ComponentType => this.collection.GetType();
        public override Type PropertyType => this.collection[this.index].GetType();
        public override bool IsReadOnly => true;
        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
        }
        public override object GetValue(object component)
        {
            return this.collection[this.index];
        }
        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }


}
