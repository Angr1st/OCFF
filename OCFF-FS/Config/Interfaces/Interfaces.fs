namespace OCFF_FS

open System.Runtime.CompilerServices

type IArguments =
    abstract member GetArgument: string -> string

type IComputeFunc =
    abstract member GetFunc: string -> System.Func<string,string>
    
type IConfigSet =
    abstract member PrefixedName: string with get

    abstract member Name: string with get

    abstract member Token: string with get

    abstract member Prefix: char with get

type IEnumerationFunc =
    abstract member GetEnumeration: string -> System.Func<string,System.Collections.Generic.List<string>>

[<Extension>]
type public FSharpFuncUtil = 

    [<Extension>] 
    static member ToFSharpFunc<'a,'b> (func:System.Converter<'a,'b>) = fun x -> func.Invoke(x)

    [<Extension>] 
    static member ToFSharpFunc<'a,'b> (func:System.Func<'a,'b>) = fun x -> func.Invoke(x)

    [<Extension>] 
    static member ToFSharpFunc<'a,'b,'c> (func:System.Func<'a,'b,'c>) = fun x y -> func.Invoke(x,y)

    [<Extension>] 
    static member ToFSharpFunc<'a,'b,'c,'d> (func:System.Func<'a,'b,'c,'d>) = fun x y z -> func.Invoke(x,y,z)

    [<Extension>] 
    static member ToFSharpFunc<'a,'b,'c,'d,'e> (func:System.Func<'a,'b,'c,'d,'e>) = fun x y z a -> func.Invoke(x,y,z,a)

    [<Extension>] 
    static member ToFSharpFunc<'a,'b,'c,'d,'e,'f> (func:System.Func<'a,'b,'c,'d,'e,'f>) = fun x y z a b -> func.Invoke(x,y,z,a,b)

    static member Create<'a,'b> (func:System.Func<'a,'b>) = FSharpFuncUtil.ToFSharpFunc func

    static member Create<'a,'b,'c> (func:System.Func<'a,'b,'c>) = FSharpFuncUtil.ToFSharpFunc func

    static member Create<'a,'b,'c,'d> (func:System.Func<'a,'b,'c,'d>) = FSharpFuncUtil.ToFSharpFunc func

    static member Create<'a,'b,'c,'d,'e> (func:System.Func<'a,'b,'c,'d,'e>) = FSharpFuncUtil.ToFSharpFunc func

    static member Create<'a,'b,'c,'d,'e,'f> (func:System.Func<'a,'b,'c,'d,'e,'f>) = FSharpFuncUtil.ToFSharpFunc func