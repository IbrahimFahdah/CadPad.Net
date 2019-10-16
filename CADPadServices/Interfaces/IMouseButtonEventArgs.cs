using CADPadServices.Enums;

namespace CADPadServices.Interfaces
{
    public interface IMouseButtonEventArgs: IMouseEventArgs
    {
        MouseButton ChangedButton { get; }
        MouseButtonState ButtonState { get; }
        bool IsShiftKeyDown();
    }
}