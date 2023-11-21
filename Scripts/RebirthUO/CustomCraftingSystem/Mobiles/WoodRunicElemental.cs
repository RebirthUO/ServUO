// **********
// ServUO - WoodElemental.cs
// **********

using System;
using System.Diagnostics.CodeAnalysis;
using Server.Items;
using Server.Mobiles;
using Server.RebirthUO.CustomCraftingSystem.Config;

namespace Server.RebirthUO.CustomCraftingSystem.Mobiles
{
	[CorpseName("a wooden elemental corpse")]
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	[SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	[SuppressMessage("ReSharper", "IdentifierTypo")]
	public class WoodRunicElemental : Lich
	{
		public override bool AutoDispel => true;
		public virtual CraftResource CraftResource => CraftResource.RegularWood;

		public virtual Type ResourceType => typeof(Log);

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public double RunicDropChance { get; set; }

		[Constructable]
		public WoodRunicElemental(double runicDropChance)
		{
			RunicDropChance = runicDropChance;
			Name = $"a {CraftResource.ToString().ToLower()} wood elemental";
			Hue = CraftResources.GetHue(CraftResource);
			BodyValue = 301;
		}


		[Constructable]
		public WoodRunicElemental() : this(0.000000)
		{
		}

		public WoodRunicElemental(Serial serial) : base(serial)
		{
		}

		public override void GenerateLoot()
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);

			writer.Write(RunicDropChance);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					RunicDropChance = reader.ReadDouble();
					break;
				}
			}
		}

		public override void OnDeath(Container c)
		{
			if (RunicHarvestSystem.Enabled)
			{
				if (ResourceType.IsSubclassOf(typeof(BaseLog)))
				{
					if (Activator.CreateInstance(ResourceType, 10) is Item item)
					{
						c.DropItem(item);
					}
				}

				if (RunicDropChance > 0.00)
				{
					var value = Utility.RandomDouble() * 100.00;
					if (value <= RunicDropChance)
					{
						if (CraftResource.Equals(CraftResource.RegularWood))
						{
							if (Utility.RandomBool())
							{
								c.DropItem(new FletcherTools(RunicHarvestSystem.PlainUses));
							}
							else
							{
								c.DropItem(new DovetailSaw(RunicHarvestSystem.PlainUses));
							}
						}
						else
						{
							if (Utility.RandomBool())
							{
								c.DropItem(new RunicFletcherTool(CraftResource, RunicHarvestSystem.RunicUses));
							}
							else
							{
								c.DropItem(new RunicDovetailSaw(CraftResource, RunicHarvestSystem.RunicUses));
							}
						}
					}
				}
			}

			base.OnDeath(c);
		}
	}

	[SuppressMessage("ReSharper", "IdentifierTypo")]
	[CorpseName("an oakwood elemental corpse")]
	public class OakWoodRunicElemental : WoodRunicElemental
	{
		public override CraftResource CraftResource => CraftResource.OakWood;

		public override Type ResourceType => typeof(OakLog);

		[Constructable]
		public OakWoodRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public OakWoodRunicElemental()
		{
		}

		public OakWoodRunicElemental(Serial serial) : base(serial)
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
	}

	[SuppressMessage("ReSharper", "IdentifierTypo")]
	[CorpseName("an ashwood elemental corpse")]
	public class AshWoodRunicElemental : WoodRunicElemental
	{
		public override CraftResource CraftResource => CraftResource.AshWood;

		public override Type ResourceType => typeof(AshLog);

		[Constructable]
		public AshWoodRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public AshWoodRunicElemental()
		{
		}

		public AshWoodRunicElemental(Serial serial) : base(serial)
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
	}

	[SuppressMessage("ReSharper", "IdentifierTypo")]
	[CorpseName("a yewwood elemental corpse")]
	public class YewWoodRunicElemental : WoodRunicElemental
	{
		public override CraftResource CraftResource => CraftResource.YewWood;
		public override Type ResourceType => typeof(YewLog);

		[Constructable]
		public YewWoodRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public YewWoodRunicElemental()
		{
		}

		public YewWoodRunicElemental(Serial serial) : base(serial)
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
	}

	[SuppressMessage("ReSharper", "IdentifierTypo")]
	[CorpseName("a heartwood elemental corpse")]
	public class HeartwoodRunicElemental : WoodRunicElemental
	{
		public override CraftResource CraftResource => CraftResource.YewWood;
		public override Type ResourceType => typeof(HeartwoodLog);

		[Constructable]
		public HeartwoodRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public HeartwoodRunicElemental()
		{
		}

		public HeartwoodRunicElemental(Serial serial) : base(serial)
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
	}
}
