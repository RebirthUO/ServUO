// **********
// ServUO - GenericRating.cs
// **********

using System;
using Server.RebirthUO.Modules.CustomModuleMarker;
using Server.SkillHandlers;

namespace Server.RebirthUO.Modules.RatingValueSystem
{
	[CustomModuleLink(CustomModule.ValueRating)]
	public static class GenericRating
	{
		public static RatingValue GetRating(Item item, double limit)
		{
			var imbuingValue = Imbuing.GetTotalWeight(item, -1, false, true);

			var percentage = imbuingValue / limit * 100;

			return GetRating(percentage);
		}

		public static RatingValue GetRating(double value)
		{
			if (value >= 95)
			{
				return RatingValue.Legendary;
			}

			if (value >= 85)
			{
				return RatingValue.Epic;
			}

			if (value >= 75)
			{
				return RatingValue.Rare;
			}

			if (value >= 45)
			{
				return RatingValue.Uncommon;
			}

			return value >= 30 ? RatingValue.Common : RatingValue.None;
		}

		public static bool CanBeReevaluated(RatingValue value)
		{
			switch (value)
			{
				case RatingValue.None:
					return true;
				case RatingValue.Generic:
					return true;
				case RatingValue.Common:
					return true;
				case RatingValue.Uncommon:
					return true;
				case RatingValue.Rare:
					return true;
				case RatingValue.Epic:
					return true;
				case RatingValue.Legendary:
					return true;
				case RatingValue.Event:
					return false;
				case RatingValue.Artifact:
					return false;
				case RatingValue.Donation:
					return false;
				case RatingValue.Unique:
					return false;
				case RatingValue.Season:
					return false;
				case RatingValue.Deprecated:
					return false;
				default:
					throw new ArgumentOutOfRangeException(nameof(value), value, null);
			}
		}
	}
}
