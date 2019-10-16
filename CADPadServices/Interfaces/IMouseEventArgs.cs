namespace CADPadServices.Interfaces
{
    public interface IMouseEventArgs
    {
        bool IsMiddlePressed { get; }
        bool IsLeftPressed { get; }
        bool IsRightPressed { get; }
        double X { get; }
        double Y { get; }

        object InnerArgs { get; }
    }
}
