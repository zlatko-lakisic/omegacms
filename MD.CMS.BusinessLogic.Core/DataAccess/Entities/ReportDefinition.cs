using MD.Tools.BaseDataAccess.Core.Entities;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner;
using Newtonsoft.Json;
using System;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ReportDefinition : BaseEntity<long>
    {
        #region Attributes
        private string _name;
        private InnerReportDefinition _definition;
        private string _sql;
        private string _authorId;
        private User _author;
        private DateTime _dateCreated;
        private DateTime _dateModified;
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public InnerReportDefinition Definition
        {
            get { return _definition; }
            set { _definition = value; }
        }
        
        public string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }
        
        public string Json
        {
            get 
            {
                if (_definition != null)
                {
                    return JsonConvert.SerializeObject(_definition);
                }
                return string.Empty;
            }
            set 
            {
                try
                {
                    _definition = JsonConvert.DeserializeObject<InnerReportDefinition>(value);
                }
                catch (Exception error)
                {

                }
            }
        }

        public string AuthorId
        {
            get { return _authorId; }
            set { _authorId = value; }
        }

        public User Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        public DateTime DateModified
        {
            get { return _dateModified; }
            set { _dateModified = value; }
        }
        #endregion
    }
}
