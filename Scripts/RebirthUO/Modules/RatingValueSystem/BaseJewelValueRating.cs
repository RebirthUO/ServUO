// **********
// ServUO - BaseJewelValueRating.cs
// **********

using Server.Items;
using Server.RebirthUO.CustomModuleMarker;

namespace Server.RebirthUO.Modules.RatingValueSystem
{
	[CustomModule(CustomModule.ValueRating)]
	public static class BaseJewelValueRating
	{
		public static RatingValue GetRating(BaseJewel jewel)
		{
			if (jewel.IsArtifact || jewel.ArtifactRarity > 0)
			{
				return RatingValue.Artifact;
			}

			return GenericRating.CanBeReevaluated(jewel.Rating) ? GenericRating.GetRating(jewel, 500) : jewel.Rating;
		}
	}
}
