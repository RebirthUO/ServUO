// **********
// ServUO - BulkOrderRewardHelper.cs
// **********

using System;
using Server.Engines.BulkOrders;
using Server.Items;
using Server.RebirthUO.CustomCraftingSystem.Items;
using Server.RebirthUO.CustomCraftingSystem.Items.Tools;

namespace Server.RebirthUO.CustomCraftingSystem.Helper
{
	public static class BulkOrderRewardHelper
	{
		public static BODCollectionItem GetBagOfResources()
		{
			const string text =
				"A magic infused bag crafted by a legendary artisan. Allows its user to hold an infinite amount " +
				"of resources without gaining weight. On the other side, the artisan were insane, so its cursed.";

			return new BODCollectionItem(0xE76, text, 0, 10000, i => new BagOfResources());
		}

		public static BODCollectionItem GetRunicTinkerTool(CraftResource resource, int price)
		{
			const string text =
				"Receive a {0} runic tinker tool.  This tinker tool may be used to craft jewelry with magical " +
				"properties; the number of magical properties and their minimum intensity depends on the type of" +
				" runic tinker tool used. It may also be used to reforge non-magical items into magical items " +
				"provided that the user has the required Imbuing skill to reforge the item. The {1} runic tinker " +
				"tool has {2} uses when new.";

			var type = resource - CraftResource.Iron;
			var uses = 55 - (type * 5);
			var textId = resource.ToString();
			var toolTip = String.Format(text, textId, textId, uses);
			var hue = CraftResources.GetHue(resource);

			return new BODCollectionItem(0x1EB8, toolTip, hue, price, i => new RunicTinkerTool(resource, uses));
		}
	}
}
