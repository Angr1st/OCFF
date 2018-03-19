using System;
using System.Collections.Generic;

namespace TCOCFP
{
    public interface IEnumerationFunc
    {
        Func<string, IEnumerable<string>> GetEnumeration(string funcName);
    }
}
