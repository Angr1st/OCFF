using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCFF_UnitTest
{
    [TestClass]
    public class ConfigParsedDataTest
    {
        [TestMethod]
        public void GetDataStoreTest()
        {
            ConfigParsedData configParsedData = CreateConfigParsedData();
            AddTestSection(configParsedData);
            var result = configParsedData.GetDataStore();
            Assert.IsTrue(result.ContainsKey("Test"));
        }

        [TestMethod]
        public void AddTest()
        {
            ConfigParsedData configParsedData = CreateConfigParsedData();
            AddTestSection(configParsedData);
            Assert.IsTrue(configParsedData.KeyExsists("Test"));
        }

        [TestMethod]
        public void AddDoubleTest()
        {
            ConfigParsedData configParsedData = CreateConfigParsedData();
            AddTestSection(configParsedData);
            AddTestSection(configParsedData);
            Assert.IsTrue(configParsedData.KeyExsists("Test"));
        }

        private static ConfigParsedData AddTestSection(ConfigParsedData configParsedData)
        {
            configParsedData.Add(new ConfigSection("Test", "Value", true, new EmptyComputeFuncs(), new EmptyEnumerationFuncs()));
            return configParsedData;
        }

        private static ConfigParsedData CreateConfigParsedData()
        {
            return new ConfigParsedData();
        }
    }
}
