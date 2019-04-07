using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace UnityClient.Editor {
	public sealed class BuildUtility {
		public string ProjectPath { get; }
		public string AssetPath   { get; }

		public BuildUtility(string projectPath, string assetPath) {
			ProjectPath = projectPath;
			AssetPath   = assetPath;
		}

		public void BuildForConfiguration(string name) {
			var curDir  = Directory.GetCurrentDirectory();
			var parent  = Directory.GetParent(curDir);
			var path    = Path.Combine(parent.FullName, ProjectPath);
			var outPath = $"{curDir}/{AssetPath}";

			if ( Run($"clean \"{path}\" -o \"{outPath}\"") && Run($"build \"{path}\" -c {name} -o \"{outPath}\"") ) {
				AssetDatabase.Refresh();
			}
		}

		static bool Run(string command) {
			// Hack, because %PATH% inside Unity may be different on MacOS
			var oldPath = Environment.GetEnvironmentVariable("PATH");
			Environment.SetEnvironmentVariable("PATH", oldPath + ":/usr/local/share/dotnet/");

			var startInfo = new ProcessStartInfo {
				FileName               = "dotnet",
				Arguments              = command,
				RedirectStandardError  = true,
				RedirectStandardOutput = true,
				UseShellExecute        = false,
				CreateNoWindow         = true
			};

			try {
				using ( var process = Process.Start(startInfo) ) {
					process.WaitForExit();
					var output = process.StandardOutput.ReadToEnd() + "\n" + process.StandardError.ReadToEnd();
					if ( process.ExitCode == 0 ) {
						Debug.Log($"Success: '{command}'\n{output}");
						return true;
					} else {
						Debug.LogError($"Failed: '{command}'\n{output}");
					}
				}
			} catch ( Exception e ) {
				Debug.LogError($"Failed: '{command}': {e}");
			}
			return false;
		}
	}
}
