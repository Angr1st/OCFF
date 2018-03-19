using System;
using System.Collections.Generic;
using System.Linq;

namespace TCOCFP
{
    public class ConfigSection
    {
        public string Key;
        public string Value;
        public bool IsString;
        private List<ConfigComputeSet> ComputeVariables;
        private List<ConfigReplaceSet> ReplaceVariables;
        private List<ConfigEnumerationSet> EnumerationVariables;
        private int NumberOfVariables = 0;
        private IComputeFunc ComputeFuncs;
        private IEnumerationFunc EnumerationFuncs;

        public ConfigSection(string key, string value, bool isStringHeader, IComputeFunc computeFuncs, IEnumerationFunc enumerationFuncs)
        {
            ComputeFuncs = computeFuncs;
            EnumerationFuncs = enumerationFuncs;
            Key = key;
            Value = value;
            if (isStringHeader)
            {
                ReplaceVariables = new List<ConfigReplaceSet>();
                ComputeVariables = new List<ConfigComputeSet>();
                EnumerationVariables = new List<ConfigEnumerationSet>();
                foreach (var item in RetrieveAllVariables('@'))
                {
                    ReplaceVariables.Add(new ConfigReplaceSet(item.key, item.token));
                }
                foreach (var item in RetrieveAllVariables('$'))
                {
                    ComputeVariables.Add(new ConfigComputeSet(item.key, item.token, RetrieveFunc(item.key.TrimStart('$'))));
                }
                foreach (var item in RetrieveAllVariables('&'))
                {
                    EnumerationVariables.Add(new ConfigEnumerationSet(item.key, item.token, GetEnumeration(item.key.TrimStart('&'))));
                }
                ReplaceAllVariables();
                IsString = isStringHeader;
            }
            else
            {
                bool.Parse(value);
                IsString = isStringHeader;
            }
        }

        public ConfigSection(string key, List<string> valueList, bool isStringHeader, IComputeFunc computeFuncs, IEnumerationFunc enumerationFuncs) : this(key, string.Join("\n", valueList), isStringHeader, computeFuncs, enumerationFuncs)
        { }

        public List<IConfigSet> ReturnAllVariables()
        {
            var returnList = new List<IConfigSet>();
            returnList.AddRange(ReplaceVariables);
            returnList.AddRange(ComputeVariables);
            returnList.AddRange(EnumerationVariables);
            return returnList;
        }

        private void ReplaceAllVariables()
        {
            ReplaceVariables.ForEach(x => Value = Value.Replace(x.PräfixedName, x.Token));
            ComputeVariables.ForEach(x => Value = Value.Replace(x.PräfixedName, x.Token));
            EnumerationVariables.ForEach(x => Value = Value.Replace(x.PräfixedName, x.Token));
        }

        private Func<string, string> RetrieveFunc(string funcName) => ComputeFuncs.GetFunc(funcName);

        private Func<string, IEnumerable<string>> GetEnumeration(string enumeratorName) => EnumerationFuncs.GetEnumeration(enumeratorName);

        private IEnumerable<(string key, string token)> RetrieveAllVariables(char token)
        {
            List<string> präfixedStrings = new List<string>();
            foreach (var item in IndexOfAll(token))
            {
                präfixedStrings.Add(GetShortestTake(Value.Substring(item)));
            }
            präfixedStrings = präfixedStrings.Distinct().ToList();
            return präfixedStrings.Select(key => { var indexNumber = präfixedStrings.IndexOf(key) + NumberOfVariables; NumberOfVariables++; return (key, GetReplaceToken(indexNumber)); });
        }

        private string GetShortestTake(string subString)
        {
            var endAtDoubleQuote = string.Join(string.Empty, subString.TakeWhile(x => x != '\"'));
            var endAtNewLine = string.Join(string.Empty, subString.TakeWhile(x => x != '\n'));

            if (endAtDoubleQuote.Length < endAtNewLine.Length)
            {
                return endAtDoubleQuote;
            }
            else
            {
                return endAtNewLine;
            }
        }

        private string GetReplaceToken(int position) => $"%{position}$&";

        private List<int> IndexOfAll(char token)
        {
            var foundIndexes = new List<int>();

            for (int i = Value.IndexOf(token); i > -1; i = Value.IndexOf(token, i + 1))
            {
                // for loop end when i=-1 ('a' not found)
                foundIndexes.Add(i);
            }

            return foundIndexes;
        }
    }
}
