[System.Serializable]
public class PlayerProgress
{
	#region Serializables
		
		public LevelIdentification
			lastLevelLoaded = new LevelIdentification((Season) 0, (Difficulty) 0),
			maxLevelAchieved = new LevelIdentification((Season) 0, (Difficulty) 0);
			
		public SeasonData[] seasons = new SeasonData[]{
			new SeasonData(Season.Spring),
			new SeasonData(Season.Summer),
			new SeasonData(Season.Autumn),
			new SeasonData(Season.Winter)
		};
	
	#endregion
	
	#region Constructors
	
		public const string
			directoryName = "Progress",
			dataFileName = "PlayerProgress";
	
		// default constructor, no arguments (no specific use, but can be handy in some situation when instantiating an empty "PlayerProgress")
		public PlayerProgress(){
			lastLevelLoaded = new LevelIdentification((Season) 0, (Difficulty) 0);
			maxLevelAchieved = new LevelIdentification((Season) 0, (Difficulty) 0);
			
			seasons = new SeasonData[]{
				new SeasonData(Season.Spring),
				new SeasonData(Season.Summer),
				new SeasonData(Season.Autumn),
				new SeasonData(Season.Winter)
			};
		}
		
		// Create New
		public PlayerProgress(string username){
			// cache
				string[] directory = new string[]{ username, directoryName };
				int maxSeasonLength = 4;
			
			seasons = new SeasonData[maxSeasonLength];
			
			for(int i = 0; i < maxSeasonLength; i ++){
				var type = (Season) i;
				
				seasons[i] = new SeasonData(type);
				seasons[i].Save(username);
			}
			
			// For Wrapper Data
				lastLevelLoaded = new LevelIdentification((Season) 0, (Difficulty) 0);
				maxLevelAchieved = new LevelIdentification((Season) 0, (Difficulty) 0);
			
				var newDataWrapper = new PlayerProgressWrapper(this);
				SaveManager.Save(directory, dataFileName, newDataWrapper);
				
			// QUESTION:
				// What is PlayerProgressWrapper use for?
					// It is a PlayerProgress data without "SeasonData" in it, the purpose is to shorten the save and organize it in different files.
		}
		
		// Load
		public PlayerProgress(PlayerProgressWrapper dataWrapper){
			lastLevelLoaded = dataWrapper.lastLevelLoaded;
			maxLevelAchieved = dataWrapper.maxLevelAchieved;
			seasons = dataWrapper.seasons;
		}
		
		// Save - these methods aren't use above, this are for other scripts and is mostly called during gamePlay not much in creating user
			public void Save(string user){ // no use yet
				// cache
					string[] directory = new string[]{ user, directoryName };
					
				for(int i = 0; i < seasons.Length; i ++)
					seasons[i].Save(user);
			}
			
			public void SaveWrapper(string user){
				// cache
					string[] directory = new string[]{ user, directoryName };
					
				var newDataWrapper = new PlayerProgressWrapper(this);
				SaveManager.Save(directory, dataFileName, newDataWrapper);
			}
	
	#endregion

	// --------------------------- PUBLIC METHODS / CALL BACKS --------------------------- //
	
	// Called from Level Select
	#region Lock Checks
	
		// Season
		public bool IsLocked(Season season){ // NEW: just check if the last level of season (boss battle) is completed [of previous season] {still check for star completion}
			// check previous season's "IsCompleted" then unlock
			int seasonIndex = (int) season;
			int previousIndex = seasonIndex - 1;
			
			// the very first season in array should never be locked
			var previousSeason = (seasonIndex > 0)? seasons[previousIndex]: null; // make sure that the indices are always in range
			
			bool isSeasonLocked = false; // predetermine the output by creating a new variable
			
			if(previousSeason != null) // else, "isSeasonLocked" is still false
				isSeasonLocked = !previousSeason.IsCompleted(); // whether true or false (completed or not)
			
			return isSeasonLocked;
		}
		
		// Difficulty
		public bool IsLocked(Season season, Difficulty difficulty){
			// check it on SeasonData, because it holds the Difficulties array, and in order for us to tell the answer, it should base on previous difficulty in that array
			
			int index = (int) season;
			bool isDifficultyLocked = seasons[index].IsDifficultyLocked(difficulty);
			
			return isDifficultyLocked;
		}
		
		// Level
		public bool IsLocked(LevelIdentification identification){ // use from: Level manager "OnLevelFinished", LevelSelect_LevelButton "Setup"
			// check it on DifficultyData, because it holds the levels array
			
			var season = seasons[(int) identification.season];
			var difficulty = season.difficulties[(int) identification.difficulty];
			
			bool isLevelLocked = difficulty.IsLevelLocked(identification.level);
			
			return isLevelLocked;
		}
		
		// New: Called from LevelSelect "Extra Levels"
			public bool IsBossLevelLocked(Season season){
				var lastLevel = LevelIdentification.last; // winter, hard, 4
					lastLevel.season = season; // "season", hard, 4
					
				bool isLastLevelLocked = IsLocked(lastLevel);
				return isLastLevelLocked;
			}
			
			public bool IsBossLevelCompleted(Season season){ // minigame buttons of season depends on this condition, the buttons will not show until the boss battle is finished
				return seasons[(int) season].isBossLevelCompleted;
			}

	#endregion

	#region Gets
		
		public SeasonData GetData(Season season){
			return seasons[(int) season];
		}
		
		public DifficultyData GetData(Season season, Difficulty difficulty){
			int seasonIndex = (int) season;
			int difficultyIndex = (int) difficulty;
			
			return seasons[seasonIndex].difficulties[difficultyIndex];
		}
		
		public LevelData GetData(Season season, Difficulty difficulty, int index){
			// UnityEngine.Debug.Log("tae");
			var currentSeason = seasons[(int) season];
			var currentDifficulty = currentSeason.difficulties[(int) difficulty];
			
			if(index > currentDifficulty.levels.Length) return null;
			
			return currentDifficulty.levels[index];
		}
			
			// Override for "Get Level Data"
			public LevelData GetData(LevelIdentification identification){ // Called from "LevelSelect_LevelButton"
				
				return GetData(
					identification.season,
					identification.difficulty,
					identification.level
				);
			}
	
	#endregion	
	
	#region MaxLevel and ResumeLevel
	
		public LevelIdentification IsMaxLevelAchieved(LevelIdentification newEntry){
			
			// check for highest season
			// check for highest difficulty
			// check for highest level id
			
			LevelIdentification output = maxLevelAchieved; // predetermine the output
			
			// check for highest season
				int newSeason = (int) newEntry.season;
				int currentSeason = (int) maxLevelAchieved.season;
				
				if(newSeason > currentSeason) output = newEntry;
				else{
					int newDifficulty = (int) newEntry.difficulty;
					int currentDifficulty = (int) maxLevelAchieved.difficulty;
					
					if(newDifficulty > currentDifficulty) output = newEntry;
					else{
						int newLevel = newEntry.level;
						int currentLevel = maxLevelAchieved.level;
						
						if(newLevel > currentLevel) output = newEntry;
					}
				}
				
			return output;
		}
		
		public LevelIdentification ResumeLevel(){
			var resumeLevel = new LevelIdentification();
			
			for(int i = 0; i < seasons.Length; i++){
				var season = seasons[i];
				
				if(!season.IsCompleted()){
					resumeLevel = season.ResumeLevel();
					break;
				}
			}
			return resumeLevel;
		}
	
	#endregion
}