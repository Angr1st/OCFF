using System;

namespace OCFF
{
    public interface IComputeFunc
    {
        Func<string, string> GetFunc(string funcName);
    }
}
