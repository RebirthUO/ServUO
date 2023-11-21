// **********
// ServUO - ContainerStorageGroup.cs
// **********

using System.Collections.Generic;

namespace Server.RebirthUO.CustomCraftingSystem.Items.StorageContainer
{
	public class ContainerStorageGroup<T1,T2> 
	{
		public ContainerStorageGroup(string name, Dictionary<T1, T2> values)
		{
			Name = name;
			Values = values;
		}

		public string Name { get; }
		public Dictionary<T1, T2> Values { get; }
	}
}
