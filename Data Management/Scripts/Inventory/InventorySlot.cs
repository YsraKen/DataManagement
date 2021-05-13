[System.Serializable]
public class InventorySlot
{
	#region Serializables
	
		public string name;
		
		public Item item;
		public int count;
	
	#endregion
	
	#region Constructor
	
		public InventorySlot(Item _item){
			item = _item;
			count = 1;
			name = item.name;
		}
		
	#endregion
	
	// Increment
	// Decrement
}