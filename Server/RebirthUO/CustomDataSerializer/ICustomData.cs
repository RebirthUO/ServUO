// **********
// ServUO - ICustomData.cs
// **********

using Server.RebirthUO.CustomModuleMarker;

namespace Server.RebirthUO.CustomDataSerializer
{
	[CustomModule(CustomModule.Serialization)]
	public interface ICustomData
	{
		void WriteCustomData(GenericWriter writer);
		void ReadCustomData(GenericReader reader);
	}
}
