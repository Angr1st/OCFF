namespace OCFF
{
    public sealed class ArgumentResult : IArgumentResult
    {
        public string Result { get; }

        public bool IsFound()
            => !string.IsNullOrWhiteSpace(Result);

        public ArgumentResult(string result)
        {
            Result = result;
        }

        public static ArgumentResult Default() => new ArgumentResult(null);
    }

}
