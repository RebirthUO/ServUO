// **********
// ServUO - BaseIntStorageContainerGump.cs
// **********

namespace Server.RebirthUO.CustomCraftingSystem.Items.StorageContainer
{
	public class BaseIntStorageContainerGump<T> : BaseStorageContainerGump<T, int>
	{

		public BaseIntStorageContainerGump(string title, Mobile mobile, BaseIntStorageContainer<T> container) :
			base(title, mobile, container)
		{
		}

		protected override string ValueToString(int value)
		{
			return value.ToString();
		}
	}
}
