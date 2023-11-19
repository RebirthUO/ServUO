// **********
// ServUO - SuperChampionSkullHelper.cs
// **********

using System;
using System.Linq;

namespace Server.RebirthUO.Modules.CustomChampionSystem.Helper
{
	public static class SuperChampionSkullHelper
	{
		public static T GetSkullType<T>()
		{
			var array = Enum.GetValues(typeof(T))
				.Cast<T>()
				.ToArray();

			return array[Utility.Random(array.Length)];
		}

		public static T GetSkullType<T>(Func<T, bool> whereFilter)
		{
			var array = Enum.GetValues(typeof(T))
				.Cast<T>()
				.Where(whereFilter)
				.ToArray();

			return array[Utility.Random(array.Length)];
		}
	}
}
