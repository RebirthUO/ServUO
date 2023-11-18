// **********
// RebirthUO - CustomModuleAttribute.cs
// **********

using System;

namespace Server.RebirthUO.Modules.CustomModuleMarker
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct |
	                AttributeTargets.Property | AttributeTargets.Method |
	                AttributeTargets.Interface | AttributeTargets.Enum)]
	public class CustomModuleLinkAttribute : Attribute
	{
		public CustomModule Module { get; }

		public CustomModuleLinkAttribute(CustomModule module)
		{
			Module = module;
		}
	}
}
