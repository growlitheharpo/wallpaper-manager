namespace WallpaperManager
{
	public interface IWallpaperQuery
	{
		WallpaperData.Color[] colors { get; }
		WallpaperData.Environment[] environments { get; }
		WallpaperData.Season? season { get; }
		
		bool? pale { get; }
		bool? dark { get; }
		bool? funny { get; }
		bool? inappropriate { get; }
		bool? photograph { get; }
		bool? food { get; }
		bool? edgy { get; }
		bool? gaming { get; }
		bool? christmas { get; }
	}
}
