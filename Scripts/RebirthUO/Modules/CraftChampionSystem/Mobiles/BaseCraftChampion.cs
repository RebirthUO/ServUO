// **********
// ServUO - BaseCraftChampion.cs
// **********

using System.Diagnostics.CodeAnalysis;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Mobiles;
using Server.RebirthUO.Modules.CraftChampionSystem.Config;
using Server.RebirthUO.Modules.CraftChampionSystem.Enums;
using Server.RebirthUO.Modules.CraftChampionSystem.Items;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;
using Server.RebirthUO.Modules.CustomChampionSystem.Mobiles;
using Server.RebirthUO.Modules.CustomChampionSystem.Timer;

namespace Server.RebirthUO.Modules.CraftChampionSystem.Mobiles
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public abstract class BaseCraftChampion : SuperBaseChampion<CraftChampionSkullType, CraftChampionSpawnType>
	{
		public override int PowerScrollAmount => CraftChampionConfig.GetAmountOfScrolls();

		protected BaseCraftChampion(AIType aiType) : base(aiType)
		{
		}

		protected BaseCraftChampion(AIType aiType, FightMode mode) : base(aiType, mode)
		{
		}

		protected BaseCraftChampion(Serial serial) : base(serial)
		{
		}

		protected override void DoRewardShower()
		{
			var min = ChampionSystem.GoldShowerMinAmount;
			var max = ChampionSystem.GoldShowerMaxAmount;
			var timer = new SuperRewardTimer(Location, Map, ChampionSystem.GoldShowerPiles, () => new Gold(min, max));

			timer.Start();
		}

		protected override SuperChampionSkull<CraftChampionSkullType> CreateChampionSkull()
		{
			return new CraftChampionSkull(SkullType);
		}

		protected override bool CanProvideSkull()
		{
			return SkullType != CraftChampionSkullType.None;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();
		}
	}
}
