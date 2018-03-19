namespace TCOCFP
{
    public interface IConfigSet
    {
        string PräfixedName { get; }

        string Name { get; }

        string Token { get; }

        char Präfix { get; }
    }
}
