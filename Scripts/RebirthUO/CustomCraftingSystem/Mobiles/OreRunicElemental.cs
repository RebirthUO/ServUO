// **********
// ServUO - OreElemental.cs
// **********

using System;
using System.Diagnostics.CodeAnalysis;
using Server.Items;
using Server.Mobiles;
using Server.RebirthUO.CustomCraftingSystem.Config;
using Server.RebirthUO.CustomCraftingSystem.Interfaces;
using Server.RebirthUO.CustomCraftingSystem.Items.Tools;

namespace Server.RebirthUO.CustomCraftingSystem.Mobiles
{
	[CorpseName("an iron ore elemental corpse")]
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	[SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	public class OreRunicElemental : EarthElemental, IRunicElemental
	{
		public virtual CraftResource CraftResource => CraftResource.Iron;

		public override bool AutoDispel => true;
		public virtual Type ResourceType => typeof(IronOre);

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public double RunicDropChance { get; set; }

		public void InitMobile()
		{
			SetStr(126, 155);
			SetDex(66, 85);
			SetInt(71, 92);

			SetHits(76, 93);

			SetDamage(9, 16);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 30, 35);
			SetResistance(ResistanceType.Fire, 10, 20);
			SetResistance(ResistanceType.Cold, 10, 20);
			SetResistance(ResistanceType.Poison, 15, 25);
			SetResistance(ResistanceType.Energy, 15, 25);

			SetSkill(SkillName.MagicResist, 50.1, 95.0);
			SetSkill(SkillName.Tactics, 60.1, 100.0);
			SetSkill(SkillName.Wrestling, 60.1, 100.0);

			Fame = 3500;
			Karma = -3500;
		}

		[Constructable]
		public OreRunicElemental(double runicDropChance)
		{
			RunicDropChance = runicDropChance;
			Name = $"a {CraftResource.ToString().ToLower()} ore elemental";
			Hue = CraftResources.GetHue(CraftResource);
		}

		[Constructable]
		public OreRunicElemental() : this(0.000000)
		{
		}

		public OreRunicElemental(Serial serial) : base(serial)
		{
		}

		public override void GenerateLoot()
		{
		}

		public override void OnDeath(Container c)
		{
			if (RunicHarvestSystem.Enabled)
			{
				if (ResourceType.IsSubclassOf(typeof(BaseOre)))
				{
					if (Activator.CreateInstance(ResourceType, RunicHarvestSystem.ResourceAmount) is Item item)
					{
						c.DropItem(item);
					}
				}

				if (RunicDropChance > 0.00)
				{
					var value = Utility.RandomDouble() * 100.00;

					if (value <= RunicDropChance)
					{
						if (CraftResource.Equals(CraftResource.Iron))
						{
							switch (Utility.RandomMinMax(1, 3))
							{
								case 1:
								{
									c.DropItem(new SmithHammer(RunicHarvestSystem.PlainUses));
									break;
								}
								case 2:
								{
									c.DropItem(new MalletAndChisel(RunicHarvestSystem.PlainUses));
									break;
								}
								case 3:
								{
									c.DropItem(new TinkerTools(RunicHarvestSystem.PlainUses));
									break;
								}
							}
						}
						else
						{
							switch (Utility.RandomMinMax(1, 3))
							{
								case 1:
								{
									c.DropItem(new RunicHammer(CraftResource, RunicHarvestSystem.RunicUses));
									break;
								}
								case 2:
								{
									c.DropItem(new RunicMalletAndChisel(CraftResource, RunicHarvestSystem.RunicUses));
									break;
								}
								case 3:
								{
									c.DropItem(new RunicTinkerTool(CraftResource, RunicHarvestSystem.RunicUses));
									break;
								}
							}
						}
					}
				}
			}

			base.OnDeath(c);
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
	}

	[SuppressMessage("ReSharper", "IdentifierTypo")]
	[CorpseName("an dull copper ore elemental corpse")]
	public class DullCopperRunicRunicElemental : OreRunicElemental
	{
		public override CraftResource CraftResource => CraftResource.DullCopper;

		public override Type ResourceType => typeof(DullCopperOre);

		[Constructable]
		public DullCopperRunicRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public DullCopperRunicRunicElemental()
		{
		}

		public DullCopperRunicRunicElemental(Serial serial) : base(serial)
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
	[CorpseName("an shadow ore elemental corpse")]
	public class ShadowIronRunicRunicElemental : OreRunicElemental
	{
		public override CraftResource CraftResource => CraftResource.ShadowIron;

		public override Type ResourceType => typeof(ShadowIronIngot);

		[Constructable]
		public ShadowIronRunicRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public ShadowIronRunicRunicElemental()
		{
		}

		public ShadowIronRunicRunicElemental(Serial serial) : base(serial)
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
	[CorpseName("an copper ore elemental corpse")]
	public class CopperRunicRunicElemental : OreRunicElemental
	{
		public override CraftResource CraftResource => CraftResource.Copper;

		public override Type ResourceType => typeof(CopperOre);

		[Constructable]
		public CopperRunicRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public CopperRunicRunicElemental()
		{
		}

		public CopperRunicRunicElemental(Serial serial) : base(serial)
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
	[CorpseName("an bronze ore elemental corpse")]
	public class BronzeRunicRunicElemental : OreRunicElemental
	{
		public override CraftResource CraftResource => CraftResource.Bronze;

		public override Type ResourceType => typeof(BronzeOre);

		[Constructable]
		public BronzeRunicRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public BronzeRunicRunicElemental()
		{
		}

		public BronzeRunicRunicElemental(Serial serial) : base(serial)
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
	[CorpseName("an gold ore elemental corpse")]
	public class GoldRunicRunicElemental : OreRunicElemental
	{
		public override CraftResource CraftResource => CraftResource.Gold;

		public override Type ResourceType => typeof(GoldOre);

		[Constructable]
		public GoldRunicRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public GoldRunicRunicElemental()
		{
		}

		public GoldRunicRunicElemental(Serial serial) : base(serial)
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
	[CorpseName("an agapite ore elemental corpse")]
	public class AgapiteRunicRunicElemental : OreRunicElemental
	{
		public override CraftResource CraftResource => CraftResource.Agapite;

		public override Type ResourceType => typeof(AgapiteOre);

		[Constructable]
		public AgapiteRunicRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public AgapiteRunicRunicElemental()
		{
		}

		public AgapiteRunicRunicElemental(Serial serial) : base(serial)
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
	[CorpseName("an verite ore elemental corpse")]
	public class VeriteRunicRunicElemental : OreRunicElemental
	{
		public override CraftResource CraftResource => CraftResource.Verite;

		public override Type ResourceType => typeof(VeriteOre);

		[Constructable]
		public VeriteRunicRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public VeriteRunicRunicElemental()
		{
		}

		public VeriteRunicRunicElemental(Serial serial) : base(serial)
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
	[CorpseName("an valorite ore elemental corpse")]
	public class ValoriteRunicRunicElemental : OreRunicElemental
	{
		public override CraftResource CraftResource => CraftResource.Valorite;

		public override Type ResourceType => typeof(ValoriteOre);

		[Constructable]
		public ValoriteRunicRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public ValoriteRunicRunicElemental()
		{
		}
		
		public ValoriteRunicRunicElemental(Serial serial) : base(serial)
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
