namespace CADPadServices.Interfaces
{
    public interface IMouseWheelEventArgs : IMouseEventArgs
    {
        int Delta { get; }
    }
}