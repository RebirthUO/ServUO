// **********
// ServUO - RandomRunicScroll.cs
// **********

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Server.Items;
using Server.RebirthUO.CustomCraftingSystem.Helper;
using Server.RebirthUO.CustomCraftingSystem.Items.Tools;
using Server.RebirthUO.Helper;
using Server.RebirthUO.Modules.RatingValueSystem;

namespace Server.RebirthUO.CustomCraftingSystem.Items
{
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	public class RandomRunicScroll : Item
	{
		[Constructable]
		public RandomRunicScroll() : base(0x2D51)
		{
			Stackable = true;
			Name = "Scroll of Crafting";
			Hue = 1165;
			Amount = 1;
			LootType = LootType.Cursed;
			Rating = RatingValue.Legendary;
		}

		public RandomRunicScroll(Serial serial) : base(serial)
		{
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			list.Add(ColorHelper.WrapNameWithHtmlColor("A magical infused scroll by an unknown artisan", Color.Purple));
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (Amount < 1)
			{
				from.SendMessage(
					"The magic infused in this scroll should stay as a secret. As result of this mythical magic, the scroll destroys itself.");
				Delete();
				return;
			}

			if (IsChildOf(from.Backpack))
			{
				Item item = null;

				switch (Utility.RandomMinMax(0, 5))
				{
					case 0: //RunicMallet
					{
						var resource =
							CraftResourceHelper.GetRandomResource(CraftResourceHelper.GetMetalRunicResources());
						var type = resource - CraftResource.Iron;
						var uses = 55 - (type * 5);
						item = new RunicMalletAndChisel(resource, uses);
						break;
					}
					case 1: //RunicFletcherTool
					{
						var resource =
							CraftResourceHelper.GetRandomResource(CraftResourceHelper.GetWoodRunicResources());
						var type = resource - CraftResource.RegularWood;
						var uses = 75 - (type * 15);

						item = new RunicFletcherTool(resource, uses);
						break;
					}
					case 2: //RunicHammer
					{
						var resource =
							CraftResourceHelper.GetRandomResource(CraftResourceHelper.GetMetalRunicResources());
						var type = resource - CraftResource.Iron;
						var uses = 55 - (type * 5);

						item = new RunicHammer(resource, uses);
						break;
					}
					case 3: //RunicSewingKit
					{
						var resource =
							CraftResourceHelper.GetRandomResource(CraftResourceHelper.GetLeatherRunicResources());
						var type = resource - CraftResource.RegularLeather;
						var uses = 60 - (type * 15);

						item = new RunicSewingKit(resource, uses);
						break;
					}
					case 4: //RunicTinkerTool
					{
						var resource =
							CraftResourceHelper.GetRandomResource(CraftResourceHelper.GetMetalRunicResources());
						var type = resource - CraftResource.Iron;
						var uses = 55 - (type * 5);

						item = new RunicTinkerTool(resource, uses);
						break;
					}
					case 5: //RunicDovetailSaw
					{
						var resource =
							CraftResourceHelper.GetRandomResource(CraftResourceHelper.GetWoodRunicResources());
						var type = resource - CraftResource.RegularWood;
						var uses = 75 - (type * 15);

						item = new RunicDovetailSaw(resource, uses);
						break;
					}
				}

				if (item != null)
				{
					from.Backpack.DropItem(item);

					from.SendMessage("Magic roams around you and a magical tool appear in your backpack.");
					Amount -= 1;
				}

				if (Amount < 1)
				{
					Delete();
				}
			}
			else
			{
				from.SendLocalizedMessage(503225);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}
	}
}
