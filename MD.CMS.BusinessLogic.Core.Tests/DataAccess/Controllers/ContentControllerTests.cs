using Microsoft.VisualStudio.TestTools.UnitTesting;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Tests;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options;
using System.Linq;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Tests
{
    [TestClass()]
    public class ContentControllerTests
    {
        [TestMethod()]
        public async Task GetByIdTest()
        {
            await Startup.Init();

            IEnumerable<Content> contents = await ContentController<Content>.Instance.Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).GetByIdAsync(new ContentOptions()
            {
                ContentIds = new string[] { "8" }.ToList(),
                FillFields = true,
                LoadAuthor = true,
                FillMetaData = true,
                Lcid = 2057
            });

            Assert.IsTrue(contents != null && contents.Any());
        }

        [TestMethod()]
        public async Task GetByFolderIdAsyncTest()
        {
            await Startup.Init();

            IEnumerable<Content> contents = await ContentController<Content>.Instance.Caller(MD.CMS.BusinessLogic.Core.DataAccess.Entities.User.SystemUser()).GetByFolderIdAsync(
                id: 6,
                loadAuthor: true,
                lcid: 2057,
                loadFields: true,
                loadMetaDataFields: true
            );

            Assert.IsTrue(contents != null && contents.Any());
        }
    }
}