using System;
using System.IO;

namespace ApplicationShortcut
{
	public class WindowsShortcutGenerator : IShortcutGenerator
	{
		public void Generate(ShortcutRequest shortcutRequest)
		{
			var shortcutPath = Path.Combine(
				shortcutRequest.ShortcutDirectoryPath, 
				$"{shortcutRequest.ShortcutName}.lnk");
			
			if (File.Exists(shortcutPath))
				throw new InvalidOperationException("Shortcut already exists.");
			
			var shortcut = new CSharpLib.Shortcut();
			shortcut.CreateShortcutToFile(shortcutRequest.SourceFullPath, shortcutPath);
		}
	}
}