using System.Windows.Input;
using CADPadServices.Interfaces;
using MouseButton = CADPadServices.Enums.MouseButton;
using MouseButtonState = CADPadServices.Enums.MouseButtonState;

namespace CADPadWPF.Control.Canvas
{
    internal class CanvasMouseButtonEventArgs : CanvasMouseEventArgs, IMouseButtonEventArgs
    {
        public CanvasMouseButtonEventArgs(MouseButtonEventArgs e, CADPadCanvas parent) : base(e, parent)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                ChangedButton = MouseButton.Left;
            if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
                ChangedButton = MouseButton.Right;
            if (e.ChangedButton == System.Windows.Input.MouseButton.Middle)
                ChangedButton = MouseButton.Middle;
            if (e.ChangedButton == System.Windows.Input.MouseButton.XButton1)
                ChangedButton = MouseButton.XButton1;
            if (e.ChangedButton == System.Windows.Input.MouseButton.XButton2)
                ChangedButton = MouseButton.XButton2;

            if (e.ButtonState ==System.Windows.Input.MouseButtonState.Pressed)
                ButtonState = MouseButtonState.Pressed;
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Released)
                ButtonState = MouseButtonState.Released;
        }

        public MouseButton ChangedButton { get; }
        public MouseButtonState ButtonState { get; }
        public bool IsShiftKeyDown()
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                return true;
            }
            return false;
        }
    }
}