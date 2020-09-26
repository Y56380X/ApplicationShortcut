/*
	MIT License

	Copyright (c) 2020 Y56380X

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
*/

using System;
using System.IO;
using System.Text;
using Mono.Unix;

namespace ApplicationShortcut
{
	public static class ExceptionData
	{
		public const string ShortcutPath = "shortcut_path";
	}
	
	internal class GnomeShortcutGenerator : IShortcutGenerator
	{
		public void Generate(ShortcutRequest shortcutRequest)
		{
			var shortcutName = shortcutRequest.ShortcutName.ToLower().Replace(" ", "-");
			var shortcutPath = Path.Combine(
				shortcutRequest.ShortcutDirectoryPath, 
				$"{shortcutName}.desktop");

			if (File.Exists(shortcutPath))
			{
				var exception = new InvalidOperationException("Shortcut already exists.");
				exception.Data[ExceptionData.ShortcutPath] = shortcutPath;
				throw exception;
			}

			var shortcutBuilder = new StringBuilder();
			shortcutBuilder.AppendLine("[Desktop Entry]");
			shortcutBuilder.AppendLine($"Name={shortcutRequest.ShortcutName}");
			shortcutBuilder.AppendLine($"Exec={shortcutRequest.SourceFullPath}");
			shortcutBuilder.AppendLine($"Path={shortcutRequest.WorkingDirectory}");
			shortcutBuilder.AppendLine("Type=Application");
			shortcutBuilder.AppendLine($"Icon={shortcutRequest.IconPath}");
			shortcutBuilder.AppendLine("Comment=");
			shortcutBuilder.AppendLine("Terminal=false");

			File.WriteAllText(shortcutPath, shortcutBuilder.ToString());

			UnixFileSystemInfo.GetFileSystemEntry(shortcutPath).FileAccessPermissions |= FileAccessPermissions.UserExecute;
		}
	}
}