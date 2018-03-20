using System;
using System.Collections.Generic;

namespace OCFF
{
    class ConfigEnumerationSet : IConfigSet
    {
        public string PräfixedName { get; }

        public string Name { get; }

        public string Token { get; }

        private Func<string, IEnumerable<string>> Func { get; }

        public char Präfix { get; }

        public ConfigEnumerationSet(string präfixedName, string token, Func<string, IEnumerable<string>> func, char präfix = '&')
        {
            PräfixedName = präfixedName;
            Token = token;
            Func = func;
            Präfix = präfix;
            Name = präfixedName.TrimStart(präfix);
        }

        public IEnumerable<string> GetEnumerable(string parameter) => Func.Invoke(parameter);
    }
}
