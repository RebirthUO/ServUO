// **********
// ServUO - RunicToolContainer.cs
// **********

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Server.Items;
using Server.RebirthUO.CustomCraftingSystem.Helper;
using Server.RebirthUO.CustomCraftingSystem.Items.StorageContainer;
using Server.RebirthUO.CustomCraftingSystem.Items.Tools;

namespace Server.RebirthUO.CustomCraftingSystem.Items.RunicToolContainer
{
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public class RunicToolContainer : BaseIntStorageContainer<CraftResource>
	{
		private ContainerStorageGroup<CraftResource, int> BlackSmith { get; set; }

		private ContainerStorageGroup<CraftResource, int> Tinkering { get; set; }

		private ContainerStorageGroup<CraftResource, int> StoneCrafting { get; set; }

		private ContainerStorageGroup<CraftResource, int> Tailor { get; set; }

		private ContainerStorageGroup<CraftResource, int> Bowcraft { get; set; }

		private ContainerStorageGroup<CraftResource, int> Carpentry { get; set; }

		private const string _BlacksmithName = "Blacksmith";
		private const string _TinkeringName = "Tinkering";
		private const string _StoneCraftingName = "StoneCrafting";
		private const string _TailorName = "Tailor";
		private const string _BowcraftName = "Bowcraft & Fletching";
		private const string _CarpentryName = "Carpentry";

		[Constructable]
		public RunicToolContainer() : base(0x1EBB)
		{
			Movable = true;
			Weight = 1.0;
			Hue = 88;
			Name = "Runic House";

			BlackSmith = RegisterNewStorageGroup(_BlacksmithName);
			Tinkering = RegisterNewStorageGroup(_TinkeringName);
			StoneCrafting = RegisterNewStorageGroup(_StoneCraftingName);
			Tailor = RegisterNewStorageGroup(_TailorName);
			Bowcraft = RegisterNewStorageGroup(_BowcraftName);
			Carpentry = RegisterNewStorageGroup(_CarpentryName);

			RegisterGroupContent(BlackSmith, CraftResourceHelper.GetMetalRunicResources());
			RegisterGroupContent(Tinkering, CraftResourceHelper.GetMetalRunicResources());
			RegisterGroupContent(StoneCrafting, CraftResourceHelper.GetMetalRunicResources());
			RegisterGroupContent(Tailor, CraftResourceHelper.GetLeatherRunicResources());
			RegisterGroupContent(Bowcraft, CraftResourceHelper.GetWoodRunicResources());
			RegisterGroupContent(Carpentry, CraftResourceHelper.GetWoodRunicResources());
		}

		private ContainerStorageGroup<CraftResource, int> RegisterNewStorageGroup(string name)
		{
			return new ContainerStorageGroup<CraftResource, int>(name, RegisterNewGroup(name));
		}

		public RunicToolContainer(int itemId) : base(itemId)
		{
		}

		public RunicToolContainer(Serial serial) : base(serial)
		{
		}

		private void RegisterGroupContent(ContainerStorageGroup<CraftResource, int> group, List<CraftResource> keys)
		{
			foreach (var key in keys)
			{
				group.Values.Add(key, 0);
			}
		}

		protected override Dictionary<CraftResource, int> Generate()
		{
			return new Dictionary<CraftResource, int>();
		}

		public override string GetEntryName(CraftResource entryKey)
		{
			switch (entryKey)
			{
				case CraftResource.DullCopper:
					return "Dull Copper";
				case CraftResource.ShadowIron:
					return "Shadow Iron";
				case CraftResource.Copper:
					return "Copper";
				case CraftResource.Bronze:
					return "Bronze";
				case CraftResource.Gold:
					return "Gold";
				case CraftResource.Agapite:
					return "Agapite";
				case CraftResource.Verite:
					return "Verite";
				case CraftResource.Valorite:
					return "Valorite";
				case CraftResource.SpinedLeather:
					return "Spined Leather";
				case CraftResource.HornedLeather:
					return "Horned Leather";
				case CraftResource.BarbedLeather:
					return "Barbed Leather";
				case CraftResource.OakWood:
					return "OakWood";
				case CraftResource.AshWood:
					return "AshWood";
				case CraftResource.YewWood:
					return "YewWood";
				case CraftResource.Heartwood:
					return "Heartwood";
				default:
					throw new ArgumentOutOfRangeException(nameof(entryKey), entryKey, null);
			}
		}

		protected override bool ConsumeObject(Dictionary<CraftResource, int> list, Item item)
		{
			if (item is BaseRunicTool tool)
			{
				var amount = list[tool.Resource];

				if (tool.UsesRemaining + amount > MaxValue)
				{
					var difference = MaxValue - tool.UsesRemaining;
					list[tool.Resource] = MaxValue;
					tool.UsesRemaining -= difference;
				}
				else
				{
					list[tool.Resource] += tool.UsesRemaining;
					tool.Delete();
				}

				return true;
			}

			return false;
		}

		protected override void WriteEntry(CraftResource key, int value, GenericWriter writer)
		{
			writer.Write(0);

			writer.Write(key);

			writer.Write(value);
		}

		protected override void ReadEntry(Dictionary<CraftResource, int> parent, GenericReader reader)
		{
			var version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					var key = reader.ReadEnum<CraftResource>();
					var value = reader.ReadInt();
					parent.Add(key, value);

					break;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(version), version, null);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			BlackSmith = RegisterStorageGroup(_BlacksmithName, Data[_BlacksmithName]);
			Tinkering = RegisterStorageGroup(_TinkeringName, Data[_TinkeringName]);
			StoneCrafting = RegisterStorageGroup(_StoneCraftingName, Data[_StoneCraftingName]);
			Tailor = RegisterStorageGroup(_TailorName, Data[_TailorName]);
			Bowcraft = RegisterStorageGroup(_BowcraftName, Data[_BowcraftName]);
			Carpentry = RegisterStorageGroup(_CarpentryName, Data[_CarpentryName]);
		}

