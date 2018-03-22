using System.Collections.Generic;
using System.Linq;

namespace OCFF
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

        public Dictionary<string, List<ConfigSection>> GetDataStore() => new Dictionary<string, List<ConfigSection>>(DataStore);

        public List<ConfigSection> GetDataStoreEntry(string key) => DataStore[key];

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
