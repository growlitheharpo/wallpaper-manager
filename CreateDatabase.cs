using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WallpaperManager
{
	public static class CreateDatabase
	{
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
			var categories = typeof(WallpaperData)
				.GetFields(BindingFlags.Instance | BindingFlags.Public)
				.Select(x => x.Name)
				.Select(x => char.ToUpper(x[0]) + x.Substring(1))
				.ToArray();

			using (var f = File.OpenWrite(baseFolder + "database.csv"))
			{
				var line = "\"" + string.Join("\",\"", categories) + "\",\n";
				var bytes = Encoding.ASCII.GetBytes(line);
				f.Write(bytes, 0, bytes.Length);

				foreach (var file in files)
				{
					line = file + new string(',', categories.Length) + "\n";
					bytes = Encoding.ASCII.GetBytes(line);
					f.Write(bytes, 0, bytes.Length);
				}

				f.Flush();
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
