// **********
// ServUO - BagOfResources.cs
// **********

using System;
using System.Diagnostics.CodeAnalysis;
using Server.Items;
using Server.RebirthUO.CustomCraftingSystem.Helper;

namespace Server.RebirthUO.CustomCraftingSystem.Items
{
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	public class BagOfResources : Bag
	{
		[Constructable]
		public BagOfResources()
		{
			Name = "Bag of Resources";
			Weight = 0;
			LootType = LootType.Cursed;
			Hue = CraftResources.GetHue(
				CraftResourceHelper.GetRandomResource(CraftResourceHelper.GetMetalRunicResources()));
		}

		public BagOfResources(Serial serial) : base(serial)
		{
		}

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (!IsValid(dropped))
			{
				return false;
			}

			Weight = 0;
			return base.OnDragDrop(from, dropped);
		}

		public override bool OnDragDropInto(Mobile from, Item dropped, Point3D p)
		{
			if (!IsValid(dropped))
			{
				return false;
			}

			Weight = 0;
			return base.OnDragDropInto(from, dropped, p);
		}

		private bool IsValid(Item dropped)
		{
			return dropped is BaseOre || dropped is BaseIngot || dropped is BaseHides || dropped is BaseLeather ||
			       dropped is BaseWoodBoard || dropped is BaseLog;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (Weight != 0)
			{
				Timer.DelayCall(TimeSpan.Zero, InvalidateProperties);
			}

			Weight = 0;
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

			Weight = 0;
		}
	}
}
