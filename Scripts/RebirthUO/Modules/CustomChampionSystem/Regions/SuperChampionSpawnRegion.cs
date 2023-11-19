// **********
// ServUO - SuperChampionSpawnRegion.cs
// **********

using System;
using Server.Mobiles;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;
using Server.Regions;

namespace Server.RebirthUO.Modules.CustomChampionSystem.Regions
{
	public abstract class SuperChampionSpawnRegion<T> : BaseRegion where T : Enum
	{
		public override bool YoungProtected => false;

		private SuperChampionSpawn<T> ChampionSpawn { get; }

		protected SuperChampionSpawnRegion(SuperChampionSpawn<T> spawn)
			: base(null, spawn.Map, Find(spawn.Location, spawn.Map), spawn.SpawnArea)
		{
			ChampionSpawn = spawn;
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return false;
		}

		public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
		{
			base.AlterLightLevel(m, ref global, ref personal);
			global = Math.Max(global, 1 + ChampionSpawn.Level);
		}

		public override bool OnMoveInto(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation)
		{
			if (m is PlayerMobile && !m.Alive && (m.Corpse == null || m.Corpse.Deleted) && Map == Map.Felucca)
			{
				return false;
			}

			return base.OnMoveInto(m, d, newLocation, oldLocation);
		}
	}
}
