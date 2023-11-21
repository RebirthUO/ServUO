// **********
// ServUO - ContainerStorageGumpLink.cs
// **********

namespace Server.RebirthUO.CustomCraftingSystem.Items.StorageContainer
{
	public class ContainerStorageGumpLink<T1, T2>
	{
		public ContainerStorageGumpLink(string groupKey, T1 entryKey, T2 value)
		{
			GroupKey = groupKey;
			EntryKey = entryKey;
			Value = value;
		}

		public string GroupKey { get; }
		public T1 EntryKey { get; }
		public T2 Value { get; }
	}
}
