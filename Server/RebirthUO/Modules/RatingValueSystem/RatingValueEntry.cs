// **********
// ServUO - RatingValueEntry.cs
// **********

using System;
using System.Drawing;
using Server.RebirthUO.CustomModuleMarker;

namespace Server.RebirthUO.Modules.RatingValueSystem
{
	[CustomModule(CustomModule.ValueRating)]
	public class RatingValueEntry
	{
		public static RatingValueEntry Common => new RatingValueEntry(RatingValue.Common, Color.White, "Common");

		public static RatingValueEntry Uncommon =>
			new RatingValueEntry(RatingValue.Uncommon, Color.LawnGreen, "Uncommon");

		public static RatingValueEntry Rare => new RatingValueEntry(RatingValue.Rare, Color.DodgerBlue, "Rare");
		public static RatingValueEntry Epic => new RatingValueEntry(RatingValue.Epic, Color.Purple, "Epic");

		public static RatingValueEntry Legendary =>
			new RatingValueEntry(RatingValue.Legendary, Color.OrangeRed, "Legendary");

		public static RatingValueEntry Event => new RatingValueEntry(RatingValue.Event, Color.Aqua, "Event");
		public static RatingValueEntry Artifact => new RatingValueEntry(RatingValue.Artifact, Color.Red, "Artifact");
		public static RatingValueEntry Donation => new RatingValueEntry(RatingValue.Donation, Color.Blue, "Donation");
		public static RatingValueEntry Unique => new RatingValueEntry(RatingValue.Unique, Color.SandyBrown, "Unique");
		public static RatingValueEntry Season => new RatingValueEntry(RatingValue.Season, Color.DarkRed, "Season");

		public static RatingValueEntry Deprecated =>
			new RatingValueEntry(RatingValue.Deprecated, Color.Yellow, "Deprecated");

		public Color Color { get; }
		public string Text { get; }
		public RatingValue RatingValue { get; }

		public RatingValueEntry(RatingValue ratingValue, Color color, string text)
		{
			RatingValue = ratingValue;
			Color = color;
			Text = text;
		}

		public static RatingValueEntry GetEntry(RatingValue value)
		{
			switch (value)
			{
				case RatingValue.None:
					return null;
				case RatingValue.Generic:
					return null;
				case RatingValue.Common:
					return Common;
				case RatingValue.Uncommon:
					return Uncommon;
				case RatingValue.Rare:
					return Rare;
				case RatingValue.Epic:
					return Epic;
				case RatingValue.Legendary:
					return Legendary;
				case RatingValue.Event:
					return Event;
				case RatingValue.Artifact:
					return Artifact;
				case RatingValue.Donation:
					return Donation;
				case RatingValue.Unique:
					return Unique;
				case RatingValue.Season:
					return Season;
				case RatingValue.Deprecated:
					return Deprecated;
				default:
					throw new ArgumentOutOfRangeException(nameof(value), value, null);
			}
		}
	}
}
