using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BuildNupack
{
	class Program
	{

		public static string DefaultConfiguration {
			get {
#if DEBUG
				return "Debug";
#else
				return "Release";
#endif
			}
		}

		public static string CurrentFolderPath {
			get { return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName); }
		}

		public static string GetNuGetPath(string workingDir) {
			return Path.Combine(workingDir, "nuget.exe");
		}

		static void Main(string[] args) {

			var isDebug = DefaultConfiguration == "Debug";

			var nuSpecPath = (null != args && args.Length > 0)
				? args[0]
				: Path.Combine(CurrentFolderPath,"..","..","..","vertesaur.nuspec"); // should be in the build folder
			var buildDir = Path.GetDirectoryName(nuSpecPath);
            if(String.IsNullOrWhiteSpace(buildDir))
                throw new DirectoryNotFoundException(String.Format("Build director is not found. NuSpecPath: '{0}'", nuSpecPath));

			var workingDir = Path.Combine(buildDir,"packed");
			if (!Directory.Exists(workingDir))
				Directory.CreateDirectory(workingDir);
			var targetAssemblyPath = Path.Combine(buildDir, "..", "bin", DefaultConfiguration, "net40-client", "Vertesaur.Core.dll");

			var targetAssemblyMetadata = Assembly.ReflectionOnlyLoadFrom(targetAssemblyPath);
			var targetAssemblyVersion = targetAssemblyMetadata.GetName().Version;
			var targetAssemblyVersionString = targetAssemblyVersion.ToString();
			var targetAssemblyCompany = targetAssemblyMetadata
				.GetCustomAttributesData()
				.Where(a => a.Constructor.DeclaringType == typeof (AssemblyCompanyAttribute))
				.SelectMany(a => a.ConstructorArguments)
				.Where(a => a.Value != null)
				.Select(a => a.Value.ToString())
				.First();

			var versionString = targetAssemblyVersionString;
			if (isDebug) {
				// force the version to the first 3 parts
				versionString = String.Join(".",
					versionString.Split(".".ToCharArray())
						.Take(2)
						.ToArray()
					);

				// combine the last two version numbers into a new one
				//var versionNumber = ((ushort)(targetAssemblyVersion.Build * 0xff / 65535.0) << 8) | (ushort)(targetAssemblyVersion.Revision * 0xff / 65535.0);
				var buildSeconds = (targetAssemblyVersion.Build * (60 * 60 * 24)) + targetAssemblyVersion.Revision - 402600000;
				var buildNumber = checked((int)(buildSeconds / 60.0));
				versionString += String.Concat('.', buildNumber);

				// add the semver stuff
				versionString += "-Dev-" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd-hhmmss");
			}

			var arguments = new Dictionary<string,string>{
				{"version", versionString },
				{"author", targetAssemblyCompany},
				{"configuration", DefaultConfiguration}
			};
			var argumentsString = String.Format(@"pack ""{0}"" -Properties {1}",
				nuSpecPath,
				String.Join(";", arguments.Select(a => a.Key + "=\"" + a.Value + "\"").ToArray()));

			var nuGetPackCommand = new Process {
				StartInfo = new ProcessStartInfo(GetNuGetPath(buildDir)) {
					Arguments = argumentsString,
					RedirectStandardOutput = true,
					UseShellExecute = false,
					WorkingDirectory = workingDir
				},
			};

			Console.WriteLine(nuGetPackCommand.StartInfo.FileName + " " + nuGetPackCommand.StartInfo.Arguments);
			nuGetPackCommand.Start();
			Console.WriteLine(nuGetPackCommand.StandardOutput.ReadToEnd());
			nuGetPackCommand.WaitForExit();
			
		}
	}
}
