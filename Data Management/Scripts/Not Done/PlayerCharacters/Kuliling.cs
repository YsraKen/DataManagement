public class Kuliling : PlayerCharacter
{
	public Kuliling(){
		character = DefaultCharacters.Kuliling;
		name = character.ToString();
		
		// i will add the specifics later
			stats = defaultStats;
	}
	
	// for newly created user
	public Kuliling(string username){
		user = username;
		
		character = DefaultCharacters.Kuliling;
		name = character.ToString();
		
		// i will add the specifics later
			stats = defaultStats;
			
		Save();
	}
}