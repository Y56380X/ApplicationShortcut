using System;

namespace ApplicationShortcut
{
	public class ShortcutRequest
	{
		public string ShortcutDirectoryPath { get; }
		public string ShortcutName { get; }
		public string SourceFullPath { get; }

		public ShortcutRequest(
			string shortcutName,
			string sourceFullPath)
		{
			ShortcutDirectoryPath = ShortcutPathProvider.PathForLauncher();
			ShortcutName = shortcutName;
			SourceFullPath = sourceFullPath;
		}
		
		public ShortcutRequest(
			Environment.SpecialFolder shortcutFolder,
			string shortcutName,
			string sourceFullPath)
		{
			ShortcutDirectoryPath = Environment.GetFolderPath(shortcutFolder);
			ShortcutName = shortcutName;
			SourceFullPath = sourceFullPath;
		}
		
		public ShortcutRequest(
			string shortcutDirectoryPath, 
			string shortcutName, 
			string sourceFullPath)
		{
			ShortcutDirectoryPath = shortcutDirectoryPath;
			ShortcutName = shortcutName;
			SourceFullPath = sourceFullPath;
		}
	}
}