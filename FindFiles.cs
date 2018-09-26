using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WallpaperManager
{
	public class FindFiles
	{
		private const string DEFAULT_DATABASE = "M:\\Google Drive\\Pictures\\Wallpapers\\Wallpapersdatabasecut.csv";

		public static void DoFindFiles(bool useDefaultDatabase)
		{
			var dbFile = useDefaultDatabase ? DEFAULT_DATABASE : GetDbFile();
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
				.PerformQuery()
				.ToArray();

			if (targetFiles.Length == 0)
				return;

			Directory.CreateDirectory(targetDirectory);

			var counter = 0;
			foreach (var f in targetFiles)
			{
				try
				{
					var sourcefile = basePath + f.filename;
					var targetFile = targetDirectory + $"\\{++counter}." + Path.GetFileName(sourcefile).Split('.')[1];
					File.Copy(sourcefile, targetFile);
				}
				catch (Exception e)
				{
					Console.WriteLine("Caught exception while copying file: " + e);
				}
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

	internal static class QueryExtensions
	{
		public static IEnumerable<WallpaperData> PerformQuery(this IEnumerable<WallpaperData> targetFiles)
		{
			var query = QueryProvider.GetQuery();

			if (query.colors.Length > 0)
				targetFiles = query.colors.Aggregate(targetFiles, (current, color) => current.Where(x => x.HasProperty(color)));

			if (query.environments.Length > 0)
				targetFiles = query.environments.Aggregate(targetFiles, (current, environment) => current.Where(x => x.HasProperty(environment)));
			
			if (query.season != null)
				targetFiles = targetFiles.Where(x => x.season == query.season);

			if (query.pale != null)
				targetFiles = targetFiles.Where(x => x.pale == query.pale);

			if (query.dark != null)
				targetFiles = targetFiles.Where(x => x.dark == query.dark);

			if (query.funny != null)
				targetFiles = targetFiles.Where(x => x.funny == query.funny);

			if (query.inappropriate != null)
				targetFiles = targetFiles.Where(x => x.inappropriate == query.inappropriate);

			if (query.photograph != null)
				targetFiles = targetFiles.Where(x => x.photograph == query.photograph);

			if (query.food != null)
				targetFiles = targetFiles.Where(x => x.food == query.food);

			if (query.edgy != null)
				targetFiles = targetFiles.Where(x => x.edgy == query.edgy);

			if (query.gaming != null)
				targetFiles = targetFiles.Where(x => x.gaming == query.gaming);

			if (query.christmas != null)
				targetFiles = targetFiles.Where(x => x.christmas == query.christmas);

			return targetFiles;
		}
	}
}
