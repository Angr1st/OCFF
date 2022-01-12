using OCFF;

namespace OCFF_UnitTest
{
    class TestArgument : IArguments
    {
        public string TestFunc;

        public TestArgument(string test) => TestFunc = test;

        public ArgumentResult GetArgument(string argumentName)
        {
            switch (argumentName)
            {
                case nameof(TestFunc):
                    return new ArgumentResult(TestFunc);
                default:
                    return ArgumentResult.Default();
            }
        }
    }
}
