using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Server.Items;
using Server.Mobiles;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;
using Server.Services.Virtues;

namespace Server.RebirthUO.Modules.CustomChampionSystem.Mobiles
{
	[SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
	[SuppressMessage("ReSharper", "IntroduceOptionalParameters.Global")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	public abstract class SuperBaseChampion<T1, T2> : BaseCreature where T1 : Enum where T2 : Enum
	{
		public override bool CanBeParagon => false;
		public abstract T1 SkullType { get; }
		public abstract Type[] UniqueList { get; }
		public abstract Type[] SharedList { get; }
		public abstract Type[] DecorativeList { get; }
		public abstract MonsterStatuetteType[] StatueTypes { get; }
		public virtual bool NoGoodies => false;

		public virtual bool CanGivePowerScrolls => true;
		public virtual bool RestrictedToFelucca => true;
		public abstract int PowerScrollAmount { get; }

		protected SuperBaseChampion(AIType aiType)
			: this(aiType, FightMode.Closest)
		{
		}

		protected SuperBaseChampion(AIType aiType, FightMode mode)
			: base(aiType, mode, 18, 1, 0.1, 0.2)
		{
		}

		protected SuperBaseChampion(Serial serial)
			: base(serial)
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

		public virtual Item GetArtifact()
		{
			var random = Utility.RandomDouble();

			if (0.05 >= random)
			{
				return CreateArtifact(UniqueList);
			}

			if (0.15 >= random)
			{
				return CreateArtifact(SharedList);
			}

			if (0.30 >= random)
			{
				return CreateArtifact(DecorativeList);
			}

			return null;
		}

		public Item CreateArtifact(Type[] list)
		{
			if (list.Length == 0)
			{
				return null;
			}

			var random = Utility.Random(list.Length);

			var type = list[random];

			var artifact = Loot.Construct(type);

			if (artifact is MonsterStatuette statuette && StatueTypes.Length > 0)
			{
				statuette.Type = StatueTypes[Utility.Random(StatueTypes.Length)];
				statuette.LootType = LootType.Regular;
			}

			return artifact;
		}

		public virtual void GivePowerScrolls()
		{
			if (Map == null || (RestrictedToFelucca && Map.Rules != MapRules.FeluccaRules))
			{
				return;
			}

			var toGive = new List<Mobile>();
			var rights = GetLootingRights();

			for (var i = rights.Count - 1; i >= 0; --i)
			{
				var ds = rights[i];

				if (ds.m_HasRight && InRange(ds.m_Mobile, 100) && ds.m_Mobile.Map == Map)
				{
					toGive.Add(ds.m_Mobile);
				}
			}

			if (toGive.Count == 0)
			{
				return;
			}

			foreach (var mobile in toGive)
			{
				if (!(mobile is PlayerMobile))
				{
					continue;
				}

				var gainedPath = false;

				var pointsToGain = 800;

				if (VirtueHelper.Award(mobile, VirtueName.Valor, pointsToGain, ref gainedPath))
				{
					mobile.SendLocalizedMessage(gainedPath ? 1054032 : 1054030);
				}
			}

			// Randomize - PowerScrolls
			for (var i = 0; i < toGive.Count; ++i)
			{
				var rand = Utility.Random(toGive.Count);

				(toGive[i], toGive[rand]) = (toGive[rand], toGive[i]);
			}

			for (var i = 0; i < PowerScrollAmount; ++i)
			{
				var mobile = toGive[i % toGive.Count];

				var powerScroll = GetPowerScroll();
				GiveItemMessage(mobile, powerScroll);

				GivePowerScrollTo(mobile, powerScroll);
			}

			// Randomize - Primers
			for (var i = 0; i < toGive.Count; ++i)
			{
				var rand = Utility.Random(toGive.Count);
				(toGive[i], toGive[rand]) = (toGive[rand], toGive[i]);
			}

			for (var i = 0; i < PowerScrollAmount; ++i)
			{
				var mobile = toGive[i % toGive.Count];

				var primer = CreateRandomPrimer();
				GiveItemMessage(mobile, primer);

				GivePowerScrollTo(mobile, primer);
			}

			ColUtility.Free(toGive);
		}

		public virtual void GiveItemMessage(Mobile m, Item item)
		{
			if (m == null)
			{
				return;
			}

			if (item is ScrollOfTranscendence)
			{
				m.SendLocalizedMessage(1094936); // You have received a Scroll of Transcendence!
			}
			else if (item is SkillMasteryPrimer)
			{
				m.SendLocalizedMessage(1156209); // You have received a mastery primer!
			}
			else
			{
				m.SendLocalizedMessage(1049524); // You have received a scroll of power!
			}
		}

		public virtual void GivePowerScrollTo(Mobile m, Item item)
		{
			if (m == null)
			{
				return;
			}

			if (m.Alive)
			{
				m.AddToBackpack(item);
			}
			else
			{
				if (m.Corpse != null && !m.Corpse.Deleted)
				{
					m.Corpse.DropItem(item);
				}
				else
				{
					m.AddToBackpack(item);
				}
			}

			if (item is PowerScroll && m is PlayerMobile playerMobile)
			{
				foreach (var protector in playerMobile.JusticeProtectors)
				{
					if (protector.Map != playerMobile.Map || protector.Murderer || protector.Criminal ||
					    !JusticeVirtue.CheckMapRegion(playerMobile, protector) ||
					    !protector.InRange(this, 100))
					{
						continue;
					}

					var chance = 0;

					switch (VirtueHelper.GetLevel(protector, VirtueName.Justice))
					{
						case VirtueLevel.Seeker:
							chance = 60;
							break;
						case VirtueLevel.Follower:
							chance = 80;
							break;
						case VirtueLevel.Knight:
							chance = 100;
							break;
					}

					if (chance > Utility.Random(100))
					{
						var powerScroll = GetJusticePowerScroll();

						protector.SendLocalizedMessage(
							1049368); // You have been rewarded for your dedication to Justice!

						if (protector.Alive)
						{
							protector.AddToBackpack(powerScroll);
						}
						else
						{
							if (protector.Corpse != null && !protector.Corpse.Deleted)
							{
								protector.Corpse.DropItem(powerScroll);
							}
							else
							{
								protector.AddToBackpack(powerScroll);
							}
						}
					}
				}
			}
		}

		public virtual Item GetPowerScroll()
		{
			return CreateRandomPowerScroll();
		}

		public virtual Item GetJusticePowerScroll()
		{
			return CreateRandomPowerScroll();
		}

		public virtual Item CreateRandomPowerScroll()
		{
			int level;
			var random = Utility.RandomDouble();

			if (0.05 >= random)
			{
				level = 20;
			}
			else if (0.4 >= random)
			{
				level = 15;
			}
			else
			{
				level = 10;
			}

			return PowerScroll.CreateRandomNoCraft(level, level);
		}

		public virtual SkillMasteryPrimer CreateRandomPrimer()
		{
			return SkillMasteryPrimer.GetRandom();
		}

		public abstract void OnChampPopped(SuperChampionSpawn<T2> spawn);

		public override bool OnBeforeDeath()
		{
			if (CanGivePowerScrolls && !NoKillAwards)
			{
				GivePowerScrolls();

				if (NoGoodies)
				{
					return base.OnBeforeDeath();
				}

				DoRewardShower();
			}

			return base.OnBeforeDeath();
		}

		protected abstract void DoRewardShower();

		public override void OnDeath(Container c)
		{
			if (Map == Map.Felucca)
			{
				var rights = GetLootingRights();
				var toGive = new List<Mobile>();

				for (var i = rights.Count - 1; i >= 0; --i)
				{
					var ds = rights[i];

					if (ds.m_HasRight)
					{
						toGive.Add(ds.m_Mobile);
					}
				}

				if (CanProvideSkull())
				{
					if (toGive.Count > 0)
					{
						toGive[Utility.Random(toGive.Count)].AddToBackpack(CreateChampionSkull());
					}
					else
					{
						c.DropItem(CreateChampionSkull());
					}
				}

				RefinementComponent.Roll(c, 3, 0.10);
			}

			base.OnDeath(c);
		}

		protected abstract SuperChampionSkull<T1> CreateChampionSkull();

		protected abstract bool CanProvideSkull();
	}
}
