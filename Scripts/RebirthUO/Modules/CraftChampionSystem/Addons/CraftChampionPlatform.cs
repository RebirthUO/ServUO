// **********
// ServUO - CraftChampionPlatform.cs
// **********

using Server.RebirthUO.Modules.CraftChampionSystem.Enums;
using Server.RebirthUO.Modules.CustomChampionSystem.Addons;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;

namespace Server.RebirthUO.Modules.CraftChampionSystem.Addons
{
	public sealed class CraftChampionPlatform : SuperChampionPlatform<CraftChampionSpawnType>
	{
		public CraftChampionPlatform(SuperChampionSpawn<CraftChampionSpawnType> spawn) : base(spawn)
		{
		}

		public CraftChampionPlatform(Serial serial) : base(serial)
		{
		}
	}
}
