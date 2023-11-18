// **********
// ServUO - ColorHelper.cs
// **********

using System.Drawing;

namespace Server.RebirthUO.Helper
{
	public static class ColorHelper
	{
		public static string WrapNameWithHtmlColor(string input, Color color)
		{
			var hexCode = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
			return WrapNameWithHtmlColor(input, hexCode);
		}

		public static string WrapNameWithHtmlColor(string input, string color)
		{
			return $"<BASEFONT COLOR={color}>{input}<BASEFONT COLOR=#FFFFFF>";
		}
	}
}
