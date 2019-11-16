using CADPadDrawing.Canvas;
using System;
using System.Windows.Input;

namespace CADPadWPF.Helpers
{

    public class CADPadDrawingCommand : ICommand
    {
        public CADPadDrawingCommand(Action action, Func<bool> canAction =null)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            _canAction = canAction;
            this.action = action;
        }

        private readonly Action action;
        private readonly Func<bool> _canAction;

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            action?.Invoke();
        }

        public bool CanExecute(object parameter)
        {
            if (_canAction == null)
                return true;
            return _canAction.Invoke();
        }
    }
}
