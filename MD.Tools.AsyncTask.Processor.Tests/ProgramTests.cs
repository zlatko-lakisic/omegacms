using Microsoft.VisualStudio.TestTools.UnitTesting;
using MD.Tools.AsyncTask.Processor;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace MD.Tools.AsyncTask.Processor.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void CreateHostBuilderTest()
        {
            Program.CreateHostBuilder(new List<string>().ToArray()).Build().Run();
        }
    }
}