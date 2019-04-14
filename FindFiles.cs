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
					var targetFile = $"{targetDirectory}\\{++counter}.{Path.GetExtension(sourcefile)}";
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
			
			targetFiles = targetFiles.Where(x => query.season == null || x.season == query.season);
			targetFiles = targetFiles.Where(x => query.pale == null || x.pale == query.pale);
			targetFiles = targetFiles.Where(x => query.dark == null || x.dark == query.dark);
			targetFiles = targetFiles.Where(x => query.funny == null || x.funny == query.funny);
			targetFiles = targetFiles.Where(x => query.inappropriate == null || x.inappropriate == query.inappropriate);
			targetFiles = targetFiles.Where(x => query.photograph == null || x.photograph == query.photograph);
			targetFiles = targetFiles.Where(x => query.food == null || x.food == query.food);
			targetFiles = targetFiles.Where(x => query.edgy == null || x.edgy == query.edgy);
			targetFiles = targetFiles.Where(x => query.gaming == null || x.gaming == query.gaming);
			targetFiles = targetFiles.Where(x => query.christmas == null || x.christmas == query.christmas);

			return targetFiles;
		}
	}
}
