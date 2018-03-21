using System;
using System.Collections.Generic;
using System.Text;
using OCFF;

namespace OCFF_UnitTest
{
    class EmptyArguments : IArguments
    {
        public string GetArgument(string argumentName)
        {
            throw new NotImplementedException();
        }
    }
}
