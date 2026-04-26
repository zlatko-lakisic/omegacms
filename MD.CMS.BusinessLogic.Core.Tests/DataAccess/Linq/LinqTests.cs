using Microsoft.VisualStudio.TestTools.UnitTesting;
using MD.CMS.BusinessLogic.Core.DataAccess.Linq.Content;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options;
using System.Linq;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Linq.Tests
{
    [TestClass()]
    public class LinqTests
    {
        [TestMethod()]
        public async Task Execute()
        {
            var query = from element in new ContentContext()
                        where element.IsDeleted == false && element.ApprovalPending == false
                        orderby element.Id descending
                        select element;
        }
    }
}