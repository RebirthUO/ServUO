// **********
// ServUO - CraftChampionSkull.cs
// **********

using System;
using Server.RebirthUO.Modules.CraftChampionSystem.Enums;
using Server.RebirthUO.Modules.CustomChampionSystem.Helper;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;

namespace Server.RebirthUO.Modules.CraftChampionSystem.Items
{
	public class CraftChampionSkull : SuperChampionSkull<CraftChampionSkullType>
	{
		[Constructable]
		public CraftChampionSkull(CraftChampionSkullType type) : base(type)
		{
			switch (type)
			{
				case CraftChampionSkullType.None:
					break;
				case CraftChampionSkullType.Smith:
					break;
				case CraftChampionSkullType.Tailor:
					Hue = 0x159;
					ItemID = 0x1034;
					break;
				case CraftChampionSkullType.Tinker:
					break;
				case CraftChampionSkullType.Bowcraft:
					break;
				case CraftChampionSkullType.Carpentry:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		[Constructable]
		public CraftChampionSkull() : base(
			SuperChampionSkullHelper.GetSkullType<CraftChampionSkullType>(type =>
				!type.Equals(CraftChampionSkullType.None)))
		{
		}

		public CraftChampionSkull(Serial serial) : base(serial)
		{
		}

		protected override void InitSkullType()
		{
			switch (Type)
			{
				case CraftChampionSkullType.Smith:
					Hue = 0x025;
					break;
				case CraftChampionSkullType.Tailor:
					Hue = 0x172;
					break;
				case CraftChampionSkullType.Tinker:
					Hue = 0x1EE;
					break;
				case CraftChampionSkullType.Bowcraft:
					Hue = 0x035;
					break;
				case CraftChampionSkullType.Carpentry:
					Hue = 0x159;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
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