		private ContainerStorageGroup<CraftResource, int> RegisterStorageGroup(string name,
			Dictionary<CraftResource, int> value)
		{
			return new ContainerStorageGroup<CraftResource, int>(name, value);
		}

		public override void ShowGump(Mobile mobile)
		{
			mobile.CloseGump(typeof(BaseIntStorageContainerGump<CraftResource>));
			mobile.SendGump(new BaseIntStorageContainerGump<CraftResource>("Runic Tools Collector", mobile, this));
		}

		public override int MaxValue => 100000;

		public override void IncreaseStep()
		{
			var list = new Dictionary<int, int>
			{
				{ 1, 5 },
				{ 10, 25 },
				{ 25, 50 },
				{ 50, 100 },
				{ 100, 250 },
				{ 250, 500 },
				{ 500, 1000 },
				{ 1000, 2500 },
				{ 2500, 5000 },
				{ 5000, 10000 },
				{ 10000, 25000 },
				{ 25000, 50000 },
				{ 50000, 100000 }
			};
			TakeAmount = list[TakeAmount];
		}

		public override void DecreaseStep()
		{
			var list = new Dictionary<int, int>
			{
				{ 1, 100000 },
				{ 5, 1 },
				{ 10, 5 },
				{ 25, 10 },
				{ 50, 25 },
				{ 100, 50 },
				{ 250, 100 },
				{ 500, 250 },
				{ 1000, 500 },
				{ 2500, 1000 },
				{ 5000, 2500 },
				{ 10000, 5000 },
				{ 25000, 10000 },
				{ 100000, 50000 }
			};
			TakeAmount = list[TakeAmount];
		}

		public override void Collect(Mobile user)
		{
			if (IsAccessibleTo(user))
			{
				InternalCollect<RunicHammer>(user, BlackSmith);
				InternalCollect<RunicMalletAndChisel>(user, StoneCrafting);
				InternalCollect<RunicTinkerTool>(user, Tinkering);
				InternalCollect<RunicSewingKit>(user, Tailor);
				InternalCollect<RunicDovetailSaw>(user, Carpentry);
				InternalCollect<RunicFletcherTool>(user, Bowcraft);
			}
			else
			{
				user.SendLocalizedMessage(500447);
			}
		}

		private void InternalCollect<T>(Mobile mobile, ContainerStorageGroup<CraftResource, int> group)
			where T : BaseRunicTool
		{
			var list = mobile.Backpack.FindItemsByType<T>();

			foreach (var item in list)
			{
				if (group.Values.ContainsKey(item.Resource))
				{
					if (!ConsumeObject(group.Values, item))
					{
						// Error Message
						return;
					}
				}
			}
		}

		public override void Add(Mobile user)
		{
			if (IsAccessibleTo(user))
			{
			}
			else
			{
				user.SendLocalizedMessage(500447);
			}
		}

		public override void TakeOff(Mobile mobile, Tuple<string, CraftResource> keys)
		{
			if (IsAccessibleTo(mobile))
			{
				if (keys.Item1.Equals(BlackSmith.Name))
				{
					InternalTakeOff<RunicHammer>(mobile, BlackSmith, keys.Item2);
				}
				else if (keys.Item1.Equals(Tinkering.Name))
				{
					InternalTakeOff<RunicTinkerTool>(mobile, Tinkering, keys.Item2);
				}
				else if (keys.Item1.Equals(StoneCrafting.Name))
				{
					InternalTakeOff<RunicMalletAndChisel>(mobile, StoneCrafting, keys.Item2);
				}
				else if (keys.Item1.Equals(Tailor.Name))
				{
					InternalTakeOff<RunicSewingKit>(mobile, Tailor, keys.Item2);
				}
				else if (keys.Item1.Equals(Bowcraft.Name))
				{
					InternalTakeOff<RunicFletcherTool>(mobile, Bowcraft, keys.Item2);
				}
				else if (keys.Item1.Equals(Carpentry.Name))
				{
					InternalTakeOff<RunicDovetailSaw>(mobile, Carpentry, keys.Item2);
				}
			}
			else
			{
				mobile.SendLocalizedMessage(500447);
			}
		}

		private void InternalTakeOff<T>(Mobile mobile, ContainerStorageGroup<CraftResource, int> group,
			CraftResource resource)
			where T : BaseRunicTool
		{
			var remainingValue = group.Values[resource];
			var difference = remainingValue - TakeAmount;

			var takeOut = TakeAmount > remainingValue ? remainingValue : difference;

			if (takeOut > 0)
			{
				var obj = Activator.CreateInstance(typeof(T), resource, takeOut) as T;
				if (obj is BaseRunicTool tool)
				{
					if (mobile.Backpack.TryDropItem(mobile, tool, true))
					{
						group.Values[resource] -= takeOut;
					}
				}
			}
		}
	}
}
