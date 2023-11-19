// **********
// ServUO - SuperSacrificeTarget.cs
// **********

using System;
using Server.RebirthUO.Modules.CustomChampionSystem.Addons;
using Server.RebirthUO.Modules.CustomChampionSystem.Items;
using Server.Targeting;

namespace Server.RebirthUO.Modules.CustomChampionSystem.Targets
{
	public class SuperSacrificeTarget<T> : Target where T : Enum
	{
		private readonly SuperChampionSkullBrazier<T> _Brazier;

		public SuperSacrificeTarget(SuperChampionSkullBrazier<T> brazier)
			: base(12, false, TargetFlags.None)
		{
			_Brazier = brazier;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			_Brazier.EndSacrifice(from, targeted as SuperChampionSkull<T>);
		}
	}
}
