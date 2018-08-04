using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace WallpaperManager
{
    public class FindFiles
    {
        public static void DoFindFiles()
        {
            var dbFile = GetDbFile();
            var allLines = File.ReadAllLines(dbFile);

            var query = from line in allLines.Skip(1)
                let data = line.Split(',')
                select new
                {
                    fileName = data[0],
                    pale = bool.Parse(data[1]),
                    color = new[] {data[2].ToLower(), data[3].ToLower()},
                    dark = bool.Parse(data[4]),
                    season = data[5].ToLower(),
                    environment1 = data[6].ToLower(),
                    environment2 = data[7].ToLower(),
                    environment = new[] { data[6].ToLower(), data[7].ToLower() },
                    funny = bool.Parse(data[8]),
                    inappropriate = bool.Parse(data[9]),
                    photograph = bool.Parse(data[10]),
                    food = bool.Parse(data[11]),
                    edgy = bool.Parse(data[12]),
                    gaming = bool.Parse(data[13]),
                    christmas = bool.Parse(data[14]),
                };

            var basePath = Path.GetDirectoryName(dbFile) + "\\";
            var files = query.Where(x => !x.pale 
				&& !x.dark
				&& !x.funny 
				&& !x.edgy 
				&& x.season != "winter"
				&& x.season != "fall");

            var targetDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\ToolOutput";
            if (Directory.Exists(targetDirectory))
                Directory.Delete(targetDirectory, true);

            Directory.CreateDirectory(targetDirectory);

            int counter = 1;
            foreach (var f in files)
            {
                var sourcefile = basePath + f.fileName;
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
            {
                //return dialog.ShowDialog() == DialogResult.OK ? "" : dialog.FileName;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }
            }

            return "";
        }
    }
}