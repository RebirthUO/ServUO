// **********
// ServUO - SuperRewardTimer.cs
// **********

using System;
using System.Linq;
using Server.Items;

namespace Server.RebirthUO.Modules.CustomChampionSystem.Timer
{
	public class SuperRewardTimer : Server.Timer
	{
		private int Count { get; set; }
		private int Limit { get; }
		private Map Map { get; }
		private Point3D Location { get; }
		private Func<Item> Generator { get; }

		public SuperRewardTimer(Point3D center, Map map, int limit, Func<Item> generator) : base(
			TimeSpan.FromSeconds(0.25d), TimeSpan.FromSeconds(0.25d))
		{
			Location = center;
			Map = map;
			Limit = limit;
			Generator = generator;
			Count = 0;
		}

		protected override void OnTick()
		{
			if (Count >= Limit)
			{
				Stop();
				return;
			}

			var location = FindNextLocation(Map, Location, Limit / 8);
			var item = Generator.Invoke();

			if (item != null)
			{
				item.MoveToWorld(location, Map);

				switch (Utility.Random(3))
				{
					case 0: // Fire column
						Effects.SendLocationParticles(
							EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration),
							0x3709, 10, 30, 5052);
						Effects.PlaySound(item, item.Map, 0x208);
						break;
					case 1: // Explosion
						Effects.SendLocationParticles(
							EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration),
							0x36BD, 20, 10, 5044);
						Effects.PlaySound(item, item.Map, 0x307);
						break;
					case 2: // Ball of fire
						Effects.SendLocationParticles(
							EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration),
							0x36FE, 10, 10, 5052);
						break;
				}
			}

			++Count;
		}

		private Point3D FindNextLocation(Map map, Point3D center, int range)
		{
			var cx = center.X;
			var cy = center.Y;

			for (var i = 0; i < 20; ++i)
			{
				var x = cx + Utility.Random(range * 2) - range;
				var y = cy + Utility.Random(range * 2) - range;
				if (((cx - x) * (cx - x)) + ((cy - y) * (cy - y)) > range * range)
				{
					continue;
				}

				var z = map.GetAverageZ(x, y);
				if (!map.CanFit(x, y, z, 6, false, false))
				{
					continue;
				}

				var topZ = map.GetItemsInRange(new Point3D(x, y, z), 0)
					.Select(item => item.Z + item.ItemData.CalcHeight).Prepend(z).Max();

				return new Point3D(x, y, topZ);
			}

			return center;
		}
	}
}
