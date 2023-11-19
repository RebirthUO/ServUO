using System;
using System.Diagnostics.CodeAnalysis;

namespace Server.RebirthUO.Modules.CustomChampionSystem.Items
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	[SuppressMessage("ReSharper", "PublicConstructorInAbstractClass")]
	public abstract class SuperChampionSkull<T> : Item where T : Enum
	{
		private T m_Type;

		[CommandProperty(AccessLevel.GameMaster)]
		public T Type
		{
			get => m_Type;
			set
			{
				m_Type = value;
				InvalidateProperties();
			}
		}

		public SuperChampionSkull(T type) : base(0x1AE1)
		{
			m_Type = type;

			LootType = LootType.Cursed;

			InitSkullType();
		}

		public SuperChampionSkull(Serial serial) : base(serial)
		{
		}

		protected abstract void InitSkullType();


		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(m_Type);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();

			m_Type = (T)reader.ReadEnum();
		}
	}
}
