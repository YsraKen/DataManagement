[System.Serializable]
public class PlayerCharacter
{
	[UnityEngine.HideInInspector] public string user; // new: this is a shortcut for accessing the data without passing through userDatabase
	[UnityEngine.HideInInspector] public string name;
	
	public DefaultCharacters character;
	public PlayerStats[] stats;
	
	// for script that inherits "this", they will defaultly assign it to their own stats array.
	protected static PlayerStats[] defaultStats = new PlayerStats[]{
		new PlayerStats("Move Speed"),
		new PlayerStats("Jump Force"),
		new PlayerStats("Aerial Down Force")
	};
	
	#region Constructors
		
		public const string
			directoryName = "Characters";
		
		public PlayerCharacter(){}
		
		public virtual void Save(){
			var directory = new string[]{ user, directoryName };
			SaveManager.Save(directory, name, this);
		}
	
	#endregion
}

public enum DefaultCharacters{ Edison, Kuliling, Brown, Blue, Unspecified }