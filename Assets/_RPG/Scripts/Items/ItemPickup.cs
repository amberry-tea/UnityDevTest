using UnityEngine;

namespace RPG
{
    public class ItemPickup : Interactable
    {
        public Item item; //The item scriptable object to get item data from

        public override void Interact(){
            base.Interact();

            PickUp();
        }

        void PickUp()
        {
            Debug.Log("Picking up " + item.name);
            //Add to inventory
            bool wasPickedUp = Inventory.instance.Add(item);
            //Destroy gameobject
            if(wasPickedUp)
                Destroy(gameObject);
        }
    }
}
