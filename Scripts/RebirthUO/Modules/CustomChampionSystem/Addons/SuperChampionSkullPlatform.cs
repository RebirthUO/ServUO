using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Server.Items;

namespace Server.RebirthUO.Modules.CustomChampionSystem.Addons
{
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	[SuppressMessage("ReSharper", "PublicConstructorInAbstractClass")]
	public abstract class SuperChampionSkullPlatform<T> : BaseAddon where T : Enum
	{
		private new List<SuperChampionSkullBrazier<T>> Sockets { get; }

		public SuperChampionSkullPlatform()
		{
			GenerateDesign();
			Sockets = GenerateSockets();
		}

		public SuperChampionSkullPlatform(Serial serial) : base(serial)
		{
		}

		protected abstract void GenerateDesign();

		protected abstract List<SuperChampionSkullBrazier<T>> GenerateSockets();

		public void Validate()
		{
			if (Sockets.Any(socket => !Validate(socket)))
			{
				return;
			}

			if (SpawnSuperBoss() == null)
			{
				return;
			}

			foreach (var socket in Sockets)
			{
				Clear(socket);
			}
		}

		protected abstract Mobile SpawnSuperBoss();

		private void Clear(SuperChampionSkullBrazier<T> brazier)
		{
			if (brazier != null)
			{
				Effects.SendBoltEffect(brazier);

				brazier.Skull?.Delete();
			}
		}

		private bool Validate(SuperChampionSkullBrazier<T> brazier)
		{
			return brazier?.Skull != null && !brazier.Skull.Deleted;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(Sockets.Count);

			foreach (var item in Sockets)
			{
				writer.Write(item);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					var count = reader.ReadInt();

					for (var i = 0; i < count; i++)
					{
						Sockets.Add(reader.ReadItem() as SuperChampionSkullBrazier<T>);
					}

					break;
				}
			}
		}
	}
}
