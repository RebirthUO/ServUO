// **********
// ServUO - PropertySealingEngine.cs
// **********

using System;
using System.Drawing;
using Server.RebirthUO.Helper;
using Server.RebirthUO.Modules.CustomModuleMarker;

namespace Server.RebirthUO.Modules.PropertySealing
{
	[CustomModuleLink(CustomModule.PropertySealing)]
	public class PropertySealingEngine
	{
		public static void AddWeightProperty(Item item, ObjectPropertyList list)
		{
			if (item.DisplayWeight && item.Weight > 0)
			{
				var weight = item.PileWeight + item.TotalWeight;

				list.Add(weight == 1 ? 1072788 : 1072789, weight.ToString());
			}
		}

		public static void SealProperties(Item item, ObjectPropertyList list, Action customAction = null)
		{
			item.AddNameProperty(list);

			customAction?.Invoke();

			list.Add(ColorHelper.WrapNameWithHtmlColor("Unidentified", Color.Peru));

			AddWeightProperty(item, list);
			
			item.AddItemRatingProperty(list);
		}

		public static bool Identify(Mobile from, Item item)
		{
			if (item.HideProperties)
			{
				from.SendLocalizedMessage(1155658); // *You reveal something hidden about the object...*
				item.HideProperties = false;
				return true;
			}

			return false;
		}
	}
}
