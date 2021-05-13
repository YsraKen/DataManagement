using UnityEngine;

// this class is new (created late than the other data classes)
	// it means, some scripts use this, some are not

[System.Serializable]
public class LevelIdentification
{
	public Season season = Season.Unspecified;
	public Difficulty difficulty = Difficulty.Unspecified;
	public int level;
	
	#region Constructor
	
		public LevelIdentification(){
			season = Season.Unspecified;
			difficulty = Difficulty.Unspecified;
		}
		
		// Season only
			public LevelIdentification(Season newSeason){
				season = newSeason;
				difficulty = Difficulty.Unspecified;
			}
		
		// Season and Difficulty
			public LevelIdentification(
				Season newSeason,
				Difficulty newDifficulty
			){
				season = newSeason;
				difficulty = newDifficulty;
			}
		
		// All
			public LevelIdentification(
				Season newSeason,
				Difficulty newDifficulty,
				int newId
			){
				season = newSeason;
				difficulty = newDifficulty;
				level = newId;
			}
	
	#endregion
	
	#region Editor
	
		// For Debug.Logging
			new public string ToString(){
				return season.ToString() + "-" + difficulty.ToString() + "-" + level.ToString();
			}
		
			public string ToString(bool useSeparator){
				string noSeparator = season.ToString() + difficulty.ToString() + level.ToString();
				
				return (useSeparator)? ToString(): noSeparator;
			}
	
	#endregion
	
	#region In Game Callbacks
	
		// Dynamically Tell The Previous and Next Levels
		
		#region magic numbers
		
			public const int
				maxLevelIndex = 4,
				maxDifficultyIndex = 2,
				maxSeasonIndex = 3,
				minimumIndex = 0;
				
		#endregion
		
		// Enums
			// Dynamic, Clamped to Season, Clamped to Difficulty
		
		public LevelIdentification Next(){
			// Cache
				int nextSeasonIndex = (int) season;
				int nextDifficultyIndex = (int) difficulty;
				int nextLevelIndex = level;
			
			// Increment Level index
			nextLevelIndex ++;
			
			if(nextLevelIndex > maxLevelIndex){
				nextLevelIndex = minimumIndex; // 0
				
				// Increment Difficulty index
				nextDifficultyIndex ++;
				
				if(nextDifficultyIndex > maxDifficultyIndex){
					nextDifficultyIndex = minimumIndex; // 0
					
					// Increment Season
					nextSeasonIndex ++;
					
					nextSeasonIndex = nextSeasonIndex % maxSeasonIndex;
				}
			}
			
			// Convert to orginal types
			var next = new LevelIdentification(
				(Season) nextSeasonIndex,
				(Difficulty) nextDifficultyIndex,
				nextLevelIndex
			);
				
				// Debug.Log("CURRENT: " + ToString());
				// Debug.Log("NEXT: " + next.ToString());
				
			return next;
		}
		
		public LevelIdentification Previous(){ // NO USE YET
			// Cache
				int prevSeasonIndex = (int) season;
				int prevDifficultyIndex = (int) difficulty;
				int prevLevelIndex = level;
				
			// Decrement Level Index
			prevLevelIndex --;
			
			if(prevLevelIndex < minimumIndex){
				prevLevelIndex = maxLevelIndex;
				
				// Decrement Difficulty Index
				prevDifficultyIndex --;
				
				if(prevDifficultyIndex < minimumIndex){
					prevDifficultyIndex = maxDifficultyIndex;
					
					// Decrement Season Index
					prevSeasonIndex --;
					prevSeasonIndex = prevSeasonIndex % maxSeasonIndex;
				}
			}
			
			// Convert to orginal types
			var previous = new LevelIdentification(
				(Season) prevSeasonIndex,
				(Difficulty) prevDifficultyIndex,
				prevLevelIndex
			);
				
				// Debug.Log("CURRENT: " + ToString());
				// Debug.Log("PREVIOUS: " + previous.ToString());
			
			return previous;
		}
		
		#region Shortcuts
		
			// just like "Vector3.zero", "Color.blue", "Quaternion.identity", "transform.forward"
			
			public static LevelIdentification first{
				get{
					return new LevelIdentification(
						(Season) minimumIndex,
						(Difficulty) minimumIndex,
						minimumIndex
					);
				}
			}
			
			public static LevelIdentification last{
				get{
					return new LevelIdentification(
						(Season) maxSeasonIndex,
						(Difficulty) maxDifficultyIndex,
						maxLevelIndex
					);
				}
			}
		
		#endregion
		
		#region bools
		
			public bool IsEqualTo(LevelIdentification compare){ // called from LevelLoader
				bool isSeasonsSame = season == compare.season;
				bool isDifficultySame = difficulty == compare.difficulty;
				bool isLevelSame = level == compare.level;
				
				return isSeasonsSame && isDifficultySame && isLevelSame;
			}
			
			public bool IsFirstLevelOfSeason{
				get{
					return ((int) difficulty) == 0 && level == 0;
				}
			}
			
			public bool IsFirstLevelOfDifficulty{
				get{ return level == 0; }
			}
			
			public bool IsLastLevelOfDifficulty{
				get{ return level == maxLevelIndex; }
			}
			
			public bool IsLastLevelOfSeason{
				get{
					return ((int) difficulty) == maxDifficultyIndex && level == maxLevelIndex;
				}
			}
			
			
		#endregion
		
	#endregion
}