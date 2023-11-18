// **********
// RebirthUO - CustomModuleAttribute.cs
// **********

using System;

namespace Server.RebirthUO.CustomModuleMarker
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct |
	                AttributeTargets.Property | AttributeTargets.Method |
	                AttributeTargets.Interface | AttributeTargets.Enum)]
	public class CustomModuleAttribute : Attribute
	{
		public CustomModule Module { get; }

		public CustomModuleAttribute(CustomModule module)
		{
			Module = module;
		}
	}
}
