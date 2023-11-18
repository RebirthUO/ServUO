// **********
// ServUO - ICustomData.cs
// **********

using Server.RebirthUO.Modules.CustomModuleMarker;

namespace Server.RebirthUO.Modules.CustomDataSerializer
{
	[CustomModuleLink(CustomModule.Serialization)]
	public interface ICustomData
	{
		void WriteCustomData(GenericWriter writer);
		void ReadCustomData(GenericReader reader);
	}
}
