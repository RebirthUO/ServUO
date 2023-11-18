﻿// **********
// ServUO - RatingValue.cs
// **********

using Server.RebirthUO.CustomModuleMarker;

namespace Server.RebirthUO.Modules.RatingValueSystem
{
	[CustomModuleLink(CustomModule.ValueRating)]
	public enum RatingValue
	{
		None,
		Generic,
		Common,
		Uncommon,
		Rare,
		Epic,
		Legendary,
		Event,
		Artifact,
		Donation,
		Unique,
		Season,
		Deprecated
	}
}
