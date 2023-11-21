// **********
// ServUO - LeatherElemental.cs
// **********

using System.Diagnostics.CodeAnalysis;
using Server.Items;
using Server.Mobiles;
using Server.RebirthUO.CustomCraftingSystem.Config;
using Server.RebirthUO.CustomCraftingSystem.Interfaces;

namespace Server.RebirthUO.CustomCraftingSystem.Mobiles
{
	[CorpseName("a leather elemental corpse")]
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	[SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	public class LeatherRunicElemental : Centaur, IRunicElemental
	{
		public override bool AutoDispel => true;
		public virtual CraftResource CraftResource => CraftResource.RegularLeather;
		public override HideType HideType => HideType.Regular;
		public override ScaleType ScaleType => ScaleType.White;

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public double RunicDropChance { get; set; }

		public virtual void InitMobile()
		{
			
			SetStr(202, 300);
			SetDex(104, 260);
			SetInt(91, 100);

			SetHits(130, 172);

			SetDamage(13, 24);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 45, 55);
			SetResistance(ResistanceType.Fire, 35, 45);
			SetResistance(ResistanceType.Cold, 25, 35);
			SetResistance(ResistanceType.Poison, 45, 55);
			SetResistance(ResistanceType.Energy, 35, 45);

			SetSkill(SkillName.Anatomy, 95.1, 115.0);
			SetSkill(SkillName.Archery, 95.1, 100.0);
			SetSkill(SkillName.MagicResist, 50.3, 80.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 95.1, 100.0);

			Fame = 6500;
			Karma = 0;
		}

		public override int Hides => RunicHarvestSystem.ResourceAmount;
		public override int Scales => RunicHarvestSystem.ResourceAmount;

		[Constructable]
		public LeatherRunicElemental(double runicDropChance)
		{
			RunicDropChance = runicDropChance;
			Name = $"a {HideType.ToString().ToLower()} leather elemental";
			Hue = CraftResources.GetHue(CraftResource);
			InitMobile();
		}

		[Constructable]
		public LeatherRunicElemental() : this(0.00)
		{
		}

		public LeatherRunicElemental(Serial serial) : base(serial)
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
				if (RunicDropChance > 0.00)
				{
					var value = Utility.RandomDouble() * 100.00;
					if (value <= RunicDropChance)
					{
						if (CraftResource.Equals(CraftResource.RegularLeather))
						{
							c.DropItem(new SewingKit(RunicHarvestSystem.PlainUses));
						}
						else
						{
							c.DropItem(new RunicSewingKit(CraftResource, RunicHarvestSystem.RunicUses));
						}
					}
				}
			}

			base.OnDeath(c);
		}
	}

	[SuppressMessage("ReSharper", "IdentifierTypo")]
	[CorpseName("a spined leather elemental corpse")]
	public class SpinedLeatherRunicElemental : LeatherRunicElemental
	{
		public override HideType HideType => HideType.Spined;
		public override ScaleType ScaleType => ScaleType.Blue;
		public override CraftResource CraftResource => CraftResource.SpinedLeather;

		[Constructable]
		public SpinedLeatherRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public SpinedLeatherRunicElemental()
		{
		}
		public SpinedLeatherRunicElemental(Serial serial) : base(serial)
		{
		}
		
		public override void InitMobile()
		{
			
			SetStr(302, 400);
			SetDex(204, 360);
			SetInt(191, 200);

			SetHits(230, 372);

			SetDamage(18, 29);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 55, 65);
			SetResistance(ResistanceType.Fire, 45, 55);
			SetResistance(ResistanceType.Cold, 35, 45);
			SetResistance(ResistanceType.Poison, 55, 65);
			SetResistance(ResistanceType.Energy, 45, 55);

			SetSkill(SkillName.Anatomy, 105.1, 120.0);
			SetSkill(SkillName.Archery, 105.1, 110.0);
			SetSkill(SkillName.MagicResist, 60.3, 90.0);
			SetSkill(SkillName.Tactics, 100.1, 110.0);
			SetSkill(SkillName.Wrestling, 105.1, 115.0);

			Fame = 9500;
			Karma = 0;
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

	[CorpseName("a horned leather elemental corpse")]
	[SuppressMessage("ReSharper", "IdentifierTypo")]
	public class HornedLeatherRunicElemental : LeatherRunicElemental
	{
		public override HideType HideType => HideType.Horned;
		public override ScaleType ScaleType => ScaleType.Red;
		public override CraftResource CraftResource => CraftResource.HornedLeather;


		[Constructable]
		public HornedLeatherRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public HornedLeatherRunicElemental()
		{
		}
		
		public HornedLeatherRunicElemental(Serial serial) : base(serial)
		{
		}
		
		public override void InitMobile()
		{
			
			SetStr(402, 500);
			SetDex(304, 460);
			SetInt(291, 300);

			SetHits(330, 472);

			SetDamage(23, 34);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 65, 75);
			SetResistance(ResistanceType.Fire, 55, 65);
			SetResistance(ResistanceType.Cold, 45, 55);
			SetResistance(ResistanceType.Poison, 65, 75);
			SetResistance(ResistanceType.Energy, 55, 65);

			SetSkill(SkillName.Anatomy, 115.1, 120.0);
			SetSkill(SkillName.Archery, 115.1, 120.0);
			SetSkill(SkillName.MagicResist, 70.3, 100.0);
			SetSkill(SkillName.Tactics, 110.1, 120.0);
			SetSkill(SkillName.Wrestling, 115.1, 120.0);

			Fame = 12500;
			Karma = 0;
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

	[CorpseName("a barbed leather elemental corpse")]
	[SuppressMessage("ReSharper", "IdentifierTypo")]
	public class BarbedLeatherRunicElemental : LeatherRunicElemental
	{
		public override HideType HideType => HideType.Barbed;
		public override ScaleType ScaleType => ScaleType.Green;
		public override CraftResource CraftResource => CraftResource.BarbedLeather;

		[Constructable]
		public BarbedLeatherRunicElemental(double runicDropChance) : base(runicDropChance)
		{
		}

		[Constructable]
		public BarbedLeatherRunicElemental()
		{
		}
		public override void InitMobile()
		{
			
			SetStr(502, 600);
			SetDex(404, 560);
			SetInt(391, 400);

			SetHits(430, 572);

			SetDamage(28, 37);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 75, 85);
			SetResistance(ResistanceType.Fire, 65, 75);
			SetResistance(ResistanceType.Cold, 55, 65);
			SetResistance(ResistanceType.Poison, 75, 85);
			SetResistance(ResistanceType.Energy, 65, 75);

			SetSkill(SkillName.Anatomy, 115.1, 120.0);
			SetSkill(SkillName.Archery, 115.1, 120.0);
			SetSkill(SkillName.MagicResist, 70.3, 100.0);
			SetSkill(SkillName.Tactics, 110.1, 120.0);
			SetSkill(SkillName.Wrestling, 115.1, 120.0);

			Fame = 15000;
			Karma = 0;
		}
		public BarbedLeatherRunicElemental(Serial serial) : base(serial)
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
