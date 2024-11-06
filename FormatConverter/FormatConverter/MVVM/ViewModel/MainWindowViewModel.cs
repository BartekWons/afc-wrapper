using Accessibility;
using FormatConverter.Core;
using FormatConverter.MVVM.Model;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace FormatConverter.MVVM.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        public RelayCommand DragWindowCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand MinimizeCommand { get; set; }
        public RelayCommand ChooseAudioFile { get; set; }

        private MainFilesExtensions _mainFilesExtensions = new();

        public List<string> AvailableExtensions { get; set; }


        public string SelectedExtension
        {
            get { return _mainFilesExtensions.SelectedExtension; }
            set { OnPropertyChanged(); }
        }

        public string SelectedExtensionFromFile
        {
            get { return null; }
            set { OnPropertyChanged(); }
        }

        public int SelectedIndex
        {
            get => _mainFilesExtensions.SelectedIndex;
            set 
            {
                _mainFilesExtensions.SelectedIndex = value;
                SelectedExtension = _mainFilesExtensions.SelectedExtension;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            AvailableExtensions = _mainFilesExtensions.AvailableExtensions;
            DragWindowCommand = new RelayCommand(o =>
            {
                Application.Current.MainWindow.DragMove();
            });

            CloseCommand = new RelayCommand(o => 
            {
                Application.Current.MainWindow.Close();
            });

            MinimizeCommand = new RelayCommand(o =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });

            ChooseAudioFile = new RelayCommand(o =>
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "ALL|*.mp3;*.wav;*.ogg;*.flac;",
                    Multiselect = false
                };
                if (dialog.ShowDialog() == true)
                {
                    Debug.WriteLine(dialog.SafeFileName);
                    string extension = Path.GetExtension(dialog.SafeFileName);
                    Debug.WriteLine(extension);
                    SelectedExtensionFromFile = extension;
                }
            });
        }
    }
}
