public enum Difficulty{ Easy, Normal, Hard, Unspecified }

[System.Serializable]
public class DifficultyData
{
	#region Constructor
	
	[UnityEngine.HideInInspector] public string name;
	[UnityEngine.HideInInspector] public Difficulty type;
	
	static readonly LevelData[] defaultLevel = new LevelData[]{
		new LevelData(),
		new LevelData(),
		new LevelData(),
		new LevelData(),
		new LevelData()
	};
	
	public DifficultyData(Difficulty newDifficulty){
		name = newDifficulty.ToString();
		type = newDifficulty;
		
		levels = defaultLevel;
	}
	
	#endregion
	
	#region Serializables
	
		public LevelData[] levels = defaultLevel;
		
	#endregion
	
	#region InGame Callbacks
	
		// Called from LevelSelect Scene "PlayerProgress < DataManager < LevelSelect"
		public bool IsLocked(){
			bool isLocked = false; // predetermining the output
			
			foreach(var level in levels){
				bool isKeyAcquired = level.key; // requirement: all of the levels in this difficulty should be unlocked // WE CAN IGNORE THE STAR still the difficulty will be able to complete
				
				if(!isKeyAcquired){
					isLocked = true;
					break;
				}
			}
			return isLocked;
		}
		
		// the purpose is to let the player enter the level on Level Select even the key is still not acquired (on the very first level of array, there is no way for player the enter the very first level if we don't automatically unlock it for them)
		public bool IsLevelLocked(int index){
			int previousIndex = index - 1;
			
			// the very first level in array should never be locked
			var previousLevel = (index > 0)? levels[previousIndex]: null; // make sure that the indices are always in range
			
			bool isLevelLocked = false; // predetermine the output by creating a new variable
			
			if(previousLevel != null) // else, isLevelLocked is still false
				isLevelLocked = !previousLevel.key; // whether true or false (completed or not)
			
			return isLevelLocked;
		}
		
	#endregion
	
	#region ResumeLevel
		
		public bool IsCompleted(){
			bool isDifficultyCompleted = true; // predetermining the output
			
			foreach(var level in levels){
				
				if(!level.IsCompleted()){
					isDifficultyCompleted = false;
					break;
				}
			}
			return isDifficultyCompleted;
		}
	
		
		public int ResumeLevel(){
			int resumeLevel = 0;
			
			for(int i = 0; i < levels.Length; i++){
				var level = levels[i];
				
				if(!level.IsCompleted()){
					resumeLevel = i;
					break;
				}
			}
			return resumeLevel;
		}
		
	#endregion
}