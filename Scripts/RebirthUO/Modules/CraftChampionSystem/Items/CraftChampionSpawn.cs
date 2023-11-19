// **********
// ServUO - CraftChampionSpawn.cs
// **********

using System;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.RebirthUO.Modules.CraftChampionSystem.Addons;
using Server.RebirthUO.Modules.CraftChampionSystem.Enums;
using Server.RebirthUO.Modules.CraftChampionSystem.Mobiles;
using Server.RebirthUO.Modules.CraftChampionSystem.Regions;
using Server.RebirthUO.Modules.CraftChampionSystem.SpawnInfo;
using Server.RebirthUO.Modules.CustomChampionSystem.Addons;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;
using Server.RebirthUO.Modules.CustomChampionSystem.Regions;

namespace Server.RebirthUO.Modules.CraftChampionSystem.Items
{
	public sealed class CraftChampionSpawn : SuperChampionSpawn<CraftChampionSpawnType>
	{
		protected override string TimerId => "CraftChampSpawnTimer";
		protected override string RestartTimerId => "CraftChampSpawnRestartTimer";

		protected override double GetMaxStrayDistance => 250;

		[Constructable]
		public CraftChampionSpawn()
		{
		}

		public CraftChampionSpawn(Serial serial) : base(serial)
		{
		}

		protected override SuperIdolOfTheChampion<CraftChampionSpawnType> CreateIdolOfChampion()
		{
			return new CraftIdolOfTheChampion(this);
		}

		protected override SuperChampionPlatform<CraftChampionSpawnType> CreateChampionPlatform()
		{
			return new CraftChampionPlatform(this);
		}

		protected override SuperChampionAltar<CraftChampionSpawnType> CreateChampionAltar()
		{
			return new CraftChampionAltar(this);
		}

		protected override SuperChampionSpawnRegion<CraftChampionSpawnType> CreateChampionSpawnRegion()
		{
			return new CraftChampionSpawnRegion(this);
		}

		protected override void HandleGlobalEvents()
		{
			//throw new System.NotImplementedException();
		}

		protected override int GetMaxKillsForLevel(int level)
		{
			return ChampionSystem.MaxKillsForLevel(level);
		}

		protected override int GetRankForLevel(int level)
		{
			return ChampionSystem.RankForLevel(level);
		}

		protected override void AwardChampionTitles()
		{
			//var info = ((PlayerMobile)killer).ChampionTitles;

			//info.Award(m_Type, mobSubLevel);
		}

		protected override PowerScroll CreateRandomPowerScroll()
		{
			return PowerScroll.CreateRandomNoCraft(5, 5);
		}

		protected override void GenerateNewStarRoomGate()
		{
			new StarRoomGate(true, Altar.Location, Altar.Map);
		}

		protected override Mobile GenerateChampion()
		{
			Type type;
			switch (Type)
			{
				case CraftChampionSpawnType.Tailor:
					type = new TailorSpawnInfo().Champion;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return Activator.CreateInstance(type) as Mobile;
		}

		protected override Type[][] GetSpawnTypes()
		{
			return new TailorSpawnInfo().SpawnTypes;
		}

		protected override CraftChampionSpawnType GetRandomType()
		{
			return CraftChampionSpawnType.Tailor;
		}

		protected override void HandleChampionEvents(Mobile mobile)
		{
			if (mobile is BaseCraftChampion champion)
			{
				champion.OnChampPopped(this);
			}
		}
	}
}
