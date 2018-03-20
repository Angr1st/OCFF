using System;
using System.Collections.Generic;

namespace OCFF
{
    public interface IEnumerationFunc
    {
        Func<string, IEnumerable<string>> GetEnumeration(string funcName);
    }
}
