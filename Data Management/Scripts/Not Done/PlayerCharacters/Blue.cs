public class Blue : PlayerCharacter
{
	public Blue(){
		character = DefaultCharacters.Blue;
		name = character.ToString();
		
		// i will add the specifics later
		stats = defaultStats;
	}
	
	// for newly created user
	public Blue(string username){
		user = username;
		
		character = DefaultCharacters.Blue;
		name = character.ToString();
		
		// i will add the specifics later
		stats = defaultStats;
		
		Save();
	}
}