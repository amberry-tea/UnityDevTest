using UnityEngine;

/**
* A scriptable object to create items from
*/
namespace RPG
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")] //Allows you to create scriptable objects through the (right click) > (Create) menu (in project tab)
    public class Item : ScriptableObject
    {
        //Theres already a variable called "Name" on the object class, so we use the "new" keyword to override that var
        new public string name = "New Item";
        public Sprite icon = null; //The icon that shows up in the inventory
        public bool isDefaultItem = false; //For the default items like underwear that we dont want to appear in the inventory

        public virtual void Use()
        {
            //Only some items should implement Use(), eg. crafting items are used differently than potions

            Debug.Log("Using" + name);
        }
    }
}
