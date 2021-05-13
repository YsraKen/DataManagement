using System; // Serializable
using UnityEngine;

public class DataManager : MonoBehaviour
{
	// Singleton
		public static DataManager instance{ get; private set; }
		void Awake(){ instance = this; }
	
	#region MainMenu Callbacks
	
		#region Properties
		
			UserDatabase _userDatabase;
			UserDatabase userDatabase{
				get{
					if(_userDatabase == null)
						_userDatabase = UserDatabase.Load();
					
					return _userDatabase;
				}
			}
		
			public int currentUserIndex{ // UserSelectScroll
				get{ return userDatabase.CurrentUserIndex; }
			}
			
		#endregion
		
		// calling same-name Methods ins UserDatabase class prevents null errors
		
		// UserSelectPanelUI
			public string[] GetAllUserNames(){
				return userDatabase.GetAllUserNames();
			}
		
		// StartPanelUI
			public PlayerData GetUser(){
				return userDatabase.CurrentUser();
			}
		
		// UserEditPanelUI
			public PlayerData GetUser(int userIndex){
				return userDatabase.GetUser(userIndex);
			}
			
			public void SelectUser(int userIndex){
				userDatabase.SelectUser(userIndex);
			}
		
		// UserEditPanelUI
			public void RenameUser(int userIndex, string newName){
				userDatabase.RenameUser(userIndex, newName);
			}
		
		// UserEditPanelUI
			public void DeleteUser(int userIndex){
				userDatabase.DeleteUser(userIndex);
			}
		
		// UserSelectScroll
			public int GetMaxUserIndex(){
				return userDatabase.maxUserIndex;
			}
		
		 // UerCreatePanelUI
			public void CreateUser(string newUsername){
				// Create data files
					var newUser = new PlayerData(newUsername);
					
				// Update userDatabase
					userDatabase.Add(newUser);
					userDatabase.Save();
			}
	
	#endregion

	#region LevelManager Callbacks
	
		#region PlayerProgress
		
			public PlayerProgress GetPlayerProgress(){
				var user = GetUser().name;
				var directory = new string[]{ user, PlayerProgress.directoryName };
				
				var dataWrapper = SaveManager.Load<PlayerProgressWrapper>(
					directory,
					PlayerProgress.dataFileName
				);
				
				var output = dataWrapper.LoadProgressData(user);
				
				if(output == null) Debug.LogWarning("no progressData loaded");
				// else Debug.Log("progressData has been loaded!");
				
				return output;
			}
			
			#region GetData
			
				public SeasonData GetData(Season season){
					var progressData = GetPlayerProgress();
					return progressData.GetData(season);
				}
				
				public DifficultyData GetData(Season season, Difficulty difficulty){
					var progressData = GetPlayerProgress();
					return progressData.GetData(season, difficulty);
				}
				
				public LevelData GetData(LevelIdentification levelId){
					var progressData = GetPlayerProgress();
					return progressData.GetData(levelId);
				}
			
			#endregion
			
			public void SaveProgress(LevelIdentification levelId, LevelData newData){
				var currentSeasonData = GetData(levelId.season);
				var currentDiffculty = currentSeasonData.difficulties[(int) levelId.difficulty];
				var currentLevel = currentDiffculty.levels[levelId.level];
				
					currentLevel = newData;
				
				var directory = new string[]{ GetUser().name, PlayerProgress.directoryName };
				var fileName = levelId.season.ToString();
				
					SaveManager.Save(directory, fileName, currentSeasonData);
			}
		
		#endregion
		
		#region PlayerInventory
		
			public PlayerInventory GetPlayerInventory(){
				var directory = new string[]{ GetUser().name };
				
				return SaveManager.Load<PlayerInventory>(
					directory,
					PlayerInventory.dataFileName
				);
			}
		
		#endregion
		
	#endregion
	
	#region LevelStart Callbacks
	
		public PlayerCharacter GetCharacter(DefaultCharacters character){
			var user = GetUser().name;
			var directory = new string[]{ user, PlayerCharacter.directoryName };
			
			return SaveManager.Load<PlayerCharacter>(directory, character.ToString());
		}
	
	#endregion
}