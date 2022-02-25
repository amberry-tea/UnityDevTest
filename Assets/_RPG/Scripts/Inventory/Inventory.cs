using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Inventory : MonoBehaviour
    {

        #region Singleton
        public static Inventory instance;

        void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("More than one instance of Inventory found!");
                return;
            }

            instance = this;
        }
        #endregion

        public delegate void OnItemChanged();
        public event OnItemChanged onItemChangedCallback;

        public int space = 20;

        public List<Item> items = new List<Item>();

        public bool Add(Item item)
        {
            if (!item.isDefaultItem) //Default items dont show in the inventory
            {
                if(items.Count >= space){
                    Debug.Log("Not enough room.");
                    return false;
                }
                items.Add(item);

                if(onItemChangedCallback != null)
                    onItemChangedCallback.Invoke(); //Trigger event
            }

            return true;
        }

        public void Remove(Item item)
        {
            items.Remove(item);
            
            if(onItemChangedCallback != null)
                    onItemChangedCallback.Invoke(); //Trigger event
        }
    }
}
