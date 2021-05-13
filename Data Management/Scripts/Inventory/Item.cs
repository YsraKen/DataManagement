using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
	new public string name;
	public Sprite icon;
	
	public virtual void Use(){
		var inventoryManager = InventoryManager.instance;
		if(!inventoryManager) return;
		
		if(InventoryManager.hasStarted) // I put this because I made an Editor Button script.
			inventoryManager.Use(this); // just decrement the count of item in slot (no actual usage of item).
	}
}