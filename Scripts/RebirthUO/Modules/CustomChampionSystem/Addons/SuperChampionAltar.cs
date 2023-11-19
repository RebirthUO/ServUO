using System;
using System.Diagnostics.CodeAnalysis;
using Server.Items;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;

namespace Server.RebirthUO.Modules.CustomChampionSystem.Addons
{
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	[SuppressMessage("ReSharper", "PublicConstructorInAbstractClass")]
	public abstract class SuperChampionAltar<T> : PentagramAddon where T : Enum
	{
		private SuperChampionSpawn<T> Spawn { get; set; }

		public SuperChampionAltar(SuperChampionSpawn<T> spawn)
		{
			Spawn = spawn;
			Hue = 0x455;
		}

		public SuperChampionAltar(Serial serial) : base(serial)
		{
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			Spawn?.Delete();
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
					else if (!Spawn.Active)
					{
						Hue = 0x455;
					}
					else
					{
						Hue = 0;
					}

					break;
				}
			}
		}
	}
}
