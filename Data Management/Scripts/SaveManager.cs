using System.IO;
using UnityEngine;

public static class SaveManager
{	
	static string dataPath{
		get{ return Application.persistentDataPath + "/Saves/Users Data" + "/"; }
	}
	
	const bool prettyPrint = true;
	
	#region Directory
	
		public static string CreateDirectory(string newDirectory){
			string path = dataPath + newDirectory;
				
			if(!Directory.Exists(path)) // should do overwriting
				Directory.CreateDirectory(path);
					
			return path;
		}
		
		static string ConvertToDirectory(string[] newDirectory){
			string directory = "";
				
			foreach(var d in newDirectory)
				directory += (d + "/");
				
			return directory;
		}
		
	#endregion
	
	#region Save
	
		public static void Save<T>(string fileName, T data){
			string path = dataPath + fileName + ".json";
			string newJson = JsonUtility.ToJson(data, prettyPrint);
			
			File.WriteAllText(path, newJson);
		}
			
		// Overload for Save but with Directory
			public static void Save<T>(
				string[] directory, // STRING ARRAY
				string fileName,
				T data
			){
				// check if username exist
					string username = directory[0];
					if(!Directory.Exists(dataPath + username)) return;
				
				string _directory = CreateDirectory(ConvertToDirectory(directory)); // thus recreating new directory with existing directory replaces old files in it?
				string path = _directory + fileName + ".json";
				string newJson = JsonUtility.ToJson(data, prettyPrint);	
				
				File.WriteAllText(path, newJson);
			}
	
	#endregion
	
	#region Load
	
		public static T Load<T>(string rawPath){
			string path = dataPath + rawPath + ".json";
			
			if(File.Exists(path)){
				string json = File.ReadAllText(path);
				return JsonUtility.FromJson<T>(json);
			}
			else return default(T);
		}
		
		public static T Load<T>(string[] directory, string fileName){
			string path = dataPath + ConvertToDirectory(directory) + fileName + ".json";
			
			if(File.Exists(path)){
				string json = File.ReadAllText(path);
				return JsonUtility.FromJson<T>(json);
			}
			else{
				Debug.LogWarning("NOT FOUND! " + path);
				return default(T);
			}
		}
	
	#endregion
	
	#region Edit
	
		public static void DeleteUser(string username){
			string directory = dataPath + username;
			
			if(Directory.Exists(directory))
				Directory.Delete(directory, true); // bool recursive = true;
		}
		
		public static void RenameUser(string oldName, string newName){
			string oldDirectory = dataPath + oldName;
			
			if(Directory.Exists(oldDirectory)){
				string newDirectory = dataPath + newName;
				Directory.Move(oldDirectory, newDirectory);
			}
		}
	
	#endregion
}