using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using System.Reflection;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.Enumerations;
using System.Data;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner
{
    public class Entity
    {
        #region Helper Class
        public class GridCoordinates
        {
            #region Attributes
            private int _x;
            private int _y;
            private int _width;
            private int _height;
            #endregion

            #region Properties
            public int width { get => _width; set => _width = value; }
            public int height { get => _height; set => _height = value; }
            public int x { get => _x; set => _x = value; }
            public int y { get => _y; set => _y = value; }
            #endregion
        }
        #endregion

        #region Attributes
        private long _id;
        private EntityType _type;
        private string _name;
        private string _icon;
        private GridCoordinates _coordinates;
        private List<Property> _baseFields;
        private List<Property> _extendedFields;
        private List<Property> _fields;
        #endregion

        #region Properties

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public EntityType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public GridCoordinates Coordinates
        {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        public string UniqueId
        {
            get { return string.Format("{0}_{1}_{2}", Name.Replace(" ", "_").Replace("%", "_").Replace(".", "_").Replace("-", "_"), Type.GetIntValue(), Id.ToString().Replace("-", "_")); }
        }

        public List<Property> Fields
        {
            get 
            {
                if (_fields == null)
                {
                    _fields = new List<Property>();
                    _fields.AddRange(BaseFields.Select(p => p.Clone() as Property).ToList());
                    _fields.AddRange(ExtendedFields.Select(p => p.Clone() as Property).ToList());
                }
                return _fields;
            }
            set
            {
                _fields = value;
            }
        }

        public List<Property> BaseFields
        {
            get
            {
                if (_baseFields == null)
                {
                    _baseFields = new List<Property>();
                }
                return _baseFields;
            }
            set { _baseFields = value; }
        }

        public List<Property> ExtendedFields
        {
            get 
            {
                if (_extendedFields == null)
                {
                    _extendedFields = new List<Property>();
                }
                return _extendedFields;
            }
            set { _extendedFields = value; }
        }

        public string Icon { get => _icon; set => _icon = value; }
        #endregion

        #region Methods

        public void AddBasicFields<T>()
            where T : class
        {
            BaseFields.AddRange(typeof(T).GetProperties().Where(field => field.GetCustomAttributes(typeof(CustomAttributes.OmitPropertyFromReport), true).Length.Equals(0)).Select(field => new Property(field)));
        }

        public void AddExtendedFields<T>()
            where T : class
        {
            ExtendedFields.AddRange(typeof(T).GetProperties().Where(field => field.GetCustomAttributes(typeof(CustomAttributes.OmitPropertyFromReport), true).Length.Equals(0)).Select(field => new Property(field)).Where(p => !BaseFields.Contains(p)));
        }
        #endregion
    }
}
