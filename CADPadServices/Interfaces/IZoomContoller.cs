namespace CADPadServices.Interfaces
{
    public interface IZoomContoller : IMouseKeyReceiver
    {
        double Scale { get; set; }
    }
}