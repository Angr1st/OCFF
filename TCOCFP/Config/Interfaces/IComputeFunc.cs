using System;

namespace TCOCFP
{
    public interface IComputeFunc
    {
        Func<string, string> GetFunc(string funcName);
    }
}
