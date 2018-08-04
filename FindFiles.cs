using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WallpaperManager
{
    public class FindFiles
    {
        public static void DoFindFiles()
        {
            var dbFile = GetDbFile();
	        if (dbFile == "")
		        return;

	        var targetDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\ToolOutput";
	        if (Directory.Exists(targetDirectory))
		        Directory.Delete(targetDirectory, true);

            var allLines = File.ReadAllLines(dbFile);
            var basePath = Path.GetDirectoryName(dbFile) + "\\";

	        var targetFiles = allLines
		        .Skip(1)
		        .Select(WallpaperData.Parse)
		        .Where(x =>
			        !x.funny &&
			        !x.edgy &&
			        !x.gaming &&
			        !x.christmas &&
			        !x.dark &&
			        x.HasProperty(WallpaperData.Environment.urban)).ToArray();

	        if (targetFiles.Length == 0)
		        return;

            Directory.CreateDirectory(targetDirectory);

            var counter = 0;
            foreach (var f in targetFiles)
            {
                var sourcefile = basePath + f.filename;
                var targetFile = targetDirectory + $"\\{++counter}." + Path.GetFileName(sourcefile).Split('.')[1];
                File.Copy(sourcefile, targetFile);
            }
        }

        private static string GetDbFile()
        {
            using (var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
            })
	        return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : "";
        }
    }
}
