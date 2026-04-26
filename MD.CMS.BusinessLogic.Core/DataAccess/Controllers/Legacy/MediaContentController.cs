using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class MediaContentController : BaseController<MediaContentController>
    {
        [Obsolete("Deprecated", true)]
        public MediaContent GetById(long id, int lcid = default(int), bool fillMetaDataFields = false)
        {
            return GetByIdAsync(id, lcid, fillMetaDataFields).Result;
        }

        [Obsolete("Deprecated", true)]
        public long SelectAllCount(int lcid = default(int))
        {
            return SelectAllCountAsync(lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<MediaContent> GetByFolderId(long id, int lcid = default(int))
        {
            return GetByFolderIdAsync(id, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<MediaContent> GetByFileType(long id, int lcid = default(int))
        {
            return GetByFileTypeAsync(id, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<MediaContent> SearchByFileType(string searchTerm, int fileType, int lcid = default(int))
        {
            return SearchByFileTypeAsync(searchTerm, fileType, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<MediaContent> Search(string searchTerm, int lcid = default(int))
        {
            return SearchAsync(searchTerm, lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public bool Delete(MediaContent mediaContent)
        {
            return DeleteAsync(mediaContent).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<MediaContent> GetAll(int lcid = default(int))
        {
            return GetAllAsync(lcid).Result;
        }

        [Obsolete("Deprecated", true)]
        public MediaContent Save(MediaContent mediaContent)
        {
            return SaveAsync(mediaContent).Result;
        }

        [Obsolete("Deprecated", true)]
        public MediaContent UpdatePath(string path, long id)
        {
            return UpdatePathAsync(path, id).Result;
        }

        [Obsolete("Deprecated", true)]
        public MediaContent UpdateFullName(string fullname, long id)
        {
            return UpdateFullNameAsync(fullname, id).Result;
        }

        [Obsolete("Deprecated", true)]
        public MediaContent UpdatePreviewUrl(string path, long id)
        {
            return UpdatePreviewUrlAsync(path, id).Result;
        }

        [Obsolete("Deprecated", true)]
        public MediaContent GetBaseInformation(long id)
        {
            return GetBaseInformationAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public MediaContent UpdatePreviewUrl(long id, string pathyoutube)
        {
            return UpdatePreviewUrlAsync(id, pathyoutube).Result;
        }

        [Obsolete("Deprecated", true)]
        public Entities.Base.BasePaginationEntity<MediaContent> GetByFolderIdWithPagination(long id, long currentPageIndex, long maxNumberOfRows, string searchTerm = "", int lcid = default(int), string sort = "Name ASC")
        {
            return GetByFolderIdWithPaginationAsync(id, currentPageIndex, maxNumberOfRows, searchTerm, lcid, sort).Result;
        }

        [Obsolete("Deprecated", true)]
        public int GetByFolderIdCount(long folderId, int lcid, string searchTerm)
        {
            return GetByFolderIdCountAsync(folderId, lcid, searchTerm).Result;
        }
    }
}
