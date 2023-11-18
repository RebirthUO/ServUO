// **********
// ServUO - BaseSpellBookRating.cs
// **********

using Server.Items;
using Server.RebirthUO.CustomModuleMarker;

namespace Server.RebirthUO.Modules.RatingValueSystem
{
	[CustomModule(CustomModule.ValueRating)]
	public static class BaseSpellBookRating
	{
		public static RatingValue GetRating(Spellbook spellBook)
		{
			if (spellBook.IsArtifact)
			{
				return RatingValue.Artifact;
			}

			return GenericRating.CanBeReevaluated(spellBook.Rating)
				? GenericRating.GetRating(spellBook, 500)
				: spellBook.Rating;
		}
	}
}
