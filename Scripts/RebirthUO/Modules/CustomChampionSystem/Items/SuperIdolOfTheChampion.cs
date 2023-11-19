// **********
// ServUO - SuperIdolOfTheChampion.cs
// **********

using System;

namespace Server.RebirthUO.Modules.CustomChampionSystem.Items
{
	public abstract class SuperIdolOfTheChampion<T> : Item where T : Enum
	{
		private SuperChampionSpawn<T> Spawn { get; set; }

		public override string DefaultName => "Idol of the Champion";

		protected SuperIdolOfTheChampion(SuperChampionSpawn<T> spawn, int itemId) : base(itemId)
		{
			Spawn = spawn;
			Movable = false;
		}

		protected SuperIdolOfTheChampion(Serial serial) : base(serial)
		{
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if (Spawn != null)
			{
				Spawn.Delete();
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(Spawn);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					Spawn = reader.ReadItem() as SuperChampionSpawn<T>;

					if (Spawn == null)
					{
						Delete();
					}

					break;
				}
			}
		}
	}
}
