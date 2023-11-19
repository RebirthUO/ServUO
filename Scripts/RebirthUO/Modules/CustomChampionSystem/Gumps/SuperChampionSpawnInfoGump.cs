// **********
// ServUO - SuperChampionSpawnInfoGump.cs
// **********

using System;
using System.Linq;
using Server.Gumps;
using Server.Network;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;

namespace Server.RebirthUO.Modules.CustomChampionSystem.Gumps
{
	public class SuperChampionSpawnInfoGump<T> : Gump where T : Enum
	{
		private const int _Boarder = 20;
		private const int _RowHeight = 25;
		private const int _FontHue = 0;
		private readonly int[] _Widths = { 20, 160, 160, 20 };

		private SuperChampionSpawn<T> Spawn { get; }

		public SuperChampionSpawnInfoGump(SuperChampionSpawn<T> spawn)
			: base(40, 40)
		{
			var width = _Widths.Sum();
			var tabs = new int[_Widths.Length];

			var tabIndex = 0;
			for (var i = 0; i < _Widths.Length; ++i)
			{
				tabs[i] = tabIndex;
				tabIndex += _Widths[i];
			}

			Spawn = spawn;

			AddBackground(0, 0, width, (_Boarder * 2) + (_RowHeight * (8 + spawn.DamageEntries.Count)), 0x13BE);

			var top = _Boarder;
			AddLabel(_Boarder, top, _FontHue, "Champion Spawn Info Gump");
			top += _RowHeight;

			AddLabel(tabs[1], top, _FontHue, "Kills");
			AddLabel(tabs[2], top, _FontHue, spawn.Kills.ToString());
			top += _RowHeight;

			AddLabel(tabs[1], top, _FontHue, "Max Kills");
			AddLabel(tabs[2], top, _FontHue, spawn.MaxKills.ToString());
			top += _RowHeight;

			AddLabel(tabs[1], top, _FontHue, "Level");
			AddLabel(tabs[2], top, _FontHue, spawn.Level.ToString());
			top += _RowHeight;

			AddLabel(tabs[1], top, _FontHue, "Rank");
			AddLabel(tabs[2], top, _FontHue, spawn.Rank.ToString());
			top += _RowHeight;

			AddLabel(tabs[1], top, _FontHue, "Active");
			AddLabel(tabs[2], top, _FontHue, spawn.Active.ToString());
			top += _RowHeight;

			AddLabel(tabs[1], top, _FontHue, "Auto Restart");
			AddLabel(tabs[2], top, _FontHue, spawn.AutoRestart.ToString());
			top += _RowHeight;

			var damageEntries = spawn.DamageEntries.Keys.Select(mob => new DamageEntry(mob, spawn.DamageEntries[mob]))
				.ToList().OrderByDescending(x => x.Damage).ToList();

			foreach (var damageEntry in damageEntries)
			{
				AddLabelCropped(tabs[1], top, 100, _RowHeight, _FontHue, damageEntry.Mobile.RawName);
				AddLabelCropped(tabs[2], top, 80, _RowHeight, _FontHue, damageEntry.Damage.ToString());
				top += _RowHeight;
			}

			AddButton(width - (_Boarder + 30), top, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
			AddLabel(width - (_Boarder + 100), top, _FontHue, "Refresh");
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			switch (info.ButtonID)
			{
				case 1:
					Spawn.SendGump(sender.Mobile);
					break;
			}
		}

		private class DamageEntry
		{
			public readonly int Damage;
			public readonly Mobile Mobile;

			public DamageEntry(Mobile mob, int dmg)
			{
				Mobile = mob;
				Damage = dmg;
			}
		}
	}
}
