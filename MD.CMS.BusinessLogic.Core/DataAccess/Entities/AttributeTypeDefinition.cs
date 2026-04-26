using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class AttributeTypeDefinition : BaseEntity<long>
    {
        #region Attributes
        private string _name;
        private string _defaultValue;
        private EnumType _type;
        private EnumInputType _inputType;
        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        public EnumType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public EnumInputType InputType
        {
            get { return _inputType; }
            set { _inputType = value; }
        }
        #endregion

        #region Enums
        public enum EnumType : int
        {
#pragma warning disable CA1720 // Identifier contains type name
            String = 1,
#pragma warning restore CA1720 // Identifier contains type name
#pragma warning disable CA1720 // Identifier contains type name
            Int = 2,
#pragma warning restore CA1720 // Identifier contains type name
            Boolean = 3
        }
        public enum EnumInputType : int
        {
            Input = 1,
            TrueFalse = 2,
            Textarea = 3,
            SelectSingle = 4,
            SelectMultiple = 5,
            TaxonomySelectorSingle = 6,
            TaxonomySelectorMultiple = 7,
            File = 8,
            Date = 9,
            Map = 10,
            ContentSelectorSingle = 11,
            Youtube = 12,
            Section = 13,
            MediaContentSelectorSingle = 14,
            UserSelectorSingle = 15,
            Calculated = 16,
            Tabs = 17
        }
        #endregion
    }
}
