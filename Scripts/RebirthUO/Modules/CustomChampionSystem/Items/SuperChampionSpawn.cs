using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Server.Diagnostics;
using Server.Engines.CannedEvil;
using Server.Engines.CityLoyalty;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.RebirthUO.Modules.CustomChampionSystem.Addons;
using Server.RebirthUO.Modules.CustomChampionSystem.Gumps;
using Server.RebirthUO.Modules.CustomChampionSystem.Regions;
using Server.Services.Virtues;
using Server.Spells.Necromancy;

namespace Server.RebirthUO.Modules.CustomChampionSystem.Items
{
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	[SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
	[SuppressMessage("ReSharper", "PublicConstructorInAbstractClass")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	public abstract class SuperChampionSpawn<T> : Item where T : Enum
	{
		private DateTime _NextGhostCheck;

		private Rectangle2D _SpawnArea;
		private int InternalKills { get; set; }
		private SuperChampionPlatform<T> Platform { get; set; }
		protected SuperChampionAltar<T> Altar { get; set; }
		private SuperIdolOfTheChampion<T> Idol { get; set; }
		private SuperChampionSpawnRegion<T> Region { get; set; }
		private bool InternalActive { get; set; }

		private bool TimerRunning => TimerRegistry.HasTimer(TimerId, this);

		private bool RestartTimerRunning => TimerRegistry.HasTimer(RestartTimerId, this);
		protected abstract string RestartTimerId { get; }
		protected abstract string TimerId { get; }
		public Dictionary<Mobile, int> DamageEntries { get; private set; }
		private List<Mobile> Creatures { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public string GroupName { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public double SpawnMod { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int SpawnRadius { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public double KillsMod { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool AutoRestart { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public string SpawnName { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool ConfinedRoaming { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool HasBeenAdvanced { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool RandomizeType { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan RestartDelay { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime RestartTime { get; private set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan ExpireDelay { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime ExpireTime { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Rectangle2D SpawnArea
		{
			get => _SpawnArea;
			set
			{
				_SpawnArea = value;
				InvalidateProperties();
				UpdateRegion();
			}
		}

		private List<Item> InternalWhiteSkulls { get; set; }
		private List<Item> InternalRedSkulls { get; set; }
		private T InternalType { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Level
		{
			get => InternalRedSkulls.Count;
			set
			{
				for (var i = InternalRedSkulls.Count - 1; i >= value; --i)
				{
					InternalRedSkulls[i].Delete();
					InternalRedSkulls.RemoveAt(i);
				}

				for (var i = InternalRedSkulls.Count; i < value; ++i)
				{
					var skull = new Item(0x1854) { Hue = 0x26, Movable = false, Light = LightType.Circle150 };

					skull.MoveToWorld(GetRedSkullLocation(i), Map);

					InternalRedSkulls.Add(skull);
				}

				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int Kills
		{
			get => InternalKills;
			set
			{
				InternalKills = value;
				InvalidateProperties();
			}
		}


		[CommandProperty(AccessLevel.GameMaster)]
		public T Type
		{
			get => InternalType;
			set
			{
				InternalType = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Active
		{
			get => InternalActive;
			set
			{
				if (value)
				{
					Start();
				}
				else
				{
					Stop();
				}

				HandleGlobalEvents();

				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Champion { get; set; }


		[CommandProperty(AccessLevel.GameMaster)]
		public int StartLevel { get; private set; }

		public int MaxKills => GetMaxKillsForLevel(Level);

		public int Rank => GetRankForLevel(Level);
		protected abstract double GetMaxStrayDistance { get; }

		public SuperChampionSpawn() : base(0xBD2)
		{
			Movable = false;
			Visible = false;

			Creatures = new List<Mobile>();
			InternalRedSkulls = new List<Item>();
			InternalWhiteSkulls = new List<Item>();

			Platform = CreateChampionPlatform();
			Altar = CreateChampionAltar();
			Idol = CreateIdolOfChampion();

			ExpireDelay = TimeSpan.FromMinutes(10.0);
			RestartDelay = TimeSpan.FromMinutes(10.0);

			DamageEntries = new Dictionary<Mobile, int>();
			RandomizeType = false;

			SpawnRadius = 35;
			SpawnMod = 1;

			Server.Timer.DelayCall(TimeSpan.Zero, GetInitialSpawnArea);
		}

		public SuperChampionSpawn(Serial serial) : base(serial)
		{
		}

		protected abstract SuperIdolOfTheChampion<T> CreateIdolOfChampion();
		protected abstract SuperChampionPlatform<T> CreateChampionPlatform();
		protected abstract SuperChampionAltar<T> CreateChampionAltar();
		protected abstract SuperChampionSpawnRegion<T> CreateChampionSpawnRegion();

		protected abstract void HandleGlobalEvents();
		protected abstract int GetMaxKillsForLevel(int level);
		protected abstract int GetRankForLevel(int level);

		protected virtual void GetInitialSpawnArea()
		{
			_SpawnArea = new Rectangle2D(new Point2D(X - SpawnRadius, Y - SpawnRadius),
				new Point2D(X + SpawnRadius, Y + SpawnRadius));
		}

		private void UpdateRegion()
		{
			Region?.Unregister();

			if (!Deleted && Map != Map.Internal)
			{
				Region = CreateChampionSpawnRegion();
				Region.Register();
			}
		}

		private Point3D GetRedSkullLocation(int index)
		{
			int x, y;

			if (index < 5)
			{
				x = index - 2;
				y = -2;
			}
			else if (index < 9)
			{
				x = 2;
				y = index - 6;
			}
			else if (index < 13)
			{
				x = 10 - index;
				y = 2;
			}
			else
			{
				x = -2;
				y = 14 - index;
			}

			return new Point3D(X + x, Y + y, Z - 15);
		}

		public bool IsChampionSpawn(Mobile m)
		{
			return Creatures.Contains(m);
		}

		private void Start(bool serverLoad = false)
		{
			if (InternalActive || Deleted)
			{
				return;
			}

			InternalActive = true;
			HasBeenAdvanced = false;

			TimerRegistry.Register(TimerId, this, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), false,
				spawner => spawner.OnSlice());
			TimerRegistry.RemoveFromRegistry(RestartTimerId, this);

			if (Altar != null)
			{
				Altar.Hue = 0;
			}

			HandleGlobalEvents();

			if (!serverLoad)
			{
				var chance = Utility.RandomDouble();

				if (chance < 0.1)
				{
					Level = 4;
				}
				else if (chance < 0.25)
				{
					Level = 3;
				}
				else if (chance < 0.5)
				{
					Level = 2;
				}
				else if (Utility.RandomBool())
				{
					Level = 1;
				}

				StartLevel = Level;

				if (Level > 0 && Altar != null)
				{
					Effects.PlaySound(Altar.Location, Altar.Map, 0x29);
					Effects.SendLocationEffect(new Point3D(Altar.X + 1, Altar.Y + 1, Altar.Z), Altar.Map,
						0x3728, 10);
				}
			}
		}

		private void Stop()
		{
			if (!InternalActive || Deleted)
			{
				return;
			}

			InternalActive = false;
			HasBeenAdvanced = false;

			if (Creatures != null)
			{
				foreach (var mobile in Creatures)
				{
					mobile.Delete();
				}

				Creatures.Clear();
			}

			TimerRegistry.RemoveFromRegistry(TimerId, this);
			TimerRegistry.RemoveFromRegistry(RestartTimerId, this);

			if (Altar != null)
			{
				Altar.Hue = 0x455;
			}

			HandleGlobalEvents();

			RemoveSkulls();
			InternalKills = 0;
		}

		private void OnSlice()
		{
			if (!InternalActive || Deleted)
			{
				return;
			}

			var currentRank = Rank;

			if (Champion != null)
			{
				if (Champion.Deleted)
				{
					RegisterDamageTo(Champion);

					if (Champion is BaseChampion champion)
					{
						AwardArtifact(champion.GetArtifact());
					}

					DamageEntries.Clear();

					if (Altar != null)
					{
						Altar.Hue = 0x455;

						if (Map == Map.Felucca)
						{
							GenerateNewStarRoomGate();
						}
					}

					Champion = null;
					Stop();

					if (AutoRestart)
					{
						BeginRestart(RestartDelay);
					}
				}
				else if (Champion.Alive && Champion.GetDistanceToSqrt(this) > GetMaxStrayDistance)
				{
					Champion.MoveToWorld(new Point3D(X, Y, Z - 15), Map);
				}
			}
			else
			{
				var kills = this.InternalKills;

				for (var i = 0; i < Creatures.Count; ++i)
				{
					var m = Creatures[i];

					if (m.Deleted)
					{
						Creatures.RemoveAt(i);
						--i;

						var rankOfMob = GetRankFor(m);
						if (rankOfMob == currentRank)
						{
							++this.InternalKills;
						}

						var killer = m.FindMostRecentDamager(false);

						RegisterDamageTo(m);

						if (killer is BaseCreature creature)
						{
							killer = creature.GetMaster();
						}

						if (killer is PlayerMobile pm)
						{
							#region Scroll of Transcendence

							if (Map == Map.Felucca)
							{
								if (Utility.RandomDouble() < ChampionSystem.ScrollChance)
								{
									if (Utility.RandomDouble() < ChampionSystem.TranscendenceChance)
									{
										var scrollOfTranscendence = CreateRandomSoT(true);
										GiveScrollTo(pm, scrollOfTranscendence);
									}
									else
									{
										var powerScroll = CreateRandomPowerScroll();
										GiveScrollTo(pm, powerScroll);
									}
								}
							}

							if (Map == Map.Ilshenar || Map == Map.Tokuno || Map == Map.Malas)
							{
								if (Utility.RandomDouble() < 0.0015)
								{
									pm.SendLocalizedMessage(
										1094936); // You have received a Scroll of Transcendence!
									var scrollOfTranscendence = CreateRandomSoT(false);
									pm.AddToBackpack(scrollOfTranscendence);
								}
							}

							#endregion

							var mobSubLevel = rankOfMob + 1;
							if (mobSubLevel >= 0)
							{
								var gainedPath = false;

								var pointsToGain = mobSubLevel * 40;

								if (VirtueHelper.Award(pm, VirtueName.Valor, pointsToGain, ref gainedPath))
								{
									pm.SendLocalizedMessage(gainedPath ? 1054032 : 1054030); 
								}

								AwardChampionTitles();

								CityLoyaltySystem.OnSpawnCreatureKilled(m as BaseCreature, mobSubLevel);
							}
						}
					}
				}

				// Only really needed once.
				if (this.InternalKills > kills)
				{
					InvalidateProperties();
				}

				var n = this.InternalKills / (double)MaxKills;
				var p = (int)(n * 100);

				if (p >= 90)
				{
					AdvanceLevel();
				}
				else if (p > 0)
				{
					SetWhiteSkullCount(p / 20);
				}

				if (DateTime.UtcNow >= ExpireTime)
				{
					Expire();
				}

				Respawn();
			}

			if (TimerRunning && _NextGhostCheck < DateTime.UtcNow && Region != null)
			{
				foreach (var ghost in Region.AllPlayers.OfType<PlayerMobile>()
					         .Where(pm => !pm.Alive && (pm.Corpse == null || pm.Corpse.Deleted)))
				{
					var map = ghost.Map;
					var loc = ExorcismSpell.GetNearestShrine(ghost, ref map);

					if (loc != Point3D.Zero)
					{
						ghost.MoveToWorld(loc, map);
					}
					else
					{
						ghost.MoveToWorld(new Point3D(989, 520, -50), Map.Malas);
					}
				}

				_NextGhostCheck = DateTime.UtcNow + TimeSpan.FromMinutes(Utility.RandomMinMax(5, 8));
			}
		}

		protected abstract void AwardChampionTitles();

		protected abstract PowerScroll CreateRandomPowerScroll();

		protected abstract void GenerateNewStarRoomGate();

		private void RemoveSkulls()
		{
			if (InternalWhiteSkulls != null)
			{
				foreach (var item in InternalWhiteSkulls)
				{
					item.Delete();
				}

				InternalWhiteSkulls.Clear();
			}

			if (InternalRedSkulls != null)
			{
				foreach (var item in InternalRedSkulls)
				{
					item.Delete();
				}

				InternalRedSkulls.Clear();
			}
		}

		protected virtual void RegisterDamageTo(Mobile m)
		{
			if (m == null)
			{
				return;
			}

			foreach (var de in m.DamageEntries)
			{
				if (de.HasExpired)
				{
					continue;
				}

				var mobile = de.Damager;

				var master = mobile.GetDamageMaster(m);

				if (master != null)
				{
					mobile = master;
				}

				RegisterDamage(mobile, de.DamageGiven);
			}
		}

		private void RegisterDamage(Mobile from, int amount)
		{
			if (from == null || !from.Player)
			{
				return;
			}

			if (DamageEntries.ContainsKey(from))
			{
				DamageEntries[from] += amount;
			}
			else
			{
				DamageEntries.Add(from, amount);
			}
		}

		private void BeginRestart(TimeSpan ts)
		{
			TimerRegistry.Register(RestartTimerId, this, ts, spawner => spawner.EndRestart());

			RestartTime = DateTime.UtcNow + ts;
		}

		private void EndRestart()
		{
			if (RandomizeType)
			{
				Type = GetRandomType();
			}

			HasBeenAdvanced = false;

			Start();
		}

		private void AwardArtifact(Item artifact)
		{
			if (artifact == null)
			{
				return;
			}

			var totalDamage = 0;

			var validEntries = new Dictionary<Mobile, int>();

			foreach (var kvp in DamageEntries.Where(kvp => IsEligible(kvp.Key, artifact)))
			{
				validEntries.Add(kvp.Key, kvp.Value);
				totalDamage += kvp.Value;
			}

			var randomDamage = Utility.RandomMinMax(1, totalDamage);

			totalDamage = 0;

			foreach (var kvp in validEntries)
			{
				totalDamage += kvp.Value;

				if (totalDamage >= randomDamage)
				{
					GiveArtifact(kvp.Key, artifact);
					return;
				}
			}

			artifact.Delete();
		}

		private bool IsEligible(Mobile m, Item artifact)
		{
			return m.Player && m.Alive && m.Region != null && m.Region == Region && m.Backpack != null &&
			       m.Backpack.CheckHold(m, artifact, false);
		}

		private void GiveArtifact(Mobile to, Item artifact)
		{
			if (to == null || artifact == null)
			{
				return;
			}

			to.PlaySound(0x5B4);

			var pack = to.Backpack;

			if (pack == null || !pack.TryDropItem(to, artifact, false))
			{
				artifact.Delete();
			}
			else
			{
				to.SendLocalizedMessage(1062317);
			}
		}

		private int GetRankFor(Mobile m)
		{
			var types = GetSpawnTypes();
			var t = m.GetType();

			for (var i = 0; i < types.GetLength(0); i++)
			{
				var individualTypes = types[i];

				if (individualTypes.Any(individualType => t == individualType))
				{
					return i;
				}
			}

			return -1;
		}

		protected virtual ScrollOfTranscendence CreateRandomSoT(bool felucca)
		{
			var level = Utility.RandomMinMax(1, 5);

			if (felucca)
			{
				level += 5;
			}

			return ScrollOfTranscendence.CreateRandom(level, level);
		}

		private void AdvanceLevel()
		{
			ExpireTime = DateTime.UtcNow + ExpireDelay;

			if (Level < 16)
			{
				InternalKills = 0;
				++Level;
				InvalidateProperties();
				SetWhiteSkullCount(0);

				if (Altar != null)
				{
					Effects.PlaySound(Altar.Location, Altar.Map, 0x29);
					Effects.SendLocationEffect(new Point3D(Altar.X + 1, Altar.Y + 1, Altar.Z), Altar.Map,
						0x3728, 10);
				}
			}
			else
			{
				SpawnChampion();
			}
		}

		private void SpawnChampion()
		{
			InternalKills = 0;
			Level = 0;
			StartLevel = 0;
			InvalidateProperties();
			SetWhiteSkullCount(0);

			try
			{
				Champion = GenerateChampion();
			}
			catch (Exception e)
			{
				ExceptionLogging.LogException(e);
			}

			if (Champion != null)
			{
				var p = new Point3D(X, Y, Z - 15);

				Champion.MoveToWorld(p, Map);

				if (Champion is BaseCreature creature)
				{
					creature.Home = p;
				}

				HandleChampionEvents(Champion);
			}
		}

		private void SetWhiteSkullCount(int val)
		{
			for (var i = InternalWhiteSkulls.Count - 1; i >= val; --i)
			{
				InternalWhiteSkulls[i].Delete();
				InternalWhiteSkulls.RemoveAt(i);
			}

			for (var i = InternalWhiteSkulls.Count; i < val; ++i)
			{
				var skull = new Item(0x1854) { Movable = false, Light = LightType.Circle150 };

				skull.MoveToWorld(GetWhiteSkullLocation(i), Map);

				InternalWhiteSkulls.Add(skull);

				Effects.PlaySound(skull.Location, skull.Map, 0x29);
				Effects.SendLocationEffect(new Point3D(skull.X + 1, skull.Y + 1, skull.Z), skull.Map, 0x3728, 10);
			}
		}

		private Point3D GetWhiteSkullLocation(int index)
		{
			int x, y;

			switch (index)
			{
				default:
					x = -1;
					y = -1;
					break;
				case 1:
					x = 1;
					y = -1;
					break;
				case 2:
					x = 1;
					y = 1;
					break;
				case 3:
					x = -1;
					y = 1;
					break;
			}

			return new Point3D(X + x, Y + y, Z - 15);
		}

		private void GiveScrollTo(Mobile killer, SpecialScroll scroll)
		{
			if (scroll == null || killer == null) //sanity
			{
				return;
			}

			killer.SendLocalizedMessage(scroll is ScrollOfTranscendence ? 1094936 : 1049524); 
			
			if (killer.Alive)
			{
				killer.AddToBackpack(scroll);
			}
			else
			{
				if (killer.Corpse != null && !killer.Corpse.Deleted)
				{
					killer.Corpse.DropItem(scroll);
				}
				else
				{
					killer.AddToBackpack(scroll);
				}
			}

			// Justice reward
			var pm = (PlayerMobile)killer;
			foreach (var protector in pm.JusticeProtectors)
			{
				if (protector.Map != killer.Map || protector.Murderer || protector.Criminal ||
				    !JusticeVirtue.CheckMapRegion(killer, protector))
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
					try
					{
						protector.SendLocalizedMessage(1049368); // You have been rewarded for your dedication to Justice!

						if (Activator.CreateInstance(scroll.GetType()) is SpecialScroll scrollDupe)
						{
							scrollDupe.Skill = scroll.Skill;
							scrollDupe.Value = scroll.Value;
							protector.AddToBackpack(scrollDupe);
						}
					}
					catch (Exception e)
					{
						ExceptionLogging.LogException(e);
					}
				}
			}
		}

		private void Respawn()
		{
			if (!InternalActive || Deleted || Champion != null)
			{
				return;
			}

			var currentLevel = Level;
			var currentRank = Rank;
			var maxSpawn = (int)(MaxKills * 0.5d * SpawnMod);
			if (currentLevel >= 16)
			{
				maxSpawn = Math.Min(maxSpawn, MaxKills - InternalKills);
			}

			if (maxSpawn < 3)
			{
				maxSpawn = 3;
			}

			var spawnRadius = (int)(SpawnRadius * ChampionSystem.SpawnRadiusModForLevel(Level));

			var spawnBounds = new Rectangle2D(new Point2D(X - spawnRadius, Y - spawnRadius),
				new Point2D(X + spawnRadius, Y + spawnRadius));

			var mobCount = 0;
			foreach (var m in Creatures)
			{
				if (GetRankFor(m) == currentRank)
				{
					++mobCount;
				}
			}

			while (mobCount <= maxSpawn)
			{
				var m = Spawn();

				if (m == null)
				{
					return;
				}

				var loc = GetSpawnLocation(spawnBounds, SpawnRadius);
				
				m.OnBeforeSpawn(loc, Map);

				Creatures.Add(m);
				m.MoveToWorld(loc, Map);
				++mobCount;

				if (m is BaseCreature baseCreature)
				{
					baseCreature.Tamable = false;
					baseCreature.IsChampionSpawn = true;

					if (!ConfinedRoaming)
					{
						baseCreature.Home = Location;
						baseCreature.RangeHome = SpawnRadius;
					}
					else
					{
						baseCreature.Home = baseCreature.Location;

						var xWall1 = new Point2D(spawnBounds.X, baseCreature.Y);
						var xWall2 = new Point2D(spawnBounds.X + spawnBounds.Width, baseCreature.Y);
						var yWall1 = new Point2D(baseCreature.X, spawnBounds.Y);
						var yWall2 = new Point2D(baseCreature.X, spawnBounds.Y + spawnBounds.Height);

						var minXDist = Math.Min(baseCreature.GetDistanceToSqrt(xWall1), baseCreature.GetDistanceToSqrt(xWall2));
						var minYDist = Math.Min(baseCreature.GetDistanceToSqrt(yWall1), baseCreature.GetDistanceToSqrt(yWall2));

						baseCreature.RangeHome = (int)Math.Min(minXDist, minYDist);
					}
				}
			}
		}

		private Point3D GetSpawnLocation(Rectangle2D rect, int range)
		{
			var map = Map;

			if (map == null)
			{
				return Location;
			}

			// Try 20 times to find a spawnable location.
			for (var i = 0; i < 20; i++)
			{
				var dx = Utility.Random(range * 2);
				var dy = Utility.Random(range * 2);
				var x = rect.X + dx;
				var y = rect.Y + dy;

				var z = Map.GetAverageZ(x, y);

				if (Map.CanSpawnMobile(new Point2D(x, y), z))
				{
					return new Point3D(x, y, z);
				}

				/* try @ platform Z if map z fails */
				if (Map.CanSpawnMobile(new Point2D(x, y), Platform.Location.Z))
				{
					return new Point3D(x, y, Platform.Location.Z);
				}
			}

			return Location;
		}

		private Mobile Spawn()
		{
			var types = GetSpawnTypes();

			var rank = Rank;

			if (rank >= 0 && rank < types.Length)
			{
				return Spawn(types[rank]);
			}

			return null;
		}

		private Mobile Spawn(params Type[] types)
		{
			try
			{
				return Activator.CreateInstance(types[Utility.Random(types.Length)]) as Mobile;
			}
			catch (Exception e)
			{
				ExceptionLogging.LogException(e);
				return null;
			}
		}

		private void Expire()
		{
			InternalKills = 0;

			if (InternalWhiteSkulls.Count == 0)
			{
				// They didn't even get 20%, go back a level
				if (Level > StartLevel)
				{
					--Level;
				}

				InvalidateProperties();
			}
			else
			{
				SetWhiteSkullCount(0);
			}

			ExpireTime = DateTime.UtcNow + ExpireDelay;
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.SendGump(new PropertiesGump(from, this));
		}

		public override void OnLocationChange(Point3D oldLoc)
		{
			if (Deleted)
			{
				return;
			}

			if (Platform != null)
			{
				Platform.Location = new Point3D(X, Y, Z - 20);
			}

			if (Altar != null)
			{
				Altar.Location = new Point3D(X, Y, Z - 15);
			}

			if (Idol != null)
			{
				Idol.Location = new Point3D(X, Y, Z - 15);
			}

			if (InternalRedSkulls != null)
			{
				for (var i = 0; i < InternalRedSkulls.Count; ++i)
				{
					InternalRedSkulls[i].Location = GetRedSkullLocation(i);
				}
			}

			if (InternalWhiteSkulls != null)
			{
				for (var i = 0; i < InternalWhiteSkulls.Count; ++i)
				{
					InternalWhiteSkulls[i].Location = GetWhiteSkullLocation(i);
				}
			}

			_SpawnArea.X += Location.X - oldLoc.X;
			_SpawnArea.Y += Location.Y - oldLoc.Y;

			UpdateRegion();
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (InternalActive)
			{
				list.Add(1060742);
				list.Add(1060658, "Type\t{0}", InternalType);
				list.Add(1060659, "Level\t{0}", Level);
				list.Add(1060660, "Kills\t{0} of {1} ({2:F1}%)", InternalKills, MaxKills,
					100.0 * ((double)InternalKills / MaxKills));
			}
			else
			{
				list.Add(1060743);
			}
		}

		public virtual void SendGump(Mobile mobile)
		{
			mobile.SendGump(new SuperChampionSpawnInfoGump<T>(this));
		}

		public override void OnMapChange()
		{
			if (Deleted)
			{
				return;
			}

			if (Platform != null)
			{
				Platform.Map = Map;
			}

			if (Altar != null)
			{
				Altar.Map = Map;
			}

			if (Idol != null)
			{
				Idol.Map = Map;
			}

			if (InternalRedSkulls != null)
			{
				foreach (var item in InternalRedSkulls)
				{
					item.Map = Map;
				}
			}

			if (InternalWhiteSkulls != null)
			{
				foreach (var item in InternalWhiteSkulls)
				{
					item.Map = Map;
				}
			}

			UpdateRegion();
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if (Platform != null)
			{
				Platform.Delete();
			}

			if (Altar != null)
			{
				Altar.Delete();
			}

			if (Idol != null)
			{
				Idol.Delete();
			}

			RemoveSkulls();

			if (Creatures != null)
			{
				foreach (var mob in Creatures.Where(mob => !mob.Player))
				{
					mob.Delete();
				}

				Creatures.Clear();
			}

			if (Champion != null && !Champion.Player)
			{
				Champion.Delete();
			}

			Stop();

			UpdateRegion();
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			list.Add("champion spawn");
		}


		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(8);

			writer.Write(StartLevel);

			writer.Write(KillsMod);
			writer.Write(GroupName);

			writer.Write(SpawnName);
			writer.Write(AutoRestart);
			writer.Write(SpawnMod);
			writer.Write(SpawnRadius);

			writer.Write(DamageEntries.Count);
			foreach (var kvp in DamageEntries)
			{
				writer.Write(kvp.Key);
				writer.Write(kvp.Value);
			}

			writer.Write(ConfinedRoaming);
			writer.Write(Idol);
			writer.Write(HasBeenAdvanced);
			writer.Write(_SpawnArea);

			writer.Write(RandomizeType);

			writer.Write(InternalKills);

			writer.Write(InternalActive);
			writer.Write(InternalType);
			writer.Write(Creatures, true);
			writer.Write(InternalRedSkulls, true);
			writer.Write(InternalWhiteSkulls, true);
			writer.Write(Platform);
			writer.Write(Altar);
			writer.Write(ExpireDelay);
			writer.WriteDeltaTime(ExpireTime);
			writer.Write(Champion);
			writer.Write(RestartDelay);

			var restarting = RestartTimerRunning;
			writer.Write(restarting);

			if (restarting)
			{
				writer.WriteDeltaTime(RestartTime);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			DamageEntries = new Dictionary<Mobile, int>();

			var version = reader.ReadInt();

			switch (version)
			{
				case 8:
					StartLevel = reader.ReadInt();
					goto case 7;
				case 7:
					KillsMod = reader.ReadDouble();
					GroupName = reader.ReadString();
					goto case 6;
				case 6:
					SpawnName = reader.ReadString();
					AutoRestart = reader.ReadBool();
					SpawnMod = reader.ReadDouble();
					SpawnRadius = reader.ReadInt();
					goto case 5;
				case 5:
				{
					var entries = reader.ReadInt();
					for (var i = 0; i < entries; ++i)
					{
						var mobile = reader.ReadMobile();
						var damage = reader.ReadInt();

						if (mobile == null)
						{
							continue;
						}

						DamageEntries.Add(mobile, damage);
					}

					goto case 4;
				}
				case 4:
				{
					ConfinedRoaming = reader.ReadBool();
					Idol = reader.ReadItem() as SuperIdolOfTheChampion<T>;
					HasBeenAdvanced = reader.ReadBool();

					goto case 3;
				}
				case 3:
				{
					_SpawnArea = reader.ReadRect2D();

					goto case 2;
				}
				case 2:
				{
					RandomizeType = reader.ReadBool();

					goto case 1;
				}
				case 1:
				{
					InternalKills = reader.ReadInt();

					goto case 0;
				}
				case 0:
				{
					var active = reader.ReadBool();
					InternalType = (T)reader.ReadEnum();
					Creatures = reader.ReadStrongMobileList();
					InternalRedSkulls = reader.ReadStrongItemList();
					InternalWhiteSkulls = reader.ReadStrongItemList();
					Platform = reader.ReadItem() as SuperChampionPlatform<T>;
					Altar = reader.ReadItem() as SuperChampionAltar<T>;
					ExpireDelay = reader.ReadTimeSpan();
					ExpireTime = reader.ReadDeltaTime();
					Champion = reader.ReadMobile();
					RestartDelay = reader.ReadTimeSpan();

					if (reader.ReadBool())
					{
						RestartTime = reader.ReadDeltaTime();
						BeginRestart(RestartTime - DateTime.UtcNow);
					}

					if (Platform == null || Altar == null || Idol == null)
					{
						Delete();
					}
					else if (active)
					{
						Start(true);
					}

					break;
				}
			}

			foreach (var bc in Creatures.OfType<BaseCreature>())
			{
				bc.IsChampionSpawn = true;
			}

			Server.Timer.DelayCall(TimeSpan.Zero, UpdateRegion);
		}

		protected abstract Mobile GenerateChampion();
		protected abstract Type[][] GetSpawnTypes();
		protected abstract T GetRandomType();
		protected abstract void HandleChampionEvents(Mobile champion);
	}
}
