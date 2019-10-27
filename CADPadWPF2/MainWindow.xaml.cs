using System;
using System.Collections.Generic;
using System.Linq;
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
using CADPadServices.Commands.Modify;
using CADPadWPF2;
using Drawing = CADPadServices.Drawing;

namespace CADPadWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var _document = new Document();
            Drawing = new Drawing(_document);
            Drawing.Name = "My drawing";

            this.KeyDown += MainWindow_KeyDown;
            this.KeyUp += MainWindow_KeyUp;
        }

        public Drawing Drawing { get; set; }

    
        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
             sCAD.OnKeyDown2(e);
        }

        private void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            sCAD.OnKeyUp2(e);
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
        private void DrawLine_OnClick(object sender, RoutedEventArgs e)
        {
            DrawLine_Executed();
        }
        private void DrawCircle_OnClick(object sender, RoutedEventArgs e)
        {
            var cmd = new CircleCmd();
            Drawing.OnCommand(cmd);
        }
        private void DrawEllipse_OnClick(object sender, RoutedEventArgs e)
        {
            var cmd = new EllipseCmd();
            Drawing.OnCommand(cmd);
        }
        private void DrawArc_OnClick(object sender, RoutedEventArgs e)
        {
            var cmd = new ArcCmd();
            Drawing.OnCommand(cmd);
        }

        private void DrawXLine_OnClick(object sender, RoutedEventArgs e)
        {
            var cmd = new XlineCmd();
            Drawing.OnCommand(cmd);
        }

        private void DrawRectangle_OnClick(object sender, RoutedEventArgs e)
        {
            var cmd = new RectangleCmd();
            Drawing.OnCommand(cmd);
        }

        private void DrawPloyline_OnClick(object sender, RoutedEventArgs e)
        {
            var cmd = new PolylineCmd();
            Drawing.OnCommand(cmd);
        }

        private void DrawRay_OnClick(object sender, RoutedEventArgs e)
        {
            var cmd = new RayCmd();
            Drawing.OnCommand(cmd);
        }

      private void EditGridOptions_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new GridLayoutOptions();
            win.ShowDialog();
        }


        #region Commands
        private void DrawLine_Executed()
        {
            var cmd = new LinesChainCmd();
            Drawing.OnCommand(cmd);
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
        #endregion
    }



}
