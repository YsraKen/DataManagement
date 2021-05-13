[System.Serializable]
public class PlayerProgressWrapper // to save lastLevelLoaded and maxLevelAchieved separately
{
 	public LevelIdentification
		lastLevelLoaded,
		maxLevelAchieved;
		
	public SeasonData[] seasons;
	
	#region Constructors

		// Save
		public PlayerProgressWrapper(PlayerProgress progressData){
			
			lastLevelLoaded = progressData.lastLevelLoaded;
			maxLevelAchieved = progressData.maxLevelAchieved;
			
			// seasons = progressData.seasons; //  seasons are saved separately (Different folder and files).
		}
		
		public PlayerProgress LoadProgressData(string username){
			// cache
				int maxSeasonLength = 4;
				var directory = new string[]{ username, PlayerProgress.directoryName };
			
			seasons = new SeasonData[maxSeasonLength];
			
			for(int i = 0; i < maxSeasonLength; i++){
				var type = (Season) i;
				seasons[i] = SaveManager.Load<SeasonData>(directory, type.ToString());
			}
			
			return new PlayerProgress(this);
		}
		
	#endregion
}