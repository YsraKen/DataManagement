public class Edison : PlayerCharacter
{
	public Edison(){
		character = DefaultCharacters.Edison;
		name = character.ToString();
		
		// i will add the specifics later
		stats = defaultStats;
	}

	// for newly created user
	public Edison(string username){
		user = username;
		
		character = DefaultCharacters.Edison;
		name = character.ToString();
		
		// i will add the specifics later
		stats = defaultStats;
		
		Save();
	}
}