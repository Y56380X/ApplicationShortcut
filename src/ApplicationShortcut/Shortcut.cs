using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
	
	internal interface IShortcutGenerator
	{
		void Generate(ShortcutRequest shortcutRequest);
	}

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
	
	public static class Shortcut
	{
		private static IShortcutGenerator CreateShortcutGenerator() => Environment.OSVersion.Platform switch
		{
			PlatformID.Unix => new GnomeShortcutGenerator(),
			PlatformID.Win32NT => throw new NotImplementedException(),
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