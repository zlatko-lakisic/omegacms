using MD.CMS.BusinessLogic.Core.Helpers.Calculations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MD.CMS.BusinessLogic.Core.Tests.Helpers.Calculations
{
    [TestClass]
    public class PostfixMakerTests
    {
        [TestMethod]
        public void MakePostfixFromInfix_UsesOperatorPrecedence()
        {
            // 2 + 3 * 4 => 2 3 4 * +
            var maker = new PostfixMaker();

            string postfix = maker.MakePostfixFromInfix("2,+,3,*,4");

            Assert.AreEqual("2,3,4,*,+", postfix);
        }

        [TestMethod]
        public void MakePostfixFromInfix_RespectsParentheses()
        {
            // (2 + 3) * 4 => 2 3 + 4 *
            var maker = new PostfixMaker();

            string postfix = maker.MakePostfixFromInfix("(,2,+,3,),*,4");

            Assert.AreEqual("2,3,+,4,*", postfix);
        }

        [TestMethod]
        public void MakePostfixFromInfix_IgnoresTrailingSeparator()
        {
            var maker = new PostfixMaker();

            string postfix = maker.MakePostfixFromInfix("8,/,2,");

            Assert.AreEqual("8,2,/", postfix);
        }
    }
}
