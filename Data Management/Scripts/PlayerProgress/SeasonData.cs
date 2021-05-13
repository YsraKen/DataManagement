using System.Collections.Generic;

public enum Season{ Spring, Summer, Autumn, Winter, Unspecified }

[System.Serializable]
public class SeasonData
{
	#region Constructor
	
		[UnityEngine.HideInInspector] public string name;
		[UnityEngine.HideInInspector] public Season type;
		
		const Difficulty
			easy = Difficulty.Easy,
			normal = Difficulty.Normal,
			hard = Difficulty.Hard;
			
		static readonly DifficultyData[] defaultDifficulties = new DifficultyData[]{
			new DifficultyData(easy),
			new DifficultyData(normal),
			new DifficultyData(hard)
		};
		
		public SeasonData(Season newSeason){
			name = newSeason.ToString();
			type = newSeason;
			
			difficulties = defaultDifficulties;
		}
		
		public void Save(string username){ // with username input
			var directory = new string[]{ username, PlayerProgress.directoryName };
			SaveManager.Save(directory, name, this);
		}
	
	#endregion
	
	#region Serializables
	
		public DifficultyData[] difficulties = defaultDifficulties;
		public bool isBossLevelCompleted;
		
	#endregion

	#region InGame Callbacks
	
		// Called from LevelSelect Scene "PlayerProgress < DataManager < LevelSelect"
		public bool IsCompleted(){
			if(isBossLevelCompleted){
				
				bool isCompleted = true; // predetermine the output by instantiating a new boolean variable
				
				// check the requirements to know if is it completed or not
					
				// pit holes
					// checking only if the difficulty is completed can unlock the next season without completing (or getting the stars in) the last 5 levels
			
				// -----------------------------------------------------------------------------------------
				
				// Collecting the list of Star-Bools from all of levels in this season for easier iteration
					List<bool> stars = new List<bool>();
				
					foreach(var difficulty in difficulties)
						foreach(var level in difficulty.levels)
							stars.Add(level.star);
				
				// loop through stars list to finalize the answer
					foreach(bool star in stars){
						if(!star){
							isCompleted = false; // if there's one star the is not yet collected, then the entire season is incomplete
							break;
						}
					}
				return isCompleted;
			}
			
			else return isBossLevelCompleted;
		}
		
		public bool IsDifficultyLocked(Difficulty difficulty){
			// check previous difficulty's "IsCompleted" then decide
			
			int difficultyIndex = (int) difficulty;
			int previousIndex = difficultyIndex - 1;
			
			// the very first difficulty in array should never be locked
			var previousDifficulty = (difficultyIndex > 0)? difficulties[previousIndex]: null; // make sure that the indices are always in range
			
			bool isDifficultyLocked = false; // predetermine the output by creating a new variable
			
			if(previousDifficulty != null) // else, "isDifficultyLocked" is still false
				isDifficultyLocked = previousDifficulty.IsLocked(); // whether true or false (completed or not)
			
			return isDifficultyLocked;
		}
		
	#endregion
	
	#region ResumeLevel
	
		public LevelIdentification ResumeLevel(){
			// get the first level searched that has no key, no star acquired.
			
			var resumeLevel = new LevelIdentification(type);
			
			for(int i = 0; i < difficulties.Length; i++){
				var difficulty = difficulties[i];
				
				if(!difficulty.IsCompleted()){
					resumeLevel.difficulty = (Difficulty) i;
					resumeLevel.level = difficulty.ResumeLevel();
					
					break;
				}
			}
			return resumeLevel;
		}
	
	#endregion
}