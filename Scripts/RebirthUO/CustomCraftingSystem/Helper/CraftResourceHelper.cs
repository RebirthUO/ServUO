// **********
// ServUO - CraftResourceHelper.cs
// **********

using System.Collections.Generic;
using Server.Items;

namespace Server.RebirthUO.CustomCraftingSystem.Helper
{
	public static class CraftResourceHelper
	{
		public static List<CraftResource> GetMetalResources()
		{
			var list = new List<CraftResource>
			{
				CraftResource.Iron,
				CraftResource.DullCopper,
				CraftResource.ShadowIron,
				CraftResource.Copper,
				CraftResource.Bronze,
				CraftResource.Gold,
				CraftResource.Agapite,
				CraftResource.Verite,
				CraftResource.Valorite
			};

			return list;
		}

		public static List<CraftResource> GetMetalRunicResources()
		{
			var list = new List<CraftResource>
			{
				CraftResource.DullCopper,
				CraftResource.ShadowIron,
				CraftResource.Copper,
				CraftResource.Bronze,
				CraftResource.Gold,
				CraftResource.Agapite,
				CraftResource.Verite,
				CraftResource.Valorite
			};

			return list;
		}

		public static List<CraftResource> GetWoodResources()
		{
			var list = new List<CraftResource>
			{
				CraftResource.RegularWood,
				CraftResource.OakWood,
				CraftResource.AshWood,
				CraftResource.YewWood,
				CraftResource.Heartwood,
				CraftResource.Bloodwood,
				CraftResource.Frostwood
			};

			return list;
		}

		public static List<CraftResource> GetWoodRunicResources()
		{
			var list = new List<CraftResource>
			{
				CraftResource.OakWood, CraftResource.AshWood, CraftResource.YewWood, CraftResource.Heartwood
			};

			return list;
		}

		public static List<CraftResource> GetLeatherResources()
		{
			var list = new List<CraftResource>
			{
				CraftResource.RegularLeather,
				CraftResource.SpinedLeather,
				CraftResource.HornedLeather,
				CraftResource.BarbedLeather
			};

			return list;
		}

		public static List<CraftResource> GetLeatherRunicResources()
		{
			var list = new List<CraftResource>
			{
				CraftResource.SpinedLeather, CraftResource.HornedLeather, CraftResource.BarbedLeather
			};

			return list;
		}

		public static CraftResource GetRandomResource(List<CraftResource> list)
		{
			return list.Count.Equals(0) ? CraftResource.None : list[Utility.RandomMinMax(0, list.Count - 1)];
		}
	}
}
