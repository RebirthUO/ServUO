// **********
// ServUO - CraftChampionSkullPlattform.cs
// **********

using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.RebirthUO.Modules.CraftChampionSystem.Enums;
using Server.RebirthUO.Modules.CustomChampionSystem.Addons;

namespace Server.RebirthUO.Modules.CraftChampionSystem.Addons
{
	public sealed class CraftChampionSkullPlatform : SuperChampionSkullPlatform<CraftChampionSkullType>
	{
		[Constructable]
		public CraftChampionSkullPlatform()
		{
		}

		public CraftChampionSkullPlatform(Serial serial) : base(serial)
		{
		}

		protected override void GenerateDesign()
		{
			AddComponent(new AddonComponent(0x71A), -1, -1, -1);
			AddComponent(new AddonComponent(0x709), 0, -1, -1);
			AddComponent(new AddonComponent(0x709), 1, -1, -1);
			AddComponent(new AddonComponent(0x709), -1, 0, -1);
			AddComponent(new AddonComponent(0x709), 0, 0, -1);
			AddComponent(new AddonComponent(0x709), 1, 0, -1);
			AddComponent(new AddonComponent(0x709), -1, 1, -1);
			AddComponent(new AddonComponent(0x709), 0, 1, -1);
			AddComponent(new AddonComponent(0x71B), 1, 1, -1);
		}

		protected override List<SuperChampionSkullBrazier<CraftChampionSkullType>> GenerateSockets()
		{
			//AddComponent(new AddonComponent(0x50F), 0, -1, 4);
			//AddComponent(m_Power = new ChampionSkullBrazier(this, ChampionSkullType.Power), 0, -1, 5);

			return null;
		}

		protected override Mobile SpawnSuperBoss()
		{
			return Harrower.Spawn(new Point3D(X, Y, Z + 6), Map);
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
