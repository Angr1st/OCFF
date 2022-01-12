using OCFF;
using System;

namespace OCFF_UnitTest
{
    class EmptyArguments : IArguments
    {
        public ArgumentResult GetArgument(string argumentName)
        {
            throw new NotImplementedException();
        }
    }
}
