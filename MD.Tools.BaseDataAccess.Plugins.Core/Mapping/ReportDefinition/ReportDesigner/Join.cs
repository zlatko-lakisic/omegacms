using MD.Tools.Helpers.Core.Data;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner
{
    public class Join
    {
        #region Helper Class
        public class JoinInner
        {
            #region Attributes
            private Entity _entity;
            private Property _property;
            #endregion

            #region Properties

            public Entity Entity
            {
                get { return _entity; }
                set { _entity = value; }
            }

            public Property Property
            {
                get { return _property; }
                set { _property = value; }
            }
            #endregion
        }
        #endregion

        #region Attributes
        private JoinInner _left;
        private JoinInner _right;
        private ComparerTypeEnum _type;
        #endregion

        #region Properties

        public JoinInner Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public JoinInner Right
        {
            get { return _right; }
            set { _right = value; }
        }

        public ComparerTypeEnum Type
        {
            get { return _type; }
            set { _type = value; }
        }
        #endregion
    }
}
