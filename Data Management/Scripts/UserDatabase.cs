using System.Collections.Generic;
using System.Linq;

// saving: only string names (UserDatabaseWrapper)
// loading: original user data (PlayerData)

[System.Serializable] public class UserDatabaseWrapper
{
	public int currentUserIndex;
	public string[] users;
	
	// Save
	public UserDatabaseWrapper(UserDatabase rawData){
		currentUserIndex = rawData.currentUserIndex;
		
		int count = rawData.users.Count;
		users = new string[count];
		
		for(int i = 0; i < count; i++)
			users[i] = rawData.users[i].name;
		
		SaveManager.Save(UserDatabase.dataFileName, this);
	}
	
	// Load
	public PlayerData[] LoadUserData(){
		int length = users.Length;
		var output = new PlayerData[length];
		
		for(int i = 0; i < length; i++){
			var username = users[i];
			var directory = new string[]{ username };
			
			var load = SaveManager.Load<PlayerData>(directory, username);
			output[i] = (load != null)? load: new PlayerData();
		}
		
		return output;
	}
}

[System.Serializable] public class UserDatabase
{
	#region Serializables
	
		public int currentUserIndex;		
		public List<PlayerData> users = new List<PlayerData>();
	
	#endregion
	
	#region Constructor
		
		public const string dataFileName = "userDatabase";
		
		public UserDatabase(){}
		
		public UserDatabase(UserDatabaseWrapper wrapperData){
			currentUserIndex = wrapperData.currentUserIndex;
			users = wrapperData.LoadUserData().ToList();
		}
		
		#region SaveManager
		
			public void Save(){
				new UserDatabaseWrapper(this); // unreadable hard save
				SelectUser(maxUserIndex);
			}
			
			public static UserDatabase Load(){
				var load = SaveManager.Load<UserDatabaseWrapper>(dataFileName);
				
				return (load == null)?
					new UserDatabase():
					new UserDatabase(load);
			}
		
		#endregion
	
	#endregion
	
	#region Properties
	
		const int minUserIndex = 0;
		
		public int maxUserIndex{
			get{
				int arrayOffset = 1;
				return users.Count - arrayOffset;
			}
		}
		
		public int CurrentUserIndex{ // Clamped into maxUserIndex
			get{
				if(
					currentUserIndex < minUserIndex ||
					currentUserIndex > maxUserIndex
				){
					currentUserIndex = UnityEngine.Mathf.Clamp(
						currentUserIndex,
						minUserIndex,
						maxUserIndex
					);
					
					new UserDatabaseWrapper(this); // unreadable hard save
				}
					
				return currentUserIndex;
			}
			set{
				int newValue = value;
				
				if(
					newValue < minUserIndex ||
					newValue > maxUserIndex
				){
					newValue = UnityEngine.Mathf.Clamp(
						newValue,
						minUserIndex,
						maxUserIndex
					);
				}
				
				currentUserIndex = newValue;
				new UserDatabaseWrapper(this); // unreadable hard save
			}
		}
	
	#endregion
		
	public void Add(PlayerData newUser){
		// add the newUser
		// select the newUser
		
		if(!users.Contains(newUser)) // should have an overwriting system for this
			users.Add(newUser);
			
		CurrentUserIndex = maxUserIndex;
	}
	
	public PlayerData CurrentUser(){
		if(users.Count == 0) return null;
		return users[CurrentUserIndex];
	}
	
	public PlayerData GetUser(int userIndex){
		if(users.Count == 0) return null;
		return users[userIndex];
	}
		
		public string[] GetAllUserNames(){
			int count = users.Count;
			var names = new string[count];
			
			for(int i = 0; i < count; i++)
				names[i] = users[i].name;
			
			return names;
		}
	
	public void SelectUser(int userIndex){
		if(users.Count == 0) return;
		CurrentUserIndex = userIndex;
	}
	
	public void RenameUser(int userIndex, string newName){
		// things to happen
			// soft rename from userDatabase
			// rename the corresponding directory
			// update the user database file from save manager
		
		var usernames = GetAllUserNames().ToList();
		if(usernames.Contains(newName)) return; // should do errors or overwriting system for this1
		
		var oldName = users[userIndex].name; // cache the oldName first ofc
		users[userIndex].name = newName;
		
		SaveManager.RenameUser(oldName, newName);
		SaveManager.Save(dataFileName, this);
	}
	
	public void DeleteUser(int userIndex){
		// things to happen
			// soft delete from user database
			// hard delete from save manager
			// update user database file from save manager		
				
		var user = GetUser(userIndex);
			users.Remove(user); // users.RemoveAt(userIndex);
			
		SaveManager.DeleteUser(user.name);
		
		// update "currentUserIndex"  to avoid "out-of-range error"
			var updateCurrentUserIndex = CurrentUserIndex; // the "CurrentUserIndex" property has an automated range-clamping in it
			
		SaveManager.Save(dataFileName, this);
	}
}