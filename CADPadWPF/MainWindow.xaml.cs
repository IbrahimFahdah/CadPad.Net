using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CADPadServices;
using CADPadServices.Commands.Draw;
using CADPadServices.Commands.Edit;
using CADPadServices.Commands.Modify;
using CADPadServices.Serilization;
using CADPadWPF.Helpers;
using CADPadWPF2;
using Drawing = CADPadServices.Drawing;

namespace CADPadWPF
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();

            var _document = new Document();
            Drawing = new Drawing(_document);
            Drawing.Name = "My drawing";

            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;

            this.CommandBindings.Add(new CommandBinding(UndoCommand,
                (object sender, ExecutedRoutedEventArgs e) =>
                 {
                     var cmd = new UndoCmd();
                     Drawing.OnCommand(cmd);
                 },
                (object sender, CanExecuteRoutedEventArgs e) => { e.CanExecute = Drawing.CanUndo; }));

            this.CommandBindings.Add(new CommandBinding(RedoCommand,
               (object sender, ExecutedRoutedEventArgs e) =>
               {
                   var cmd = new RedoCmd();
                   Drawing.OnCommand(cmd);
               },
               (object sender, CanExecuteRoutedEventArgs e) => { e.CanExecute = Drawing.CanRedo; }));


            this.CommandBindings.Add(new CommandBinding(AboutDialogCommand,
            (object sender, ExecutedRoutedEventArgs e) =>
            {
                var win = new About();
                win.Owner = this;
                win.ShowDialog();
            },
            null));

            FileMenuCommands = new FileMenuCommands(this);
            DrawingCommands = new DrawingCommands(this);
            CenterAndZoomCommand = new CADPadDrawingCommand(() =>
            {
                FileMenuCommands.CenterAndZoomDrawing(150);
            });

            DataContext = this;
        }

        private Drawing _drawing;
        public Drawing Drawing {
            get => _drawing;
            set {
                _drawing = value;
                NotifyPropertyChanged("Drawing");
            }            
        }

        public WindowTitle WindowTitle { get; set; } = new WindowTitle();
        public FileMenuCommands FileMenuCommands { get; set; }
        public DrawingCommands DrawingCommands { get; set; }

        #region commands
        public RoutedCommand UndoCommand { get; set; } = new RoutedCommand();
        public RoutedCommand RedoCommand { get; set; } = new RoutedCommand();
        public RoutedCommand AboutDialogCommand { get; set; } = new RoutedCommand();
        public CADPadDrawingCommand CenterAndZoomCommand { get; set; }

        #endregion

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            canvas.OnKeyDown2(e);
        }

        private void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            canvas.OnKeyUp2(e);            
        }

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void EditGridOptions_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new GridLayoutOptions();
            win.ShowDialog();
        }

        private void DeleteEntities_OnClick(object sender, RoutedEventArgs e)
        {
            DeleteEntities_Executed();
        }
        private void CopyEntities_OnClick(object sender, RoutedEventArgs e)
        {
            CopyEntities_Executed();
        }
        private void MoveEntities_OnClick(object sender, RoutedEventArgs e)
        {
            MoveEntities_Executed();
        }
        private void MirrorEntities_OnClick(object sender, RoutedEventArgs e)
        {
            MirrorEntities_Executed();
        }
        private void CopyEntities_Executed()
        {
            var cmd = new CopyCmd();
            Drawing.OnCommand(cmd);
        }
        private void MoveEntities_Executed()
        {
            var cmd = new MoveCmd();
            Drawing.OnCommand(cmd);
        }
        private void MirrorEntities_Executed()
        {
            var cmd = new MirrorCmd();
            Drawing.OnCommand(cmd);
        }
        private void DeleteEntities_Executed()
        {
            var cmd = new DeleteCmd();
            Drawing.OnCommand(cmd);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            
            System.Diagnostics.Trace.WriteLine($"Layer {checkbox.Content} visibility set to {checkbox.IsChecked}");
            if (!checkbox.IsChecked??false) 
            {
                this.Drawing.CurrentBlock.Where(a => a.Layer == checkbox.Content.ToString()).ToList().ForEach(a => Drawing.Canvas.RemoveVisual(a.DrawingVisual));
            }
            else
            {
                this.Drawing.CurrentBlock.Where(a => a.Layer == checkbox.Content.ToString()).ToList().ForEach(a => Drawing.Canvas.AddVisual(a.DrawingVisual));
            }
            Drawing.Canvas.Redraw();
        }

        private void GridCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            if (!checkbox.IsChecked ?? false)
            {
                Drawing.Canvas?.Clear();
                Drawing.GridLayer.GridStyle = CADPadServices.Enums.GridStyle.None;                
            }            
            else
            {
                Drawing.GridLayer.GridStyle = CADPadServices.Enums.GridStyle.Lines;
            }
            Drawing.Canvas?.Redraw();
        }
    }
}
