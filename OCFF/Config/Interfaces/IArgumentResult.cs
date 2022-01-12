namespace OCFF
{
    public interface IArgumentResult
    {
        bool IsFound();
        string Result { get; }
    }
}
