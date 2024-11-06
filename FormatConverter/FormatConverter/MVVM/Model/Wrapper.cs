using System.Diagnostics;

namespace FormatConverter.MVVM.Model
{
    public class Wrapper
    {
        public static void Convert(string filePath, string outputPath)
        {
            string ffmpegCommand = @$"-i {filePath} {outputPath}";
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "ffmpeg",
                Arguments = ffmpegCommand,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
        }
    }
}
