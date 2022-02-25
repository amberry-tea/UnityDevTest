using UnityEngine;

namespace RPG
{
    public class InventoryUI : MonoBehaviour
    {

        public Transform itemsParent;
        public GameObject inventoryUI;

        InventorySlot[] slots;
        Inventory inventory;

        // Start is called before the first frame update
        void Start()
        {
            inventory = Inventory.instance;
            inventory.onItemChangedCallback += UpdateUI;
            slots = itemsParent.GetComponentsInChildren<InventorySlot>(); //If the size of your backpack changes, make sure to update the slots array!
        }

        // Update is called once per frame
        void Update()
        {
            // Hide or show the inventory by pressing i (the inventory button as bound in the input manager in project settings)
            if(Input.GetButtonDown("Inventory"))
            {
                inventoryUI.SetActive(!inventoryUI.activeSelf);
            }
        }

        void UpdateUI (){
            Debug.Log("Updating UI!");

            for (int i = 0; i < slots.Length; i++)
            {
                // if there are items in the inventory
                if(i < inventory.items.Count)
                {
                    slots[i].AddItem(inventory.items[i]);
                } 
                else //if there are no other items to add to our inventory
                {
                    slots[i].ClearSlot(); // make that inventory space empty
                }
            }
        }
    }
}
