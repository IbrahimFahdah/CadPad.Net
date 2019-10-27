namespace CADPadServices.Interfaces
{
    public interface IKeyEventArgs 
    {
        bool IsEscape { get; }
        bool IsDelete { get; }
    }
}