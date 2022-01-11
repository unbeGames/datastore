using System.IO;
using UnityEngine;

namespace Unbegames.Services {
	public static class Locations {
		public const string gameLogName = "Game.log";

		private const string resourcesFolderName = "Resources";
		private const string savesFolderName = "saves";
		private const string modsFolderName = "mods";

		private static string dataDir;		
		private static string saveDir;
		private static string localModsDir;
		private static string streamingAssetsDir;
		private static string resourcesDir;

		public static int SaveImageSize() {
			return Screen.height;
		}

		public static string StreamingAssetsDir() {
			return streamingAssetsDir ??= Application.streamingAssetsPath;
		}		
		
		public static string ResourcesDir() {
			return resourcesDir ??= Path.Combine(Application.dataPath, resourcesFolderName);
		}

		public static string DataDir(){
			return dataDir ??=  Application.persistentDataPath;
		}

		public static string SaveDir(){
			return saveDir ??=  Path.Combine(DataDir(), savesFolderName);
		}

		public static string LocalModsDir() {
			return localModsDir ??= Path.Combine(DataDir(), modsFolderName);
		}

		public static string SaveDir(int slot) {
			return Path.Combine(SaveDir(), slot.ToString());
		}
	}
}
