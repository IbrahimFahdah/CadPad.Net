using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace CADPadWPF.Helpers
{
    public class WindowTitle : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string _fileName = null;
        public string Title
        {
            get
            {
                return $"CADPadWPF - [{_fileName}]";
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Title));
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
