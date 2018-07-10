using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace OCFF
{
    public class ConfigFileHandler
    {
        private ConfigData ConfigData;
        private ConfigParsedData ConfigParsedData;
        private readonly IComputeFunc ComputeFuncs;
        private readonly IEnumerationFunc EnumerationFuncs;
        private IArguments Arguments;
        private readonly IFileSystem FileSystem;
        private string FileName { get; }

        public ConfigFileHandler(IComputeFunc computeFuncs, IEnumerationFunc enumerationFuncs, IFileSystem fileSystem , string fileName = "ConfigFile.ocff")
        {
            FileSystem = fileSystem;
            ComputeFuncs = computeFuncs;
            EnumerationFuncs = enumerationFuncs;
            ConfigData = new ConfigData();
            FileName = fileName;
        }

        public ConfigFileHandler(IComputeFunc computeFuncs, IEnumerationFunc enumerationFuncs, string fileName = "ConfigFile.ocff"): this(computeFuncs,enumerationFuncs,new FileSystem(),fileName)
        { }

        public void InitConfigFile(bool overwrite)
        {
            var filePath = GetFilePath();
            var fileExsists = FileSystem.File.Exists(filePath);
            if (fileExsists && overwrite)
            {
                FileSystem.File.Delete(filePath);
                FileSystem.File.Create(filePath);
            }
            else if (!fileExsists)
            {
                FileSystem.File.Create(filePath);
            }
        }

        public List<ConfigComment> GetConfigComments() => ConfigData.GetComments();

        public ConfigParsedData LoadConfigFromFile(IArguments arguments)
        {
            Arguments = arguments;
            try
            {
                var lines = new List<string>();
                var fileContent = FileSystem.File.ReadLines(GetFilePath());
                foreach (var line in fileContent)
                {
                    if (line.StartsWith("#"))
                    {
                        ConfigData.AddComment(new ConfigComment(line));
                    }
                    else if (string.IsNullOrWhiteSpace(line) && lines.Count > 0)
                    {
                        ConfigData.Add(ParseSection(lines));
                        lines = new List<string>();
                    } 
                    else if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                    {
                        lines.Add(line);
                    }
                }
                if (lines.Count > 0)
                {
                    ConfigData.Add(ParseSection(lines));
                }
                return ConfigParsedData = ConfigData.ComputeAndReplace(Arguments, ComputeFuncs, EnumerationFuncs);
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }

        public ConfigParsedData WriteToConfig(string section, string value)
        {
            ConfigData.Add(CreateConfigSection(section, value));
            return ConfigParsedData = ConfigData.ComputeAndReplace(Arguments, ComputeFuncs, EnumerationFuncs);
        }

        public ConfigParsedData WriteToConfig(string section, bool value)
        {
            ConfigData.Add(CreateConfigSection(section, value.ToString(), false));
            return ConfigParsedData = ConfigData.ComputeAndReplace(Arguments, ComputeFuncs, EnumerationFuncs);
        }

        public ConfigParsedData WriteCommentToConfig(string comment)
        {
            ConfigData.AddComment(new ConfigComment($"#{comment}"));
            return ConfigParsedData = ConfigData.ComputeAndReplace(Arguments, ComputeFuncs, EnumerationFuncs);
        }

        public void UpdateConfigFile()
        {
            FileSystem.File.Delete(GetFilePath());
            var test = ConfigData.Print();
            FileSystem.File.AppendAllText(GetFilePath(),test );
        }

        private ConfigSection CreateConfigSection(string section, string value, bool isStringHeader = true)
        {
            return new ConfigSection(section, value, isStringHeader: isStringHeader, computeFuncs: ComputeFuncs, enumerationFuncs: EnumerationFuncs);
        }

        private ConfigSection CreateConfigSection(string section, List<string> values, bool isStringHeader = true)
        {
            return new ConfigSection(section, values, isStringHeader, computeFuncs: ComputeFuncs, enumerationFuncs: EnumerationFuncs);
        }

        private ConfigSection ParseSection(List<string> lines)
        {
            if (lines.Count < 2)
            {
                throw new ArgumentException(nameof(lines));
            }

            var header = lines.First();
            var (key, IsStringHeader) = ParseHeader(header);

            return CreateConfigSection(key, ParseSubSection(lines), IsStringHeader);
        }

        private List<string> ParseSubSection(List<string> lines)
        {
            return lines.GetRange(1, lines.Count - 1);//All but the first row
        }

        private (string key, bool IsStringHeader) ParseHeader(string header)
        {

            if (IsWellFormedStringSectionHeader(header))
            {
                var key = header.Trim('[', ']');
                return (key, true);
            }
            else if (IsWellFormedBoolSectionHeader(header))
            {
                var key = header.Trim('<', '>');
                return (key, false);
            }
            else
            {
                throw new ArgumentException(nameof(header));
            }
        }

        private bool IsWellFormedStringSectionHeader(string header) => header.StartsWith("[") && header.EndsWith("]");
        private bool IsWellFormedBoolSectionHeader(string header) => header.StartsWith("<") && header.EndsWith(">");
        private string GetFilePath() => Path.Combine( FileSystem.Directory.GetCurrentDirectory(), FileName);
    }
}
