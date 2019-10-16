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
using CADPadServices.ApplicationServices;
using CADPadServices.Commands.Draw;
using CADPadWPF.Control;
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

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            LinesChainCmd cmd = new LinesChainCmd();
            Drawing.OnCommand(cmd);
        }
    }

   

}
