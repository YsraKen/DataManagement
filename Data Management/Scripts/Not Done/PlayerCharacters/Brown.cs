public class Brown : PlayerCharacter
{
	public Brown(){
		character = DefaultCharacters.Brown;
		name = character.ToString();
		
		// i will add the specifics later
		stats = defaultStats;
	}
	
	// for newly created user
	public Brown(string username){
		user = username;
		
		character = DefaultCharacters.Brown;
		name = character.ToString();
		
		// i will add the specifics later
		stats = defaultStats;
		
		Save();
	}
}