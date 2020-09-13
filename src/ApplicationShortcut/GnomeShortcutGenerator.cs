using System;
using System.IO;
using System.Text;

namespace ApplicationShortcut
{
	internal class GnomeShortcutGenerator : IShortcutGenerator
	{
		public void Generate(ShortcutRequest shortcutRequest)
		{
			var shortcutPath = Path.Combine(
				shortcutRequest.ShortcutDirectoryPath, 
				$"{shortcutRequest.ShortcutName.ToLower()}.desktop");
			
			if (File.Exists(shortcutPath))
				throw new InvalidOperationException("Shortcut already exists.");
			
			var shortcutBuilder = new StringBuilder();
			shortcutBuilder.AppendLine("[Desktop Entry]");
			shortcutBuilder.AppendLine($"Name={shortcutRequest.ShortcutName}");
			shortcutBuilder.AppendLine($"Exec={shortcutRequest.SourceFullPath}");
			shortcutBuilder.AppendLine("Type=Application");
			shortcutBuilder.AppendLine("Icon=");
			shortcutBuilder.AppendLine("Comment=");
			shortcutBuilder.AppendLine("Terminal=false");

			File.WriteAllText(shortcutPath, shortcutBuilder.ToString());
		}
	}
}