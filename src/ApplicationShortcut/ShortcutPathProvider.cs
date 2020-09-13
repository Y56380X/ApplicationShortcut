using System;
using System.IO;

namespace ApplicationShortcut
{
	internal static class ShortcutPathProvider
	{
		public static string PathForLauncher() => Environment.OSVersion.Platform switch
		{
			PlatformID.Unix => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "applications"),
			PlatformID.Win32NT => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\Start Menu\Programs"),
			_ => throw new NotSupportedException()
		};
	}
}