// **********
// ServUO - TailorSpawnInfo.cs
// **********

using System;
using Server.Engines.CannedEvil;
using Server.Mobiles;

namespace Server.RebirthUO.Modules.CraftChampionSystem.SpawnInfo
{
	public class TailorSpawnInfo : ChampionSpawnInfo
	{
		private static string SpawnName => "Tailor";
		private static Type ChampionType => null; // TODO 
		private static string[] TitleNames => new[] { "Apprentice Tailor" }; // TODO
		private static Type[] Level1Spawn => new[] { typeof(GreaterMongbat), typeof(Imp) };
		private static Type[] Level2Spawn => new[] { typeof(Gargoyle), typeof(Harpy) };
		private static Type[] Level3Spawn => new[] { typeof(FireGargoyle), typeof(StoneGargoyle) };
		private static Type[] Level4Spawn => new[] { typeof(Daemon), typeof(Succubus) };
		private static Type[][] Spawns => new[] { Level1Spawn, Level2Spawn, Level3Spawn, Level4Spawn };

		public TailorSpawnInfo() : base(SpawnName, ChampionType, TitleNames, Spawns)
		{
		}
	}
}
