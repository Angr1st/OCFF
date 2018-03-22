using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCFF;

namespace OCFF_UnitTest
{
    [TestClass]
    public class ConfigFileHandlerTest
    {
        [TestMethod]
        public void InitConfigFileTestTrue()
        {
            string content = InitConfigFile(true);
            Assert.AreEqual(string.Empty, content);
        }

        [TestMethod]
        public void InitConfigFileTestFalse()
        {
            string content = InitConfigFile(false);
            Assert.AreEqual("[Testing]\nis meh.", content);
        }

        private static string InitConfigFile(bool overwrite)
        {
            MockFileSystem fileSystem = CreateMockFileSystem();
            var sut = CreateConfigFileHandler(fileSystem);
            sut.InitConfigFile(overwrite);
            var configFile = fileSystem.GetFile(@"c:\Test\ConfigFile.ocff");
            var content = Encoding.UTF8.GetString(configFile.Contents);
            return content;
        }

        private static MockFileSystem CreateMockFileSystem()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> { { @"c:\Test\ConfigFile.ocff", new MockFileData("[Testing]\nis meh.") } }, "c:\\Test");
            return fileSystem;
        }

        private static ConfigFileHandler CreateConfigFileHandler(MockFileSystem fileSystem)
        {
            return new ConfigFileHandler(new EmptyComputeFuncs(), new EmptyEnumerationFuncs(), fileSystem);
        }

        [TestMethod]
        public void LoadConfigFileTestWithEmptyArguments()
        {
            var key = "Testing";
            var fileSystem = CreateMockFileSystem();
            var sut = CreateConfigFileHandler(fileSystem);
            var result = sut.LoadConfigFromFile(new EmptyArguments());
            Assert.IsTrue(result.KeyExsists(key));
            var resultList = result.GetDataStoreEntry(key);
            Assert.IsTrue(resultList.FirstOrDefault().Value == "is meh.");
        }
    }
}
