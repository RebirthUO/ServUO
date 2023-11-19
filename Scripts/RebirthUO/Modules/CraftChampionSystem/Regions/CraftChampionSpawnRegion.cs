// **********
// ServUO - CraftChampionSpawnRegion.cs
// **********

using System.Diagnostics.CodeAnalysis;
using Server.RebirthUO.Modules.CraftChampionSystem.Enums;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;
using Server.RebirthUO.Modules.CustomChampionSystem.Regions;

namespace Server.RebirthUO.Modules.CraftChampionSystem.Regions
{
	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
	public sealed class CraftChampionSpawnRegion : SuperChampionSpawnRegion<CraftChampionSpawnType>
	{
		public CraftChampionSpawnRegion(SuperChampionSpawn<CraftChampionSpawnType> spawn) : base(spawn)
		{
		}
	}
}
