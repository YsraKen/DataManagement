using UnityEngine;

[System.Serializable]
public class PlayerStats
{
	public string name;
	[Range(0,1)] public float level; // this will be a lerp to multiplierMinMax
	public Vector2 multiplierMinMax; // this will multiply the percent to a constant value (move speed, jump force)
	public int price;
	
	static float defaultLevel = 0f;
	static Vector2 defaultMultiplierMinMax = new Vector2(0.5f, 1.5f);
	static int defaultPrice = 100;
	
	#region Constructor
	
	// blank parameters
	public PlayerStats(){
		name = "New Stat";
		level = defaultLevel;
		multiplierMinMax = defaultMultiplierMinMax;
		price = defaultPrice;
	}
	
	// name only
	public PlayerStats(string newName){
		name = newName;
		level = defaultLevel;
		multiplierMinMax = defaultMultiplierMinMax;
		price = defaultPrice;
	}
	
	// all parameters
	public PlayerStats(
		string newName,
		float newLevel,
		Vector2 newMinMax,
		int newPrice
	){
		name = newName;
		level = newLevel;
		multiplierMinMax = newMinMax;
		price = newPrice;
	}
	
	#endregion
}