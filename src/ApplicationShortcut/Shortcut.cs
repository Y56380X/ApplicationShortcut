using System;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

namespace ApplicationShortcut
{
	public static class Shortcut
	{
		private static IShortcutGenerator CreateShortcutGenerator() => Environment.OSVersion.Platform switch
		{
			PlatformID.Unix => new GnomeShortcutGenerator(),
			PlatformID.Win32NT => new WindowsShortcutGenerator(),
			_ => throw new NotSupportedException()
		};
		
		public static void Create(ShortcutRequest createRequest)
		{
			var generator = CreateShortcutGenerator();
			generator.Generate(createRequest);
		}

		public static async Task CreateAsync(ShortcutRequest createRequest) => 
			await Task.Run(() => Create(createRequest));
	}
}