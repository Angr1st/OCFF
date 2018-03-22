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
            string content = InitConfigFile(true, false);
            Assert.AreEqual(string.Empty, content);
        }

        [TestMethod]
        public void InitConfigFileTestTrueAndEmpty()
        {
            string content = InitConfigFile(true, true);
            Assert.AreEqual(string.Empty, content);
        }

        [TestMethod]
        public void InitConfigFileTestFalse()
        {
            string content = InitConfigFile(false, false);
            Assert.AreEqual("[Testing]\nis meh.", content);
        }

        [TestMethod]
        public void WriteToConfigTest()
        {
            var comment = "#This is a comment";
            var key = "[Start]";
            var value = "Some Value";
            var fileSystem = CreateMockFileSystem(comment);
            var sut = CreateEmptyConfigFileHandler(fileSystem);
            var result = sut.WriteToConfig(key, value);
            Assert.IsTrue(result.KeyExsists(key));
            var resultList = result.GetDataStoreEntry(key);
            Assert.IsTrue(resultList.FirstOrDefault().Value == value);
        }

        [TestMethod]
        public void LoadConfigFileTestWithEmptyArguments()
        {
            var key = "Testing";
            var fileSystem = CreateMockFileSystem();
            var sut = CreateEmptyConfigFileHandler(fileSystem);
            var result = sut.LoadConfigFromFile(new EmptyArguments());
            Assert.IsTrue(result.KeyExsists(key));
            var resultList = result.GetDataStoreEntry(key);
            Assert.IsTrue(resultList.FirstOrDefault().Value == "is meh.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoadConfigFileTestWithEmptyArgumentsNotWellFormedHeader()
        {
            var content = "{Testing}\n";
            var fileSystem = CreateMockFileSystem(content);
            var sut = CreateEmptyConfigFileHandler(fileSystem);
            var result = sut.LoadConfigFromFile(new EmptyArguments());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoadConfigFileTestWithEmptyArgumentsNotWellFormedHeader2()
        {
            var content = "{Testing}\nHello";
            var fileSystem = CreateMockFileSystem(content);
            var sut = CreateEmptyConfigFileHandler(fileSystem);
            var result = sut.LoadConfigFromFile(new EmptyArguments());
        }

        [TestMethod]
        public void LoadConfigFileTestWithEmptyArgumentsAndANewLineAtTheEnd()
        {
            var key = "Testing";
            var content = "[Testing]\nis meh.\n\n<TestBool>\nTrue\n";
            var fileSystem = CreateMockFileSystem(content);
            var sut = CreateEmptyConfigFileHandler(fileSystem);
            var result = sut.LoadConfigFromFile(new EmptyArguments());
            Assert.IsTrue(result.KeyExsists(key));
            var resultList = result.GetDataStoreEntry(key);
            Assert.IsTrue(resultList.FirstOrDefault().Value == "is meh.");
            var resultList2 = result.GetDataStoreEntry("TestBool");
            Assert.IsTrue(resultList2.FirstOrDefault().Value == "True");
        }

        [TestMethod]
        public void LoadConfigFileTestWithEmptyArgumentsPlusCompute()
        {
            var key = "ComputeTest";
            var value = "sameValue";
            var content = $"[TestFunc]\n{value}\n\n[{key}]\n$TestFunc\n";
            var fileSystem = CreateMockFileSystem(content);
            var sut = CreateTestConfigFileHandler(fileSystem);
            var result = sut.LoadConfigFromFile(new EmptyArguments());
            Assert.IsTrue(result.KeyExsists(key));
            Assert.IsTrue(result.GetDataStoreEntry(key).FirstOrDefault().Value == $"TestPlus;{value}");
        }

        [TestMethod]
        public void LoadConfigFileTestWithEmptyArgumentsPlusReplace()
        {
            var key = "ComputeTest";
            var value = "sameValue";
            var content = $"[TestFunc]\n{value}\n\n[{key}]\n@TestFunc\n";
            var fileSystem = CreateMockFileSystem(content);
            var sut = CreateTestConfigFileHandler(fileSystem);
            var result = sut.LoadConfigFromFile(new EmptyArguments());
            Assert.IsTrue(result.KeyExsists(key));
            Assert.IsTrue(result.GetDataStoreEntry(key).FirstOrDefault().Value == $"{value}");
        }

        [TestMethod]
        public void LoadConfigFileTestWithEmptyArgumentsPlusEnum()
        {
            var key = "ComputeTest";
            var value = "sameValue";
            var content = $"[TestEnumFunc]\n{value}\n\n[{key}]\n&TestEnumFunc\n";
            var fileSystem = CreateMockFileSystem(content);
            var sut = CreateTestConfigFileHandler(fileSystem);
            var result = sut.LoadConfigFromFile(new EmptyArguments());
            Assert.IsTrue(result.KeyExsists(key));
            Assert.IsTrue(result.GetDataStoreEntry(key).All(x => x.Value == $"{value}"));
        }

        [TestMethod]
        public void LoadConfigFileTestWithTestArgumentsComputeValueGetsLoadedFromArgs()
        {
            var key = "ComputeTest";
            var value = "sameValue";
            var content = $"[{key}]\n$TestFunc\n";
            var fileSystem = CreateMockFileSystem(content);
            var sut = CreateTestConfigFileHandler(fileSystem);
            var result = sut.LoadConfigFromFile(new TestArgument(value));
            Assert.IsTrue(result.KeyExsists(key));
            Assert.IsTrue(result.GetDataStoreEntry(key).FirstOrDefault().Value == $"TestPlus;{value}");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void LoadConfigFileTestWithEmptyArgumentsAndWrongBoolValue()
        {
            var content = "<TestBool>\nLel\n";
            var fileSystem = CreateMockFileSystem(content);
            var sut = CreateEmptyConfigFileHandler(fileSystem);
            var result = sut.LoadConfigFromFile(new EmptyArguments());
        }

        [TestMethod]
        public void LoadConfigFileTestWithEmptyArgumentsWithComments()
        {
            var comment = "#This is a comment";
            var fileSystem = CreateMockFileSystem(comment);
            var sut = CreateEmptyConfigFileHandler(fileSystem);
            var result = sut.LoadConfigFromFile(new EmptyArguments());
            Assert.IsTrue(sut.GetConfigComments().FirstOrDefault().Comment == comment);
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void LoadConfigFileTestWithEmptyArgumentsAndEmptyFileSystem()
        {
            var fileSystem = CreateEmptyMockFileSystem();
            var sut = CreateEmptyConfigFileHandler(fileSystem);
            var result = sut.LoadConfigFromFile(new EmptyArguments());
        }

        private static string InitConfigFile(bool overwrite, bool withEmpty)
        {
            MockFileSystem fileSystem;
            if (withEmpty)
            {
                fileSystem = CreateEmptyMockFileSystem();
            }
            else
            {
                fileSystem = CreateMockFileSystem();
            }
            var sut = CreateEmptyConfigFileHandler(fileSystem);
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

        private static MockFileSystem CreateMockFileSystem(string content)
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> { { @"c:\Test\ConfigFile.ocff", new MockFileData(content) } }, "c:\\Test");
            return fileSystem;
        }

        private static MockFileSystem CreateEmptyMockFileSystem()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> { }, "c:\\Test");
            return fileSystem;
        }

        private static ConfigFileHandler CreateEmptyConfigFileHandler(MockFileSystem fileSystem)
        {
            return new ConfigFileHandler(new EmptyComputeFuncs(), new EmptyEnumerationFuncs(), fileSystem);
        }

        private static ConfigFileHandler CreateTestConfigFileHandler(MockFileSystem fileSystem)
        {
            return new ConfigFileHandler(new TestCompute(), new TestEnumeration(), fileSystem);
        }
    }
}
