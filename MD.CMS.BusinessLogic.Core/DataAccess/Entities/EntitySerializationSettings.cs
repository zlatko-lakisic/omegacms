using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class EntitySerializationSettings
    {
        private static bool _serializeAllProperties;

        public static bool SerializeAllProperties
        {
            get { return _serializeAllProperties; }
            set { _serializeAllProperties = value; }
        }
    }
}
