[System.Serializable]
public class PlayerInventory
{
	[UnityEngine.HideInInspector]
	public string username;
	
	public InventorySlot[] slots;
	
	#region Constructor
		
		public const string dataFileName = "Inventory";
		
		public PlayerInventory(){}
		
		// on new user create
		public PlayerInventory(string newUser){
			username = newUser;
			Save();
		}
		
		public void Save(){
			var directory = new string[]{ username };
			SaveManager.Save(directory, dataFileName, this);
		}
		
		/* public PlayerInventory(PlayerInventory current){
			if(current == null) return;
			if(current.slots == null) return;
			
			int length = current.slots.Length;
			slots = new InventorySlot[length];
			
			for(int i = 0; i < length; i++)
				slots[i] = new InventorySlot(current.slots[i]);
		} */
		
	#endregion
}