using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace MD.Tools.BaseDataAccess.PluginMethods.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            dynamic _pluginSettingsJson = JObject.Parse("{ \"solr_host\": \"http://localhost:8983/solr/\", \"solr_postfix\": \"\", \"solr_jobFileLocation\": \"C:\\\\temp\\\\pluginJobs.txt\" }");

        }
    }
}
