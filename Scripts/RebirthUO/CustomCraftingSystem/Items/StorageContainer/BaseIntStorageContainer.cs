// **********
// ServUO - BaseIntStorageContainer.cs
// **********

using System.Diagnostics.CodeAnalysis;

namespace Server.RebirthUO.CustomCraftingSystem.Items.StorageContainer
{
	[SuppressMessage("ReSharper", "PublicConstructorInAbstractClass")]
	public abstract class BaseIntStorageContainer<T> : BaseStorageContainer<T, int>
	{
		public BaseIntStorageContainer(int itemId) : base(itemId)
		{
		}

		public BaseIntStorageContainer(Serial serial) : base(serial)
		{
		}
	}
}
