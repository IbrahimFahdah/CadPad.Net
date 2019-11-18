using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CADPadServices.Serilization;
namespace CADPadWPF.Helpers
{
    public class FileMenuCommands
    {
        MainWindow _win;
        public FileMenuCommands(MainWindow win)
        {
            _win = win;

            SaveFileCommand = new CADPadDrawingCommand(SaveFileCommand_Executed);
            SaveAsFileCommand = new CADPadDrawingCommand(SaveAsFileCommand_Executed);
            OpenFileCommand = new CADPadDrawingCommand(OpenFileCommand_Executed);
            NewFileCommand = new CADPadDrawingCommand(NewFileCommand_Executed);

         

            _win.CommandBindings.Add(new CommandBinding(ExitCommand,
             (object sender, ExecutedRoutedEventArgs e) =>
             {
                 ShutDown();
             }, null));

            
        }

        public RoutedCommand ExitCommand { get; set; } = new RoutedCommand();

        public ICommand SaveFileCommand { get; set; }
        public ICommand SaveAsFileCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand NewFileCommand { get; set; }

        #region save drawing 

        private void SaveFileCommand_Executed()
        {
            SaveDocumentAsFile(_win.WindowTitle.FileName);
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
                _win.WindowTitle.FileName = RequestFileName();
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
                    NativeDrawingContentSerializer serializer = new NativeDrawingContentSerializer(package);
                    serializer.Write(_win.Drawing);
                }
                if (fileName.EndsWith(".dxf"))
                {
                    Stream package = File.Create(fileName);
                    DXFDrawingContentSerializer serializer = new DXFDrawingContentSerializer(package);
                    serializer.Write(_win.Drawing);
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

        private string RequestFileName()
        {
            // Create a File | Save As... dailog.
            Microsoft.Win32.SaveFileDialog dialog;
            dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.CheckFileExists = false;
            dialog.Filter = "CADPad Documents (*.cpad)|*.cpad|DXF (*.dxf)|*.dxf";

            // Display the dialog and wait for the user response.
            bool result = (bool)dialog.ShowDialog(null);

            // If the user clicked "Cancel", cancel the saving the file.
            if (result == false) return null;
            return dialog.FileName;
        }
        #endregion

        #region open drawing 
        private void OpenFileCommand_Executed()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = "CADPad Documents (*.cpad)|*.xml|DXF (*.dxf)|*.dxf";
            bool result = (bool)dialog.ShowDialog(null);
            if (result == false) return;

            _win.WindowTitle.FileName = dialog.FileName;

            OpenFile(_win.WindowTitle.FileName);
        }
        private void OpenFile(string fileName)
        {
            using (FileStream inputStream = File.OpenRead(fileName))
            {
                if (fileName.EndsWith(".cpad"))
                {
                    NativeDrawingContentDeserializer deserializer = new NativeDrawingContentDeserializer(inputStream);
                    deserializer.Read(_win.Drawing);
                }
                else if (fileName.EndsWith(".dxf"))
                {
                    DXFDrawingContentDeserializer deserializer = new DXFDrawingContentDeserializer(inputStream);
                    deserializer.Read(_win.Drawing);
                }
                _win.Drawing.Rebuild();
            }
        }
        #endregion

        #region new opening
        private void NewFileCommand_Executed()
        {
            _win.WindowTitle.FileName = null;
            _win.Drawing.Clear();
        }
        #endregion

        private void ShutDown()
        {
            var rs =MessageBox.Show(
                   "Do you like to save changes?" , "Save changes",
                   MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if(rs==MessageBoxResult.Cancel)
            {
                return;
            }
            else if (rs == MessageBoxResult.Yes)
            {
                if (_win.WindowTitle.HasFileName)
                {
                    SaveAsFileCommand_Executed();
                }
                else
                {
                    var filename=RequestFileName();
                    if (filename == null)
                    {
                        return;
                    }
                    else
                    {
                        SaveDocumentAsFile(filename);
                    }
                }
            }

            if (Application.Current != null)
                Application.Current.Shutdown();
        }
    }
}
