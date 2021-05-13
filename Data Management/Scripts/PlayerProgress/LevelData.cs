[System.Serializable]
public class LevelData
{
	public bool key, star;
	
	// [UnityEngine.TextArea(5,5)]
	// public string summary;
	
	public bool IsCompleted(){ return key && star; } // currently called from MainMenu > Play or Resume Button
	public bool IsStarAcquired(){ return star; }
	
	// public void AcquireStar(){ star = true; }
}