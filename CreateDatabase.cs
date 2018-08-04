using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WallpaperManager
{
    public static class CreateDatabase
    {
        private static string[] catgories =
        {
            "Pale",             //TRUE, FALSE
            "Color1",           //RED, BLUE, GREEN, YELLOW, ORANGE, PURPLE, BLACK, WHITE, GREY
            "Color2",           //RED, BLUE, GREEN, YELLOW, ORANGE, PURPLE, BLACK, WHITE, GREY
            "Dark",             //TRUE, FALSE
            "Season",           //WINTER, SPRING, SUMMER, FALL
            "Environment1",     //URBAN, NATURE, SPACE, RUINS, ABSTRACT, ANCIENT, INDOORS, PORTRAIT
            "Environment2",     //URBAN, NATURE, SPACE, RUINS, ABSTRACT, ANCIENT, INDOORS, PORTRAIT
            "Funny",            //TRUE, FALSE
            "Inappropriate",    //TRUE, FALSE
            "Photograph",       //TRUE, FALSE
            "Food",             //TRUE, FALSE
            "Edgy",             //TRUE, FALSE
            "Gaming",           //TRUE, FALSE
            "Christmas",        //TRUE, FALSE
        };

        public static void Create()
        {
            var folder = ChooseFolder();
            if (folder == "")
                return;

            var files = GetFiles(folder);
            WriteDatabase(folder, files);
        }

        private static void WriteDatabase(string baseFolder, string[] files)
        {
            using (var f = File.OpenWrite(baseFolder + "database.csv"))
            {
                string line = "\"File\"," + string.Join("\",\"", catgories) + "\",\n";
                byte[] bytes = Encoding.ASCII.GetBytes(line);
                f.Write(bytes, 0, bytes.Length);

                foreach (var file in files)
                {
                    line = file + new string(',', catgories.Length) + "\n";
                    bytes = Encoding.ASCII.GetBytes(line);
                    f.Write(bytes, 0, bytes.Length);
                }
            }
        }

        private static string ChooseFolder()
        {
            using (var d = new FolderBrowserDialog
            {
                Description = "Choose the Wallpapers folder:",
                ShowNewFolderButton = false,
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "..",
            })
            {
                return d.ShowDialog() != DialogResult.OK ? "" : d.SelectedPath;
            }
        }

        private static string[] GetFiles(string directory)
        {
            var allFiles = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            for (var i = 0; i < allFiles.Length; i++)
            {
                var pieces = allFiles[i].Split('/', '\\');
                allFiles[i] = pieces[pieces.Length - 2] + '/' + pieces[pieces.Length - 1];
            }

            return allFiles;
        }
    }
}
