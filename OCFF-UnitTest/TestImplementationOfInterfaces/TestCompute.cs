using OCFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCFF_UnitTest
{
    class TestCompute : IComputeFunc
    {
        private Func<string,string> TestFunc()
        {
            return (x) => $"TestPlus;{x}";
        }

        public Func<string, string> GetFunc(string funcName)
        {
            switch (funcName)
            {
                case nameof(TestFunc):
                    return TestFunc();
                default:
                    throw new ArgumentException(nameof(funcName));
            }
        }
    }
}
