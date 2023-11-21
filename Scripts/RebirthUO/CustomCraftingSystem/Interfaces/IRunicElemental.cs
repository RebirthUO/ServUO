// **********
// ServUO - IRunicElemental.cs
// **********

using Server.Items;

namespace Server.RebirthUO.CustomCraftingSystem.Interfaces
{
	public interface IRunicElemental
	{
		CraftResource CraftResource { get; }
		double RunicDropChance { get; set; }
		void InitMobile();
	}
}
