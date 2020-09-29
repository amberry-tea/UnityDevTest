using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    public class MapGenerator : MonoBehaviour
    {
        public Transform tilePrefab;
        public Transform obstaclePrefab;
        public Vector2 mapSize;

        [Range(0, 1)]
        public float outlinePercent;

        List<Coord> allTileCoords;
        Queue<Coord> shuffledTileCoords;

        public int seed = 10; //Random generation seed

        private void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            allTileCoords = new List<Coord>();
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    allTileCoords.Add(new Coord(x, y));
                }
            }
            shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));

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
                    Vector3 tilePosition = CoordToPosition(x, y);
                    //For the euler angle, Vector3.right is the X axis
                    //Multiply by 90 to set the rotation on X to 90
                    //This way the tile faces up
                    Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                    //Set all scale dimensions to the percent of outline.
                    newTile.localScale = Vector3.one * (1 - outlinePercent);
                    newTile.parent = mapHolder; //Add to holder
                }
            }

            int obstacleCount = 10;
            for(int i = 0; i < obstacleCount; i++){
                Coord randomCoord = GetRandomCoord();
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder;
            }
        }

        //Converts a coordinate on the tile map to a position in 3d space
        Vector3 CoordToPosition(int x, int y){
            //To calculate the leftmost edge, we do -mapSize.x / 2
            //This puts the tile at the center of that position. We actually want the edge to be at the leftmost edge,
            //so we shift by 0.5
            return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
        }

        //Gets a random coordinate by returning the next item in the random coord queue
        public Coord GetRandomCoord() {
            Coord randomCoord = shuffledTileCoords.Dequeue(); //Pop the random coord
            shuffledTileCoords.Enqueue(randomCoord); //Requeue  that coord to the end of the queue
            return randomCoord;
        }

        public struct Coord{
            public int x;
            public int y;

            public Coord(int _x, int _y){
                x = _x;
                y = _y;
            }
        }
    }
}
