// **********
// ServUO - BaseWeaponRating.cs
// **********

using Server.Items;
using Server.RebirthUO.CustomModuleMarker;

namespace Server.RebirthUO.Modules.RatingValueSystem
{
	[CustomModule(CustomModule.ValueRating)]
	public static class BaseWeaponRating
	{
		public static RatingValue GetRating(BaseWeapon weapon)
		{
			if (weapon.IsArtifact || weapon.ArtifactRarity > 0)
			{
				return RatingValue.Artifact;
			}

			return GenericRating.CanBeReevaluated(weapon.Rating) ? GenericRating.GetRating(weapon, 500) : weapon.Rating;
		}
	}
}
