// **********
// ServUO - BaseClothingRating.cs
// **********

using Server.Items;
using Server.RebirthUO.CustomModuleMarker;

namespace Server.RebirthUO.Modules.RatingValueSystem
{
	[CustomModuleLink(CustomModule.ValueRating)]
	public static class BaseClothingRating
	{
		public static RatingValue GetRating(BaseClothing clothing)
		{
			if (clothing.IsArtifact || clothing.ArtifactRarity > 0)
			{
				return RatingValue.Artifact;
			}

			return GenericRating.CanBeReevaluated(clothing.Rating)
				? GenericRating.GetRating(clothing, 500)
				: clothing.Rating;
		}
	}
}
