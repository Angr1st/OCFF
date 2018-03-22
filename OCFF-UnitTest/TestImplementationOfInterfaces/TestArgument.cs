using OCFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCFF_UnitTest
{
    class TestArgument : IArguments
    {
        public string TestFunc;

        public TestArgument(string test) => TestFunc = test;

        public string GetArgument(string argumentName)
        {
            switch (argumentName)
            {
                case nameof(TestFunc):
                    return TestFunc;
                default:
                    throw new ArgumentException(nameof(argumentName));
            }
        }
    }
}
