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
                (object sender, CanExecuteRoutedEventArgs e) => { e.CanExecute = Drawing.CanUndo;}));

            this.CommandBindings.Add(new CommandBinding(RedoCommand,
               (object sender, ExecutedRoutedEventArgs e) =>
               {
                   var cmd = new RedoCmd();
                   Drawing.OnCommand(cmd);
               },
               (object sender, CanExecuteRoutedEventArgs e) => { e.CanExecute = Drawing.CanRedo; }));

            SaveFileCommand = new CADPadDrawingCommand(SaveFileCommand_Executed);
            SaveAsFileCommand = new CADPadDrawingCommand(SaveAsFileCommand_Executed);
            OpenFileCommand = new CADPadDrawingCommand(OpenFileCommand_Executed);
            NewFileCommand = new CADPadDrawingCommand(NewFileCommand_Executed);

            DataContext = this;
        }

        public Drawing Drawing { get; set; }
        public WindowTitle WindowTitle { get; set; } = new WindowTitle();
        //public string FileName
        //{
        //    get
        //    {
        //        return _fileName;
        //    }
        //    set
        //    {
        //        _fileName = value;
        //        NotifyPropertyChanged();
        //    }
        //}

        #region commands
        public RoutedCommand UndoCommand { get; set; } = new RoutedCommand();
        public RoutedCommand RedoCommand { get; set; } = new RoutedCommand();

        public ICommand SaveFileCommand { get; set; }
        public ICommand SaveAsFileCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand NewFileCommand { get; set; }
        #endregion

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            sCAD.OnKeyDown2(e);
        }

        private void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            sCAD.OnKeyUp2(e);
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var cmd = new UndoCmd();
            Drawing.OnCommand(cmd);
        }


        private void Redo_Executed()
        {
            var cmd = new RedoCmd();
            Drawing.OnCommand(cmd);
        }

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

        #region save drawing 

        private void SaveFileCommand_Executed()
        {
            SaveDocumentAsFile(WindowTitle.FileName);
        }

        private void SaveAsFileCommand_Executed()
        {
            SaveDocumentAsFile(null);
        }

        public bool SaveDocumentAsFile(string fileName)
        {
            // If no filename was specified, prompt the user for one.
            if (fileName == null)
            {
                // Create a File | Save As... dailog.
                Microsoft.Win32.SaveFileDialog dialog;
                dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.CheckFileExists = false;
                dialog.Filter = " (*.xml)|*.xml";

                // Display the dialog and wait for the user response.
                bool result = (bool)dialog.ShowDialog(null);

                // If the user clicked "Cancel", cancel the saving the file.
                if (result == false) return false;
                fileName = dialog.FileName;
                WindowTitle.FileName = fileName;
            }

            // Save the document to the specified file.
            return SaveToFile(fileName);
        }

        /// <summary>
        ///   Saves the current document to a specified file.</summary>
        /// <param name='fileName'>
        ///   The name of file to save to.</param>
        /// <returns>
        ///   true if the document was saved successfully; otherwise, false
        ///   if there was an error or the user canceled the save.</returns>
        private bool SaveToFile(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            // If the file already exists, delete it (replace).
            if (File.Exists(fileName)) File.Delete(fileName);

            try
            {
                if (fileName.EndsWith(".xml"))
                {
                    Stream package = File.Create(fileName);
                    NativeDrawingSerializer serializer = new NativeDrawingSerializer(package);
                    serializer.Write(Drawing);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Error occurred during a conversion to this file format: " +
                    fileName + "\n" + e.ToString(), this.GetType().Name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;

        }
        #endregion

        #region open drawing 
        private void OpenFileCommand_Executed()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = "CADPad Documents (*.xml)|*.xml";
            bool result = (bool)dialog.ShowDialog(null);
            if (result == false) return;

            WindowTitle.FileName = dialog.FileName;

            OpenFile(WindowTitle.FileName);
        }
        private void OpenFile(string fileName)
        {
            using (FileStream inputStream = File.OpenRead(fileName))
            {
                NativeDrawingDeserializer deserializer = new NativeDrawingDeserializer(inputStream);
                deserializer.Read(Drawing);
                Drawing.Rebuild();
            }
        }
        #endregion

        #region new opening
        private void NewFileCommand_Executed()
        {
            WindowTitle.FileName = null;
            Drawing.Clear();
        }
        #endregion
    }



}
