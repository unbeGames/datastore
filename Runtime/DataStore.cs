using System;
using System.IO;
using UnityEngine;

namespace Unbegames.Services {
	public class DataStore {
		public static event EventHandler OnWriteFailure;
		public static event EventHandler OnReadFailure;

		public bool noWritePermissions = false;

		public DataStore(){
			CreateDirs();
		}

		public StreamWriter CreateStreamWriter(string fileName) {
			StreamWriter writer = null;
			try {
				writer = File.CreateText(Path.Combine(Locations.DataDir(), fileName));
				writer.AutoFlush = true;
			} catch (Exception e) {
				Helpers.Exception("Can not create streamWirter", e);
			}
			return writer;
		}

		public void CreateDirs(){
			CreateDir(Locations.DataDir());
			CreateDir(Locations.SaveDir());
			CreateDir(Locations.LocalModsDir());			
		}

		public void WriteAllText(string path, string text){
			try {
        File.WriteAllText(path, text);
      } catch (Exception e) {
        Helpers.Exception("Can not write file, because of", e);
				OnWriteFailure?.Invoke();
      }
		}

		public void Rename(string originalPath, string newPath) {
			try {
				File.Move(originalPath, newPath);
			} catch (Exception e) {
				Helpers.Exception($"Can not write file {newPath}, because of", e);				
			}
		}

		public void Delete(string path){
			try {
        File.Delete(path);
      } catch (Exception e) {
        Helpers.Exception("Can not delete file, because of", e);				
      }
		}

		public void DeleteDirectory(string path, bool recursive = false) {
			try {
				Directory.Delete(path, recursive);
			} catch (Exception e) {
				Helpers.Exception("Can not delete file, because of", e);
			}
		}


		public void WriteAllBytes(string path, byte[] bytes){
			try {
        File.WriteAllBytes(path, bytes);
      } catch (Exception e) {
        Helpers.Exception("Can not write file, because of", e);
				OnWriteFailure?.Invoke();
      }
		}

		public void WriteMemoryStream(string path, MemoryStream ms){
			using(FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write)){
				ms.WriteTo(file);
			}
		}

		public string ReadAllText(string path){
			string text = null;
			try {
				var fileInfo = new FileInfo(path);
				if(fileInfo.Exists){
					text = File.ReadAllText(path);
				}
			} catch (Exception e){
				Helpers.Error(string.Format($"File read failed: {path}"));
				Debug.LogException(e);
				OnReadFailure?.Invoke(); 
			}
			return text;
		}

		public static bool DirectoryExists(string path) {
			return new DirectoryInfo(path).Exists;
		}

		public void CreateDir(string modRoot) {
			try {
				var dirInfo = new DirectoryInfo(modRoot);
				if (!dirInfo.Exists) {
					dirInfo.Create();
				}
			} catch (UnauthorizedAccessException e) {
				noWritePermissions = true;
				OnWriteFailure?.Invoke();
				Helpers.Exception("Can not create directories", e);
			}
		}

		public void ReadLines(string path, Func<string, bool> callback){		
			try {
				var fileInfo = new FileInfo(path);
				if(fileInfo.Exists){
					string line;
					using (StreamReader file = new StreamReader(path)) {
						while ((line = file.ReadLine()) != null) {
							var shouldInterrupt = callback(line);
							if (shouldInterrupt) {
								break;
							}
						}
					}					
				}
			} catch (Exception e){
				Helpers.Error(string.Format($"File read failed: {path}"));
				Debug.LogException(e);
				OnReadFailure?.Invoke(); 
			}
		}

		public static FileInfo[] GetFiles(string path, string extension) {
			var dir = new DirectoryInfo(path);
			FileInfo[] result;
			if (dir.Exists) {
				result = dir.GetFiles($"*.{extension}");
			} else {
				Helpers.Error($"Directory {path} not found");
				result = Array.Empty<FileInfo>();
			}
			return result;
		}

		public byte[] ReadAllBytes(string path){
			byte[] bytes = null;
			try {
				var fileInfo = new FileInfo(path);
				if(fileInfo.Exists){
					bytes = File.ReadAllBytes(path);
				}
			} catch (Exception e){
				Helpers.Error(string.Format($"File read failed: {path}"));
				Debug.LogException(e);
				OnReadFailure?.Invoke(); 
			}
			return bytes;
		}
	}
}
