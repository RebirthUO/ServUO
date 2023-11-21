// **********
// ServUO - BaseStorageContainer.cs
// **********

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Server.RebirthUO.CustomCraftingSystem.Items.StorageContainer
{
	[SuppressMessage("ReSharper", "PublicConstructorInAbstractClass")]
	public abstract class BaseStorageContainer<T1, T2> : Item
	{
		public Dictionary<string, Dictionary<T1, T2>> Data { get; set; }
		public int TakeAmount { get; set; }

		public BaseStorageContainer(int itemId) : base(itemId)
		{
			Data = new Dictionary<string, Dictionary<T1, T2>>();
			TakeAmount = 1;
		}

		public BaseStorageContainer(Serial serial) : base(serial)
		{
		}

		protected abstract Dictionary<T1, T2> Generate();

		protected Dictionary<T1, T2> RegisterNewGroup(string name)
		{
			if (!Data.ContainsKey(name))
			{
				Data.Add(name, Generate());
			}

			return Data[name];
		}

		public virtual int GetMaxAmountOfEntries()
		{
			return Data.Keys.Select(key => Data[key].Count).Prepend(0).Max();
		}

		public abstract string GetEntryName(T1 entryKey);
		protected abstract bool ConsumeObject(Dictionary<T1, T2> list, Item item);

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(TakeAmount);
			writer.Write(Data.Count);

			foreach (var key in Data.Keys)
			{
				writer.Write(key);
				WriteList(Data[key], writer);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			Data = new Dictionary<string, Dictionary<T1, T2>>();

			var version = reader.ReadInt();
			switch (version)
			{
				case 0:
				{
					TakeAmount = reader.ReadInt();
					var count = reader.ReadInt();

					for (var i = 0; i < count; i++)
					{
						var key = reader.ReadString();
						
						Data.Add(key, ReadList(reader));
					}

					break;
				}
			}
		}

		private void WriteList(Dictionary<T1, T2> values, GenericWriter writer)
		{
			writer.Write(0);

			writer.Write(values.Count);

			foreach (var subKey in values.Keys)
			{
				WriteEntry(subKey, values[subKey], writer);
			}
		}

		protected abstract void WriteEntry(T1 key, T2 value, GenericWriter writer);

		private Dictionary<T1, T2> ReadList(GenericReader reader)
		{
			var version = reader.ReadInt();

			var list = new Dictionary<T1, T2>();
			switch (version)
			{
				case 0:
				{
					var count = reader.ReadInt();

					for (var i = 0; i < count; i++)
					{
						ReadEntry(list, reader);
					}

					break;
				}
			}

			return list;
		}

		protected abstract void ReadEntry(Dictionary<T1, T2> parent, GenericReader reader);
		public abstract void ShowGump(Mobile mobile);

		public override void OnDoubleClick(Mobile from)
		{
			if (IsAccessibleTo(from))
			{
				ShowGump(from);
			}
			else
			{
				from.SendLocalizedMessage(500447);
			}
		}
		
		public abstract T2 MaxValue { get; }

		public abstract void IncreaseStep();
		public abstract void DecreaseStep();
		public abstract void Collect(Mobile user);
		public abstract void Add(Mobile user);
		public abstract void TakeOff(Mobile mobile, Tuple<string, T1> keys);
	}
}
