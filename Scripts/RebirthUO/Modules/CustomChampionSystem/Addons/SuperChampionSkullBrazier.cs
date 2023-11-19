using System;
using System.Diagnostics.CodeAnalysis;
using Server.Items;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;
using Server.Targeting;

namespace Server.RebirthUO.Modules.CustomChampionSystem.Addons
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	[SuppressMessage("ReSharper", "PublicConstructorInAbstractClass")]
	public abstract class SuperChampionSkullBrazier<T> : AddonComponent where T : Enum
	{
		private SuperChampionSkull<T> m_Skull;
		private T m_Type;

		[CommandProperty(AccessLevel.GameMaster)]
		public SuperChampionSkullPlatform<T> Platform { get; set; }

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

		[CommandProperty(AccessLevel.GameMaster)]
		public SuperChampionSkull<T> Skull
		{
			get => m_Skull;
			set
			{
				m_Skull = value;

				Platform?.Validate();
			}
		}

		protected SuperChampionSkullBrazier(SuperChampionSkullPlatform<T> platform, T type) : base(0x19BB)
		{
			Hue = 0x455;
			Light = LightType.Circle300;

			Platform = platform;
			m_Type = type;
		}

		public SuperChampionSkullBrazier(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			Platform?.Validate();

			BeginSacrifice(from);
		}

		public void BeginSacrifice(Mobile from)
		{
			if (Deleted)
			{
				return;
			}

			if (m_Skull != null && m_Skull.Deleted)
			{
				Skull = null;
			}

			if (from.Map != Map || !from.InRange(GetWorldLocation(), 3))
			{
				from.SendLocalizedMessage(500446); // That is too far away.
			}
			else if (!CanSpawn())
			{
				from.SendMessage(GetCannotSpawnSuperBossMessage());
			}
			else if (m_Skull == null)
			{
				from.SendLocalizedMessage(1049485); // What would you like to sacrifice?
				from.Target = CreateSacrificingTarget();
			}
			else
			{
				SendLocalizedMessageTo(from, 1049487, ""); // I already have my champions awakening skull!
			}
		}

		/// <summary>
		/// Message will be shown, when player is unable to summon a super boss.
		/// </summary>
		/// <returns></returns>
		protected abstract string GetCannotSpawnSuperBossMessage();

		/// <summary>
		/// Checks for Permission to spawn a super boss.
		/// </summary>
		/// <returns></returns>
		protected abstract bool CanSpawn();

		/// <summary>
		/// Generates a Sacrifice Target Cursor for offering a skull.
		/// </summary>
		/// <returns></returns>
		protected abstract Target CreateSacrificingTarget();

		public void EndSacrifice(Mobile from, SuperChampionSkull<T> skull)
		{
			if (Deleted)
			{
				return;
			}

			if (m_Skull != null && m_Skull.Deleted)
			{
				Skull = null;
			}

			if (from.Map != Map || !from.InRange(GetWorldLocation(), 3))
			{
				from.SendLocalizedMessage(500446); // That is too far away.
			}
			else if (!CanSpawn())
			{
				from.SendMessage(GetCannotSpawnSuperBossMessage());
			}
			else if (skull == null)
			{
				SendLocalizedMessageTo(from, 1049488, ""); // That is not my champions awakening skull!
			}
			else if (m_Skull != null)
			{
				SendLocalizedMessageTo(from, 1049487, ""); // I already have my champions awakening skull!
			}
			else if (!skull.IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1049486); // You can only sacrifice items that are in your backpack!
			}
			else
			{
				if (skull.Type.Equals(Type))
				{
					skull.Movable = false;
					skull.MoveToWorld(GetWorldTop(), Map);

					Skull = skull;
				}
				else
				{
					SendLocalizedMessageTo(from, 1049488, ""); // That is not my champions awakening skull!
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version

			writer.Write(m_Type);
			writer.Write(Platform);
			writer.Write(m_Skull);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					m_Type = (T)reader.ReadEnum();
					Platform = reader.ReadItem() as SuperChampionSkullPlatform<T>;
					m_Skull = reader.ReadItem() as SuperChampionSkull<T>;

					if (Platform == null)
					{
						Delete();
					}

					break;
				}
			}
		}
	}
}
