using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    public class MapGenerator : MonoBehaviour
    {
        public Transform tilePrefab;
        public Vector2 mapSize;

        [Range(0, 1)]
        public float outlinePercent;

        private void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            string holderName = "Generated Map"; //The name of the empty gameObject to store tiles under
            if (transform.Find(holderName)) //Find the holder game object in the children of this object
            {
                DestroyImmediate(transform.Find(holderName).gameObject); //DestroyImmediate is used because we're calling it from the editor
            }

            Transform mapHolder = new GameObject(holderName).transform; //Create a new map holder after destroying the last one
            mapHolder.parent = transform; //Set the parent of the object to this objects transform

            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    //To calculate the leftmost edge, we do -mapSize.x / 2
                    //This puts the tile at the center of that position. We actually want the edge to be at the leftmost edge,
                    //so we shift by 0.5
                    Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
                    //For the euler angle, Vector3.right is the X axis
                    //Multiply by 90 to set the rotation on X to 90
                    //This way the tile faces up
                    Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                    //Set all scale dimensions to the percent of outline.
                    newTile.localScale = Vector3.one * (1 - outlinePercent);
                    newTile.parent = mapHolder; //Add to holder
                }
            }
        }
    }
}
