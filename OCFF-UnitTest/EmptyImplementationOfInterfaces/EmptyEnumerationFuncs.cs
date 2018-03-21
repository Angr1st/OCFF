using System;
using System.Collections.Generic;
using System.Text;
using OCFF;

namespace OCFF_UnitTest
{
    class EmptyEnumerationFuncs : IEnumerationFunc
    {
        public Func<string, IEnumerable<string>> GetEnumeration(string funcName)
        {
            throw new NotImplementedException();
        }
    }
}
