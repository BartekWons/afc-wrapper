using FormatConverter.Core;
using FormatConverter.MVVM.Model;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace FormatConverter.MVVM.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        public RelayCommand DragWindowCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand MinimizeCommand { get; set; }
        public RelayCommand ChooseAudioFileCommand { get; set; }
        public RelayCommand ChooseDestinationPathCommand { get; set; }
        public RelayCommand ExecuteCommand { get; set; }

        private MainFilesExtensions _mainFilesExtensions = new();

        public List<string> AvailableExtensions { get; set; }

        private string _filePath;
        private string _destinationPath;
        private string _fileName;

        public string SelectedExtension
        {
            get { return _mainFilesExtensions.SelectedExtension; }
            set { OnPropertyChanged(); }
        }

        private string _selectedExtensionFromFile;

        public string SelectedExtensionFromFile
        {
            get { return _selectedExtensionFromFile; }
            set 
            { 
                _selectedExtensionFromFile = value;
                OnPropertyChanged();
            }
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

            ChooseAudioFileCommand = new RelayCommand(o =>
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "ALL|*.mp3;*.wav;*.ogg;*.flac;",
                    Multiselect = false
                };
                if (dialog.ShowDialog() == false)
                    return;
                _filePath = dialog.FileName;
                ShowExtensionOnScreen(dialog.SafeFileName);
            });

            ChooseDestinationPathCommand = new RelayCommand(o =>
            {
                var dialog = new OpenFolderDialog()
                {
                    Multiselect = false,
                    InitialDirectory = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"\Downloads\")
                };
                if (dialog.ShowDialog() == false)
                    return;
                _destinationPath = dialog.FolderName;
            });

            ExecuteCommand = new RelayCommand(o =>
            {
                if (String.IsNullOrEmpty(_filePath) || String.IsNullOrEmpty(_destinationPath))
                {
                    MessageBox.Show("Choose existing audio file or destination folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (AreTheSameExtensions())
                {
                    MessageBox.Show("Conversion to the same format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Wrapper.Convert(_filePath, Path.Combine(_destinationPath, $"{_fileName}.{_mainFilesExtensions.SelectedExtension}"));
                MessageBox.Show("Succesfull conversion.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void ShowExtensionOnScreen(string path)
        {
            string fileNameWithExtension = Path.GetFileName(path);
            var matches = Regex.Match(fileNameWithExtension, @"(.*)\.(\w*)");
            if (matches.Success)
            {
                _fileName= matches.Groups[1].Value;
                SelectedExtensionFromFile = matches.Groups[2].Value;
            }
        }

        private bool AreTheSameExtensions()
        {
            var matches = Regex.Match(Path.GetExtension(_filePath), @".(\w*)");
            if (!matches.Success)
                return false;
            return _mainFilesExtensions.SelectedExtension.Equals(matches.Groups[1].Value);
        }
    }
}
