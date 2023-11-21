// **********
// ServUO - Mule.cs
// **********

using System.Collections.Generic;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;

namespace Server.RebirthUO.CustomCraftingSystem.Mobiles
{
	public class Mule : BaseMount
	{
		public override bool SubdueBeforeTame => true;

		public bool FirstTimeTamed { get; set; }

		public override Poison PoisonImmune => Poison.Lethal;

		[Constructable]
		public Mule() : base("a mule", 0x76, 0x3EB2, AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			BaseSoundID = 0xA8;

			Fame = 2000;
			Karma = -2000;

			Tamable = true;
			ControlSlots = 1;

			SetStr(200);
			SetDex(75);
			SetInt(150);

			SetHits(95, 105);

			SetDamage(6, 17);

			SetDamageType(ResistanceType.Physical, 75);
			SetDamageType(ResistanceType.Cold, 25);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Cold, 55, 60);
			SetResistance(ResistanceType.Poison, 100);
			SetResistance(ResistanceType.Energy, 15, 22);

			SetSkill(SkillName.MagicResist, 90.0);
			SetSkill(SkillName.Tactics, 75.0);
			SetSkill(SkillName.Wrestling, 80.1, 90.0);
			SetSkill(SkillName.EvalInt, 10.1, 11.0);
			SetSkill(SkillName.Magery, 10.1, 11.0);
			SetSkill(SkillName.Meditation, 10.1, 11.0);
			FirstTimeTamed = true;
		}

		public Mule(Serial serial) : base(serial)
		{
		}

		public override void OnAfterTame(Mobile tamer)
		{
			if (FirstTimeTamed)
			{
				PackItem(new Leather(100));
				PackItem(new SpinedLeather(75));
				PackItem(new HornedLeather(50));
				PackItem(new BarbedLeather(25));

				PackItem(new IronIngot(100));
				PackItem(new DullCopperIngot(90));
				PackItem(new ShadowIronIngot(80));
				PackItem(new CopperIngot(70));
				PackItem(new BronzeIngot(60));
				PackItem(new GoldIngot(50));
				PackItem(new AgapiteIngot(40));
				PackItem(new VeriteIngot(30));
				PackItem(new ValoriteIngot(20));

				PackItem(new Board(100));
				PackItem(new OakBoard(90));
				PackItem(new AshBoard(80));
				PackItem(new YewBoard(60));
				PackItem(new HeartwoodBoard(40));
				PackItem(new BloodwoodBoard(30));
				PackItem(new FrostwoodBoard(20));
			}

			FirstTimeTamed = false;
			MinTameSkill = 30.0;
		}

		public override int GetAngerSound()
		{
			if (!Controlled)
			{
				return 0x16A;
			}

			return base.GetAngerSound();
		}

		public override bool CheckNonlocalDrop(Mobile from, Item item, Item target)
		{
			return PackAnimal.CheckAccess(this, from);
		}

		public override bool CheckNonlocalLift(Mobile from, Item item)
		{
			return PackAnimal.CheckAccess(this, from);
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);

			PackAnimal.GetContextMenuEntries(this, from, list);
		}

		public override bool OnDragDrop(Mobile from, Item item)
		{
			if (CheckFeed(from, item))
			{
				return true;
			}

			if (PackAnimal.CheckAccess(this, from))
			{
				AddToBackpack(item);
				return true;
			}

			return base.OnDragDrop(from, item);
		}

		public override bool IsSnoop(Mobile from)
		{
			if (PackAnimal.CheckAccess(this, from))
			{
				return false;
			}

			return base.IsSnoop(from);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(FirstTimeTamed);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();

			switch (version)
			{
				case 0:
				{
					FirstTimeTamed = reader.ReadBool();
					break;
				}
			}
		}
	}
}
