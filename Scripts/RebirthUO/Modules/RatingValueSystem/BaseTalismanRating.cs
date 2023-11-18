// **********
// ServUO - BaseTalismanRating.cs
// **********

using Server.Items;
using Server.RebirthUO.CustomModuleMarker;

namespace Server.RebirthUO.Modules.RatingValueSystem
{
	[CustomModule(CustomModule.ValueRating)]
	public static class BaseTalismanRating
	{
		public static RatingValue GetRating(BaseTalisman talisman)
		{
			if (talisman.IsArtifact || talisman.ArtifactRarity > 0)
			{
				return RatingValue.Artifact;
			}

			return GenericRating.CanBeReevaluated(talisman.Rating)
				? GenericRating.GetRating(talisman, 500)
				: talisman.Rating;
		}
	}
}
