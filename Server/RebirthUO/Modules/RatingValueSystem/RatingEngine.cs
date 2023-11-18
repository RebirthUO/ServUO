// **********
// ServUO - RatingEngine.cs
// **********


using System;
using Server.RebirthUO.CustomModuleMarker;
using Server.RebirthUO.Helper;

namespace Server.RebirthUO.Modules.RatingValueSystem
{
	[CustomModuleLink(CustomModule.ValueRating)]
	public static class RatingEngine
	{
		public static void AddRating(RatingValue rating, ObjectPropertyList list,
			Func<RatingValue> genericFunction = null)
		{
			if (!rating.Equals(RatingValue.None))
			{
				if (rating.Equals(RatingValue.Generic))
				{
					var result = genericFunction?.Invoke() ?? RatingValue.None;

					InternalAddRating(result, list);
				}
				else
				{
					InternalAddRating(rating, list);
				}
			}
		}

		private static void InternalAddRating(RatingValue rating, ObjectPropertyList list)
		{
			var entry = RatingValueEntry.GetEntry(rating);
			if (entry != null)
			{
				list.Add(ColorHelper.WrapNameWithHtmlColor(entry.Text, entry.Color));
			}
		}
	}
}
