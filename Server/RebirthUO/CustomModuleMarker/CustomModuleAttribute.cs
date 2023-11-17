// **********
// RebirthUO - CustomModuleAttribute.cs
// **********

namespace Server.RebirthUO.CustomModuleMarker
{
	[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct |
	                       System.AttributeTargets.Property | System.AttributeTargets.Method |
	                       System.AttributeTargets.Interface)]
	public class CustomModuleAttribute : System.Attribute
	{
		public CustomModule Module { get; }

		public CustomModuleAttribute(CustomModule module)
		{
			Module = module;
		}
	}
}
