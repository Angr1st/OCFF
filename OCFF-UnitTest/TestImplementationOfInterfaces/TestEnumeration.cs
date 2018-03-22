using OCFF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCFF_UnitTest
{
    class TestEnumeration : IEnumerationFunc
    {
        public Func<string,IEnumerable<string>> TestEnumFunc()
        {
            return (x) => new List<string> { x,x,x };
        }

        public Func<string, IEnumerable<string>> GetEnumeration(string funcName)
        {
            switch (funcName)
            {
                case nameof(TestEnumFunc):
                    return TestEnumFunc();
                default:
                    throw new ArgumentException(nameof(funcName));
            }
        }
    }
}
