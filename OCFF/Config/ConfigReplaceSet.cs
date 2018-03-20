namespace OCFF
{
    class ConfigReplaceSet : IConfigSet
    {
        public string PräfixedName { get; }

        public string Name { get; }

        public string Token { get; }

        public char Präfix { get; }

        public ConfigReplaceSet(string präfixedName, string token, char präfix = '@')
        {
            PräfixedName = präfixedName;
            Präfix = präfix;
            Name = präfixedName.TrimStart(präfix);
            Token = token;
        }
    }
}
