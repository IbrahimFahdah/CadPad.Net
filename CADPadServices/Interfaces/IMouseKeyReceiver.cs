using CADPadServices.Commands;

namespace CADPadServices.Interfaces
{
    public interface IMouseKeyReceiver
    {
        IEventResult OnMouseDown(IMouseButtonEventArgs e);

        IEventResult OnMouseUp(IMouseButtonEventArgs e);

        IEventResult OnMouseMove(IMouseEventArgs e);

        IEventResult OnMouseWheel(IMouseWheelEventArgs e);

        IEventResult OnMouseDoubleClick(IMouseEventArgs e);

        IEventResult OnKeyDown(IKeyEventArgs KeyEventArgs);

        IEventResult OnKeyUp(IKeyEventArgs KeyEventArgs);
    }
}