[System.Serializable]
public class PlayerData
{
	[UnityEngine.HideInInspector] public string name; // just a shortcut for saving and loading
	
	public int coins;
	public DefaultCharacters character;
	
	#region Constructor
		
		public PlayerData(){}
		
		// Save
		public PlayerData(string newUser){
			// Create new user folder
				SaveManager.CreateDirectory(newUser);
					
			// Serializable variables
				name = newUser;
				coins = 0;
				character = (DefaultCharacters) 0;
			
			// Create new user folder
				SaveManager.CreateDirectory(newUser);
				
			// Create data files
				new PlayerProgress(newUser);
				new Edison(newUser); // PlayerCharacter
				new PlayerInventory(newUser);
					// these includes individual hard saving in it!
					 
			// Hard Save
				Save();
		}
		
		// Just save, no constructing // Called from LevelStart (OnStartButton > to update the selected character)
		public void Save(){
			var directory = new string[]{ name };
			SaveManager.Save(directory, name, this);
		}
	
	#endregion
}