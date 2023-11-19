// **********
// ServUO - CraftChampionRegionEvent.cs
// **********

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Server.Mobiles;
using Server.RebirthUO.Modules.CraftChampionSystem.Regions;
using Server.Spells.Necromancy;

namespace Server.RebirthUO.Modules.CraftChampionSystem.Config
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public static class CraftChampionRegionEventConfig
	{
		public static void Initialize()
		{
			EventSink.Logout += OnLogout;
			EventSink.Login += OnLogin;
		}

		private static void OnLogin(LoginEventArgs e)
		{
			var m = e.Mobile;

			if (m is PlayerMobile && !m.Alive && (m.Corpse == null || m.Corpse.Deleted) &&
			    m.Region.IsPartOf<CraftChampionSpawnRegion>() && m.Map == Map.Felucca)
			{
				var map = m.Map;
				var loc = ExorcismSpell.GetNearestShrine(m, ref map);

				if (loc != Point3D.Zero)
				{
					m.MoveToWorld(loc, map);
				}
				else
				{
					m.MoveToWorld(new Point3D(989, 520, -50), Map.Malas);
				}
			}
		}

		private static void OnLogout(LogoutEventArgs e)
		{
			var m = e.Mobile;

			if (m is PlayerMobile && m.Region.IsPartOf<CraftChampionSpawnRegion>() &&
			    m.AccessLevel < AccessLevel.Counselor && m.Map == Map.Felucca)
			{
				if (m.Alive && m.Backpack != null)
				{
					var list = new List<Item>(m.Backpack.Items.Where(i => i.LootType == LootType.Cursed));

					foreach (var item in list)
					{
						item.MoveToWorld(m.Location, m.Map);
					}

					ColUtility.Free(list);
				}

				Timer.DelayCall(TimeSpan.FromMilliseconds(250), () =>
				{
					var map = m.LogoutMap;

					var loc = ExorcismSpell.GetNearestShrine(m, ref map);

					if (loc != Point3D.Zero)
					{
						m.LogoutLocation = loc;
						m.LogoutMap = map;
					}
					else
					{
						m.LogoutLocation = new Point3D(989, 520, -50);
						m.LogoutMap = Map.Malas;
					}
				});
			}
		}
	}
}
