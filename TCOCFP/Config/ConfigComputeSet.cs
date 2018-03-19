using System;

namespace TCOCFP
{
    class ConfigComputeSet : IConfigSet
    {
        public string PräfixedName { get; }

        public string Name { get; }

        public string Token { get; }

        private Func<string, string> Func { get; }

        public char Präfix { get; }

        public ConfigComputeSet(string präfixedName, string token, Func<string, string> func, char präfix = '$')
        {
            PräfixedName = präfixedName;
            Präfix = präfix;
            Name = präfixedName.TrimStart(präfix);
            Token = token;
            Func = func;
        }

        public string Compute(string text) => Func.Invoke(text);

    }
}
