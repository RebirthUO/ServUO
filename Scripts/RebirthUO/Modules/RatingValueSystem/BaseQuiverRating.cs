// **********
// ServUO - BaseQuiverRating.cs
// **********

using Server.Items;
using Server.RebirthUO.Modules.CustomModuleMarker;

namespace Server.RebirthUO.Modules.RatingValueSystem
{
	[CustomModuleLink(CustomModule.ValueRating)]
	public static class BaseQuiverRating
	{
		public static RatingValue GetRating(BaseQuiver quiver)
		{
			if (quiver.IsArtifact || quiver.ArtifactRarity > 0)
			{
				return RatingValue.Artifact;
			}

			return GenericRating.CanBeReevaluated(quiver.Rating) ? GenericRating.GetRating(quiver, 500) : quiver.Rating;
		}
	}
}
