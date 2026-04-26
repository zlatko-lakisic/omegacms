using System;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.WebApi.Core.BaseControllers.Options
{
    public class ContentRequestOptions
    {
        public string[] ContentIds { get; set; }
        public bool LoadAuthor { get; set; }
        public bool LoadFields { get; set; }
        public bool LoadMetaData { get; set; }
        public int LCID { get; set; }
        public long FolderId { get; set; }
        public bool OnlyPublished { get; set; }
    }
}
