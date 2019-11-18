using CADPadServices.Commands.Draw;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CADPadWPF.Helpers
{
    public class DrawingCommands
    {
        MainWindow _win;
        public DrawingCommands(MainWindow win)
        {
            _win = win;

            _win.CommandBindings.Add(new CommandBinding(LinesChainCommand,
             (object sender, ExecutedRoutedEventArgs e) =>
             {
                 var cmd = new LinesChainCmd();
                 _win.Drawing.OnCommand(cmd);
             }, null));

            _win.CommandBindings.Add(new CommandBinding(CircleCommand,
             (object sender, ExecutedRoutedEventArgs e) =>
              {
                  var cmd = new CircleCmd();
                  _win.Drawing.OnCommand(cmd);
              }, null));

            _win.CommandBindings.Add(new CommandBinding(EllipseCommand,
            (object sender, ExecutedRoutedEventArgs e) =>
            {
                var cmd = new EllipseCmd();
                _win.Drawing.OnCommand(cmd);
            }, null));

            _win.CommandBindings.Add(new CommandBinding(ArcCommand,
             (object sender, ExecutedRoutedEventArgs e) =>
             {
                 var cmd = new ArcCmd();
                 _win.Drawing.OnCommand(cmd);
             }, null));

            _win.CommandBindings.Add(new CommandBinding(XlineCommand,
             (object sender, ExecutedRoutedEventArgs e) =>
             {
                 var cmd = new XlineCmd();
                 _win.Drawing.OnCommand(cmd);
             }, null));

            _win.CommandBindings.Add(new CommandBinding(RectangleCommand,
             (object sender, ExecutedRoutedEventArgs e) =>
             {
                 var cmd = new RectangleCmd();
                 _win.Drawing.OnCommand(cmd);
             }, null));

            _win.CommandBindings.Add(new CommandBinding(PolylineCommand,
             (object sender, ExecutedRoutedEventArgs e) =>
             {
                 var cmd = new PolylineCmd();
                 _win.Drawing.OnCommand(cmd);
             }, null));

            _win.CommandBindings.Add(new CommandBinding(RayCommand,
             (object sender, ExecutedRoutedEventArgs e) =>
             {
                 var cmd = new RayCmd();
                 _win.Drawing.OnCommand(cmd);
             }, null));
        }

        public RoutedCommand LinesChainCommand { get; set; } = new RoutedCommand();
        public RoutedCommand CircleCommand { get; set; } = new RoutedCommand();
        public RoutedCommand EllipseCommand { get; set; } = new RoutedCommand();
        public RoutedCommand ArcCommand { get; set; } = new RoutedCommand();
        public RoutedCommand XlineCommand { get; set; } = new RoutedCommand();
        public RoutedCommand RectangleCommand { get; set; } = new RoutedCommand();
        public RoutedCommand PolylineCommand { get; set; } = new RoutedCommand();
        public RoutedCommand RayCommand { get; set; } = new RoutedCommand();
    }
}
