// **********
// ServUO - CraftIdolOfTheChampion.cs
// **********

using Server.RebirthUO.Modules.CraftChampionSystem.Enums;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;

namespace Server.RebirthUO.Modules.CraftChampionSystem.Items
{
	public class CraftIdolOfTheChampion : SuperIdolOfTheChampion<CraftChampionSpawnType>
	{
		public CraftIdolOfTheChampion(SuperChampionSpawn<CraftChampionSpawnType> spawn) : base(spawn, 0x1F18)
		{
		}

		public CraftIdolOfTheChampion(Serial serial) : base(serial)
		{
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
