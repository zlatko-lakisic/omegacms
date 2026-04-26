using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Helpers.Extensions
{
    public static class MethodExtenisons
    {
        public static List<IExtendedMethodProperty> ToExtended(this Method method, List<IExtendedMethodProperty> properties)
        {
            foreach (IExtendedMethodProperty property in properties)
            {
                IMethodProperty baseProperty = method.Properties.FirstOrDefault(p => property.Id.Equals(p.Id));
                if (baseProperty != null)
                {
                    property.Value = baseProperty.Value;
                }
            }
            return properties;
        }
    }
}
