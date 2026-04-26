using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field
{
    public class Constraint
    {
        public IEnumerable<string> contentIds { get; set; }
        public IEnumerable<string> folderPaths { get; set; }
        public IEnumerable<string> userIds { get; set; }
        public IEnumerable<string> taxonomyIds { get; set; }
        public IEnumerable<string> menuPaths { get; set; }
        public string profileId { get; set; }
        public string contentTypeId { get; set; }
    }
}
