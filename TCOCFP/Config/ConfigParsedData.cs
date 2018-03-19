using System.Collections.Generic;
using System.Linq;

namespace TCOCFP
{
    public class ConfigParsedData
    {
        private Dictionary<string, List<ConfigSection>> DataStore;

        public ConfigParsedData()
        {
            DataStore = new Dictionary<string, List<ConfigSection>>();
        }

        public void Add(ConfigSection configSection)
        {
            if (KeyExsists(configSection.Key))
            {
                DataStore[configSection.Key].Add(configSection);
            }
            else
            {
                AddInitialValue(configSection);
            }
        }

        public void AddRange(IEnumerable<ConfigSection> list)
        {
            list.ToList().ForEach(x => Add(x));
        }

        public bool KeyExsists(string key)
        {
            return DataStore.ContainsKey(key);
        }

        private void AddInitialValue(ConfigSection configSection)
        {
            DataStore.Add(configSection.Key, new List<ConfigSection> { configSection });
        }
    }
}
