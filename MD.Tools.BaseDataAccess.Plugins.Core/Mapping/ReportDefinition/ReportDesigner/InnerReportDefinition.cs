using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner
{
    public class InnerReportDefinition
    {
        #region Attributes
        private List<Entity> _entities;
        private List<Join> _joins;
        private List<Column> _columns;
        private List<Filter> _filters;
        private List<Group> _groupings;
        private Limit _limit;
        #endregion

        #region Properties

        public List<Entity> Entities
        {
            get 
            {
                if (_entities == null)
                {
                    _entities = new List<Entity>();
                }
                return _entities; 
            }
            set { _entities = value; }
        }

        public List<Join> Joins
        {
            get
            {
                if (_joins == null)
                {
                    _joins = new List<Join>();
                }
                return _joins;
            }
            set { _joins = value; }
        }

        public List<Column> Columns
        {
            get
            {
                if (_columns == null)
                {
                    _columns = new List<Column>();
                }
                return _columns;
            }
            set { _columns = value; }
        }

        public List<Filter> Filters
        {
            get
            {
                if (_filters == null)
                {
                    _filters = new List<Filter>();
                }
                return _filters;
            }
            set { _filters = value; }
        }

        public List<Group> Groupings
        {
            get
            {
                if (_groupings == null)
                {
                    _groupings = new List<Group>();
                }
                return _groupings;
            }
            set { _groupings = value; }
        }

        public Limit Limit
        {
            get { return _limit; }
            set { _limit = value; }
        }
        #endregion
    }
}
