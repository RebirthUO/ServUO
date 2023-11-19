// **********
// ServUO - CraftChampionSkullBrazier.cs
// **********

using System.Linq;
using Server.Mobiles;
using Server.RebirthUO.Modules.CraftChampionSystem.Enums;
using Server.RebirthUO.Modules.CustomChampionSystem.Addons;
using Server.RebirthUO.Modules.CustomChampionSystem.Targets;
using Server.Targeting;

namespace Server.RebirthUO.Modules.CraftChampionSystem.Addons
{
	public sealed class CraftChampionSkullBrazier : SuperChampionSkullBrazier<CraftChampionSkullType>
	{
		public CraftChampionSkullBrazier(SuperChampionSkullPlatform<CraftChampionSkullType> platform,
			CraftChampionSkullType type) : base(platform, type)
		{
		}

		public CraftChampionSkullBrazier(Serial serial) : base(serial)
		{
		}

		protected override string GetCannotSpawnSuperBossMessage()
		{
			return "The legendary Artisan has already been spawned.";
		}

		protected override bool CanSpawn()
		{
			return World.Mobiles.Values.Count(mobile => mobile is Harrower).Equals(0);
		}

		protected override Target CreateSacrificingTarget()
		{
			return new SuperSacrificeTarget<CraftChampionSkullType>(this);
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
