// **********
// ServUO - BaseArmorValueRating.cs
// **********

using Server.Items;
using Server.RebirthUO.CustomModuleMarker;

namespace Server.RebirthUO.Modules.RatingValueSystem
{
	[CustomModuleLink(CustomModule.ValueRating)]
	public static class BaseArmorValueRating
	{
		public static RatingValue GetRating(BaseArmor armor)
		{
			if (armor.IsArtifact || armor.ArtifactRarity > 0)
			{
				return RatingValue.Artifact;
			}

			var limit = armor is BaseShield ? 450 : 500;

			return GenericRating.CanBeReevaluated(armor.Rating) ? GenericRating.GetRating(armor, limit) : armor.Rating;
		}
	}
}
