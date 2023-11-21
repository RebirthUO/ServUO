// **********
// ServUO - RunicTinkerTools.cs
// **********

using System.Diagnostics.CodeAnalysis;
using Server.Engines.Craft;
using Server.Items;

namespace Server.RebirthUO.CustomCraftingSystem.Items.Tools
{
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	public class RunicTinkerTool : BaseRunicTool
	{
		public override CraftSystem CraftSystem => DefTinkering.CraftSystem;

		[Constructable]
		public RunicTinkerTool(CraftResource resource) : base(resource, 0x1EB8)
		{
			Hue = CraftResources.GetHue(resource);
		}

		[Constructable]
		public RunicTinkerTool(CraftResource resource, int uses) : base(resource, uses, 0x1EB8)
		{
			Hue = CraftResources.GetHue(resource);
		}

		public RunicTinkerTool(Serial serial) : base(serial)
		{
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
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			var v = " ";

			if (!CraftResources.IsStandard(Resource))
			{
				v = CraftResources.GetName(Resource);
			}

			list.Add("{0} Runic Tinker Tools", v); // ~1_METAL_TYPE~ Runic Tinker Tools
		}
	}
}
