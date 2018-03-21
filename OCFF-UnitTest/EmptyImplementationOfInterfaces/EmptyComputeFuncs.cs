using System;
using System.Collections.Generic;
using System.Text;
using OCFF;

namespace OCFF_UnitTest
{
    class EmptyComputeFuncs : IComputeFunc
    {
        public Func<string, string> GetFunc(string funcName)
        {
            throw new NotImplementedException();
        }
    }
}
