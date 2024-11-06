using System.Runtime.Intrinsics.X86;

namespace FormatConverter.MVVM.Model
{
    public class MainFilesExtensions
    {
        public List<string> AvailableExtensions { get; set; }
        public int SelectedIndex { get; set; }
        public string SelectedExtension { get => AvailableExtensions[SelectedIndex]; }

        public MainFilesExtensions()
        {
            AvailableExtensions = ["mp3", "wav", "ogg", "flac"];
            SelectedIndex = 0;
        }
    }
}
