// **********
// ServUO - CraftChampionAltar.cs
// **********

using Server.RebirthUO.Modules.CraftChampionSystem.Enums;
using Server.RebirthUO.Modules.CustomChampionSystem.Addons;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;

namespace Server.RebirthUO.Modules.CraftChampionSystem.Addons
{
	public sealed class CraftChampionAltar : SuperChampionAltar<CraftChampionSpawnType>
	{
		public CraftChampionAltar(SuperChampionSpawn<CraftChampionSpawnType> spawn) : base(spawn)
		{
		}

		public CraftChampionAltar(Serial serial) : base(serial)
		{
		}
	}
}
