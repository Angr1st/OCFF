﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using OCFF_FS;

namespace OCFF_FS_Test
{
    class Argument : IArguments
    {
        public string GetArgument(string value)
        {
            throw new NotImplementedException();
        }
    }

    class ConfigSet : IConfigSet
    {
        public string PrefixedName => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string Token => throw new NotImplementedException();

        public char Prefix => throw new NotImplementedException();
    }

    class EnumerationFunc : IEnumerationFunc
    {
        public Func<string, List<string>> GetEnumeration(string value)
        {
            throw new NotImplementedException();
        }

        public FSharpFunc<string, FSharpList<string>> GetEnumerationFs(string value)
        {
            return innerFunc().ToFSharpFunc();

            Func<string, FSharpList<string>> innerFunc() => (innerValue) =>new FSharpList<string>(innerValue, FSharpList<string>.Empty);
        }
    }

    class ComputationFunc : IComputeFunc
    {
        public FSharpFunc<string, string> GetFuncFs(string value)
        {
            throw new NotImplementedException();
        }

        Func<string, string> IComputeFunc.GetFunc(string value)
        {
            throw new NotImplementedException();
        }
    }
}
