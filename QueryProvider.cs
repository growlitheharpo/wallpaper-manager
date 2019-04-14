using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WallpaperManager
{
	public static class QueryProvider
	{
		private class BaseQuery : IWallpaperQuery
		{
			public virtual bool? pale => null;

			public virtual WallpaperData.Color[] colors => new WallpaperData.Color[0];

			public virtual bool? dark => null;

			public virtual WallpaperData.Season? season => null;

			public virtual WallpaperData.Environment[] environments => new WallpaperData.Environment[0];

			public virtual bool? funny => null;

			public virtual bool? inappropriate => null;

			public virtual bool? photograph => null;

			public virtual bool? food => null;

			public virtual bool? edgy => null;

			public virtual bool? gaming => null;

			public virtual bool? christmas => null;
		}

		private class FallPhotography : BaseQuery
		{
			public override bool? photograph => true;

			public override WallpaperData.Season? season => WallpaperData.Season.fall;
		}

		private class CitiesInSpace : BaseQuery
		{
			public override WallpaperData.Environment[] environments => new[] { WallpaperData.Environment.urban, WallpaperData.Environment.space };
		}

		public class BooleanEntry : INotifyPropertyChanged
		{
			private bool? isChecked;
			public bool? IsChecked
			{
				get => isChecked;
				set {
					if (value == isChecked)
						return;

					isChecked = value;
					OnPropertyChanged();
				}
			}

			public string Label { get; set; }

			// Don't allow null by default
			public BooleanEntry(bool? initialValue = false)
			{
				isChecked = initialValue;
			}

			public event PropertyChangedEventHandler PropertyChanged;

			protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public class CheckboxMultiCollectionViewModel : INotifyPropertyChanged
		{
			private List<BooleanEntry> entries;
			public List<BooleanEntry> Entries
			{
				get => entries;
				set {
					if (Equals(value, entries))
						return;

					entries = value;
					OnPropertyChanged();
				}
			}

			private bool? allSelected;
			public bool? AllSelected
			{
				get => allSelected;
				set {
					if (value == allSelected)
						return;

					allSelected = value;
					AllSelectedChanged();
					OnPropertyChanged();
				}
			}

			private bool allSelectedChanging;
			public event PropertyChangedEventHandler PropertyChanged;

			public CheckboxMultiCollectionViewModel(bool defaultAllSelected, IEnumerable<string> labels)
			{
				entries = labels.Select(label => new BooleanEntry { Label = label }).ToList();

				foreach (var e in entries)
					e.PropertyChanged += EntryOnPropertyChanged;

				AllSelected = defaultAllSelected;
			}

			private void EntryOnPropertyChanged(object sender, PropertyChangedEventArgs args)
			{
				if (args.PropertyName == nameof(BooleanEntry.IsChecked))
					RecheckAllSelected();
			}

			private void RecheckAllSelected()
			{
				// Has this change been caused by some other change?
				// return so we don't mess things up
				if (allSelectedChanging)
					return;

				try
				{
					allSelectedChanging = true;

					if (entries.All(e => e.IsChecked == true))
						AllSelected = true;
					else if (entries.All(e => e.IsChecked == false))
						AllSelected = false;
					else
						AllSelected = null;
				}
				finally
				{
					allSelectedChanging = false;
				}
			}

			private void AllSelectedChanged()
			{
				// Has this change been caused by some other change?
				// return so we don't mess things up
				if (allSelectedChanging)
					return;

				try
				{
					allSelectedChanging = true;
					if (AllSelected == true)
					{
						foreach (var e in entries)
							e.IsChecked = true;
					}
					else if (AllSelected == false)
					{
						foreach (var e in entries)
							e.IsChecked = false;
					}
				}
				finally
				{
					allSelectedChanging = false;
				}
			}

			protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public class CheckboxExclusiveCollectionViewModel : INotifyPropertyChanged
		{
			private List<BooleanEntry> entries;
			public List<BooleanEntry> Entries
			{
				get => entries;
				set {
					if (Equals(value, entries))
						return;

					entries = value;
					OnPropertyChanged();
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

			private bool isProcessingChange;

			public CheckboxExclusiveCollectionViewModel(IEnumerable<string> labels)
			{
				entries = labels.Select(label => new BooleanEntry { Label = label }).ToList();

				foreach (var e in entries)
					e.PropertyChanged += EntryOnPropertyChanged;

				if (entries.Count > 0)
					entries[0].IsChecked = true;
			}

			private void EntryOnPropertyChanged(object sender, PropertyChangedEventArgs args)
			{
				if (args.PropertyName != nameof(BooleanEntry.IsChecked))
					return;

				if (isProcessingChange)
					return;

				try
				{
					isProcessingChange = true;

					foreach (var e in Entries)
					{
						if (Equals(e, sender))
							continue;

						e.IsChecked = false;
					}
				}
				finally
				{
					isProcessingChange = false;
				}
			}

			protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public class WallpaperQuerySharedContext
		{
			public CheckboxMultiCollectionViewModel ColorList { get; set; }
			public CheckboxMultiCollectionViewModel EnvironmentList { get; set; }
			public CheckboxExclusiveCollectionViewModel SeasonList { get; set; }

			public BooleanEntry Pale { get; }
			public BooleanEntry Dark { get; }
			public BooleanEntry Funny { get; }
			public BooleanEntry Inappropriate { get; }
			public BooleanEntry Photograph { get; }
			public BooleanEntry Food { get; }
			public BooleanEntry Edgy { get; }
			public BooleanEntry Gaming { get; }
			public BooleanEntry Christmas { get; }

			public List<BooleanEntry> LeftColumn => new List<BooleanEntry>
			{
				Pale,
				Dark,
				Funny,
				Inappropriate,
				Photograph
			};

			public List<BooleanEntry> RightColumn => new List<BooleanEntry>
			{
				Food,
				Edgy,
				Gaming,
				Christmas,
			};

			public List<BooleanEntry> AllBools => new List<BooleanEntry>
			{
				Pale,
				Dark,
				Funny,
				Inappropriate,
				Photograph,
				Food,
				Edgy,
				Gaming,
				Christmas,
			};

			public WallpaperQuerySharedContext()
			{
				ColorList = new CheckboxMultiCollectionViewModel(
					true,
					Enum.GetNames(typeof(WallpaperData.Color))
						.Where(x => x != "none")
						.Select(x => x.Substring(0, 1).ToUpper() + x.Substring(1, x.Length - 1))
					);

				EnvironmentList = new CheckboxMultiCollectionViewModel(
					true,
					Enum.GetNames(typeof(WallpaperData.Environment))
						.Where(x => x != "none")
						.Select(x => x.Substring(0, 1).ToUpper() + x.Substring(1, x.Length - 1))
				);

				SeasonList = new CheckboxExclusiveCollectionViewModel(
					Enum.GetNames(typeof(WallpaperData.Season))
						.Select(x => x.Substring(0, 1).ToUpper() + x.Substring(1, x.Length - 1))
				);

				Pale = new BooleanEntry(null) { Label = "Pale" };
				Dark = new BooleanEntry(null) { Label = "Dark" };
				Funny = new BooleanEntry(null) { Label = "Funny" };
				Inappropriate = new BooleanEntry(null) { Label = "Inappropriate" };
				Photograph = new BooleanEntry(null) { Label = "Photograph" };
				Food = new BooleanEntry(null) { Label = "Food" };
				Edgy = new BooleanEntry(null) { Label = "Edgy" };
				Gaming = new BooleanEntry(null) { Label = "Gaming" };
				Christmas = new BooleanEntry(null) { Label = "Christmas" };
			}
		}

		public class WallpaperFormQuery : IWallpaperQuery
		{
			public WallpaperQuerySharedContext SharedData { get; set; }

			public WallpaperData.Color[] colors
			{
				get {
					return SharedData.ColorList.AllSelected == true || SharedData.ColorList.Entries.All(x => x.IsChecked != true) ?
						new WallpaperData.Color[0] :
						SharedData.ColorList.Entries
							.Where(x => x.IsChecked == true)
							.Select(x => x.Label)
							.ToEnum<WallpaperData.Color>().ToArray();
				}
			}

			public WallpaperData.Environment[] environments
			{
				get {
					return SharedData.EnvironmentList.AllSelected == true || SharedData.EnvironmentList.Entries.All(x => x.IsChecked != true) ?
					  new WallpaperData.Environment[0] :
					  SharedData.EnvironmentList.Entries
							.Where(x => x.IsChecked == true)
							.Select(x => x.Label)
							.ToEnum<WallpaperData.Environment>().ToArray();
				}
			}

			public WallpaperData.Season? season
			{
				get {
					WallpaperData.Season? result =
					SharedData.SeasonList.Entries.Any(x => x.IsChecked == true) ?
					  SharedData.SeasonList.Entries.First(x => x.IsChecked == true).Label.ToEnumNullable<WallpaperData.Season>()
					  : null;

					return result == WallpaperData.Season.none ? null : result;
				}
			}

			public bool? pale => SharedData.Pale.IsChecked;
			public bool? dark => SharedData.Dark.IsChecked;
			public bool? funny => SharedData.Funny.IsChecked;
			public bool? inappropriate => SharedData.Inappropriate.IsChecked;
			public bool? photograph => SharedData.Photograph.IsChecked;
			public bool? food => SharedData.Food.IsChecked;
			public bool? edgy => SharedData.Edgy.IsChecked;
			public bool? gaming => SharedData.Gaming.IsChecked;
			public bool? christmas => SharedData.Christmas.IsChecked;
		}

		public static IWallpaperQuery GetQuery()
		{
			var form = new WallpaperQueryForm();
			var qWindow = new Window
			{
				Title = "Query Provider",
				Height = 700,
				Width = 600,
				Content = form
			};

			WallpaperFormQuery query = new WallpaperFormQuery
			{
				SharedData = new WallpaperQuerySharedContext()
			};

			qWindow.DataContext = form.DataContext = query.SharedData;
			form.CloseButton.Click += (sender, args) => qWindow.Close();

			qWindow.ShowDialog();

			return query;
		}
	}
}
