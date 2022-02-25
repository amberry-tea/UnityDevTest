using UnityEngine;
using UnityEngine.UI;

namespace RPG
{

    /// Keeps track of everything happening to a particular inventory slot.
    ///
    /// Updates the UI, and controls the funcitonality for the slot (on click, on remove...)
    public class InventorySlot : MonoBehaviour
    {
        //Our image for the slot
        public Image icon;

        //The item in the slot
        Item item;

        public Button removeButton;

        public void AddItem (Item item)
        {
            this.item = item;

            icon.sprite = this.item.icon;
            icon.enabled = true;
            removeButton.interactable = true;
        }

        public void ClearSlot()
        {
            item = null;

            icon.sprite = null;
            icon.enabled = false;
            removeButton.interactable = false;
        }

        public void OnRemoveButton()
        {
            Inventory.instance.Remove(item);
        }

        public void UseItem()
        {
            if(item != null){
                item.Use();
            }
        }
    }
}
