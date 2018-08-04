using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperManager
{
	public static class Ext
	{
		public static T ToEnum<T>(this string v) where T : struct
		{
			return Enum.TryParse(v, true, out T result) ? result : default(T);
		}
	}

	public struct WallpaperData
	{
		public enum Color
		{
			none,
			red,
			blue,
			green,
			yellow,
			orange,
			purple,
			black,
			white,
			grey
		};

		public enum Season
		{
			none,
			winter,
			spring,
			summer,
			fall
		};

		public enum Environment
		{
			none,
			urban,
			nature,
			space,
			ruins,
			@abstract,
			ancient,
			indoors,
			portrait
		};

		public string filename;

		public bool pale;
		public Color color1;
		public Color color2;

		public bool dark;
		public Season season;
		public Environment environment1;
		public Environment environment2;

		public bool funny;
		public bool inappropriate;
		public bool photograph;
		public bool food;
		public bool edgy;
		public bool gaming;
		public bool christmas;

		public Color[] color => new[] {color1, color2};
		public Environment[] environment => new[] {environment1, environment2};

		public static WallpaperData Parse(string line)
		{
			var data = line.Split(',');
			return new WallpaperData
			{
				filename = data[0],
				pale = bool.Parse(data[1]),
				color1 = data[2].ToLower().ToEnum<Color>(),
				color2 = data[3].ToLower().ToEnum<Color>(),
				dark = bool.Parse(data[4]),
				season = data[5].ToLower().ToEnum<Season>(),
				environment1 = data[6].ToLower().ToEnum<Environment>(),
				environment2 = data[7].ToLower().ToEnum<Environment>(),
				funny = bool.Parse(data[8]),
				inappropriate = bool.Parse(data[9]),
				photograph = bool.Parse(data[10]),
				food = bool.Parse(data[11]),
				edgy = bool.Parse(data[12]),
				gaming = bool.Parse(data[13]),
				christmas = bool.Parse(data[14]),
			};
		}
	}
}
