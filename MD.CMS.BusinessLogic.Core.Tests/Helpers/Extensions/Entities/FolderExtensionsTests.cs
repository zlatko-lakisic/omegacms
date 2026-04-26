using System.Collections.Generic;
using System.Linq;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MD.CMS.BusinessLogic.Core.Tests.Helpers.Extensions.Entities
{
    [TestClass]
    public class FolderExtensionsTests
    {
        [TestMethod]
        public void GetJson_MapsFolderHierarchyToNestedJson()
        {
            var child = new Folder<Content>
            {
                Id = 3,
                ParentId = 2,
                Name = "ChildFolder"
            };

            var parent = new Folder<Content>
            {
                Id = 2,
                ParentId = 0,
                Name = "ParentFolder",
                Children = new List<Folder<Content>> { child }
            };

            var result = new List<Folder<Content>> { parent }.GetJson().ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("ParentFolder", result[0].Name);
            Assert.AreEqual("2", result[0].Value);
            Assert.AreEqual("0", result[0].ParentId);
            Assert.IsNotNull(result[0].Children);
            Assert.AreEqual(1, result[0].Children.Count());
            Assert.AreEqual("ChildFolder", result[0].Children.First().Name);
        }
    }
}
