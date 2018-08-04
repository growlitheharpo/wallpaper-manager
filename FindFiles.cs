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
            var allLines = File.ReadAllLines(dbFile);
	        var query = allLines
		        .Skip(1)
		        .Select(WallpaperData.Parse);

            var basePath = Path.GetDirectoryName(dbFile) + "\\";
            var files = query.Where(x => 
	            !x.funny &&
				!x.edgy &&
				!x.gaming &&
				!x.christmas &&
				!x.dark &&
	            x.environment.Contains(WallpaperData.Environment.urban));

            var targetDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\ToolOutput";
            if (Directory.Exists(targetDirectory))
                Directory.Delete(targetDirectory, true);

            Directory.CreateDirectory(targetDirectory);

            int counter = 1;
            foreach (var f in files)
            {
                var sourcefile = basePath + f.filename;
                var targetFile = targetDirectory + $"\\{counter}." + Path.GetFileName(sourcefile).Split('.')[1];
                File.Copy(sourcefile, targetFile);
                counter++;
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
