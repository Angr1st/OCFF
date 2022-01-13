using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCFF
{
    class ConfigData
    {
        private readonly List<ConfigComment> Comments;
        private readonly List<ConfigSection> DataStore;

        public ConfigData()
        {
            Comments = new List<ConfigComment>();
            DataStore = new List<ConfigSection>();
        }

        public void Add(ConfigSection configSection)
        {
            if (KeyExsists(configSection.Key))
            {
                Remove(configSection.Key);
            }

            DataStore.Add(configSection);
        }

        public void AddComment(ConfigComment comment)
        {
            Comments.Add(comment);
        }

        public void AddCommentRange(IEnumerable<ConfigComment> configComments)
        {
            foreach (var item in configComments)
            {
                AddComment(item);
            }
        }

        public void AddRange(IEnumerable<ConfigSection> configSections)
        {
            foreach (var item in configSections)
            {
                Add(item);
            }
        }

        public List<ConfigComment> GetComments() => new List<ConfigComment>(Comments);

        public ConfigParsedData ComputeAndReplace(IArguments arguments, IComputeFunc computeFuncs, IEnumerationFunc enumerationFuncs)
        {
            var newConfigData = new ConfigParsedData();
            var dictionary = new Dictionary<string, List<string>>();
            List<(string header, string value, List<IConfigSet> variables)> list = new List<(string, string, List<IConfigSet>)>();
            var allKeys = TurnListConfigSectionIntoDict();
            DataStore.Where(y => y.IsString).ToList().ForEach(x => { var keyValue = allKeys[x.Key]; list.Add((x.Key, keyValue, x.ReturnAllVariables())); });
            foreach (var item in list)
            {
                StringBuilder stringBuilder = new StringBuilder(item.value);
                List<StringBuilder> stringBuilderList = new List<StringBuilder>() { stringBuilder };
                foreach (var innerItem in item.variables)
                {
                    if (innerItem is ConfigComputeSet)
                    {
                        var innerComputeItem = innerItem as ConfigComputeSet;
                        stringBuilderList = stringBuilderList.SelectMany(y => GetParameter(innerComputeItem.Name, arguments, dictionary).Select(x => new StringBuilder(y.ToString().Replace(innerComputeItem.Token, innerComputeItem.Compute(x))))).ToList();
                    }
                    else if (innerItem is ConfigEnumerationSet)
                    {
                        var innerEnumerationItem = innerItem as ConfigEnumerationSet;
                        var enumeration = GetParameter(innerEnumerationItem.Name, arguments, dictionary).SelectMany(y => innerEnumerationItem.GetEnumerable(y));
                        stringBuilderList = stringBuilderList.SelectMany(y => enumeration.Select(x => new StringBuilder(y.ToString().Replace(innerEnumerationItem.Token, x)))).ToList();
                    }
                    else
                    {
                        stringBuilderList = stringBuilderList.SelectMany(y => GetParameter(innerItem.Name, arguments, dictionary).Select(x => new StringBuilder(y.ToString().Replace(innerItem.Token, x)))).ToList();
                    }
                }
                AddConfigSections(item, stringBuilderList);
                dictionary.Add(item.header, stringBuilderList.Select(x => x.ToString()).ToList());
            }
            newConfigData.AddRange(DataStore.Where(x => !x.IsString));
            return newConfigData;

            void AddConfigSections((string header, string value, List<IConfigSet> variables) item, List<StringBuilder> listOfStringBuilders)
            {
                foreach (var itemForEach in listOfStringBuilders)
                {
                    var stringBuilderValue = itemForEach.ToString();
                    newConfigData.Add(new ConfigSection(item.header, stringBuilderValue, isStringHeader: true, computeFuncs: computeFuncs, enumerationFuncs: enumerationFuncs));
                }
            }
        }

        public IEnumerable<string> GetKeys() => DataStore.Select(x => x.Key);

        public IEnumerable<string> GetValues() => DataStore.Select(x => x.Value);

        public string Read(string key) => DataStore.Find(x => x.Key == key).Value;

        public string Print() => string.Join("\n", Comments.Select(x => x.Print())) + "\n" + string.Join("\n\n", DataStore.Select(x => x.Print()));

        private IEnumerable<string> GetParameter(string argumentName, IArguments arguments, Dictionary<string, List<string>> dict)
        {
            try
            {
                var argResult = arguments.GetArgument(argumentName);
                if (argResult.IsFound())
                {
                    return new List<string> { argResult.Result };
                }
                else
                {
                    return dict[argumentName];
                }
            }
            catch (Exception ex)
            {
                if (dict.ContainsKey(argumentName))
                {
                    return dict[argumentName];
                }
                throw new ArgumentException($"{nameof(argumentName)}:{argumentName}", ex);
            }
        }

        private Dictionary<string, string> TurnListConfigSectionIntoDict()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            DataStore.ForEach(x => dictionary.Add(x.Key, x.Value));
            return dictionary;
        }

        private void Remove(string key) => DataStore.Remove(DataStore.Find(x => x.Key == key));

        private bool KeyExsists(string key) => DataStore.Count(x => x.Key == key) > 0;
    }
}
