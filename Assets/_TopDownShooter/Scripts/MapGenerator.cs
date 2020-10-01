using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    public class MapGenerator : MonoBehaviour
    {
        public Transform tilePrefab;
        public Transform obstaclePrefab;
        public Transform navmeshFloor;
        public Transform navmeshMaskPrefab;
        public Vector2 mapSize;
        public Vector2 maxMapSize;

        [Range(0, 1)]
        public float outlinePercent;
        [Range(0, 1)]
        public float obstaclePercent;

        public float tileSize;

        List<Coord> allTileCoords;
        Queue<Coord> shuffledTileCoords; //We use a queue so that every time we get a random coordinate, we move it to the back of the queue.

        public int seed = 10; //Random generation seed
        Coord mapCenter;

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
            mapCenter = new Coord((int)mapSize.x / 2, (int)mapSize.y / 2);

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
                    newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                    newTile.parent = mapHolder; //Add to holder
                }
            }

            bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y]; //keeps track of what tiles are occupied by obstacles 

            int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
            int currentObstacleCount = 0;

            for (int i = 0; i < obstacleCount; i++)
            {
                Coord randomCoord = GetRandomCoord();

                obstacleMap[randomCoord.x, randomCoord.y] = true;
                currentObstacleCount++;

                //Makes sure the map is fully accessible. We cant spawn a tile in the center, because that is the origin
                //from which we determine if things are accessible.
                if (randomCoord != mapCenter && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
                {
                    //Instantiate the obstacles
                    Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);

                    Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * .5f, Quaternion.identity) as Transform;
                    newObstacle.parent = mapHolder;
                    newObstacle.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                }
                else
                {
                    obstacleMap[randomCoord.x, randomCoord.y] = false;
                    currentObstacleCount--;
                }
            }

            //Masks the navmesh
            //See notes for explanation of the formula for the position
            //By using Vector3.right or Vector3.left, we determine what direction to go in when we multiply it by the formula we made
            Transform maskLeft = Instantiate(navmeshMaskPrefab, Vector3.left * (mapSize.x + maxMapSize.x) / 4 * tileSize, Quaternion.identity) as Transform;
            maskLeft.parent = mapHolder;
            maskLeft.localScale = new Vector3((maxMapSize.x - mapSize.x)/2, 1, mapSize.y) * tileSize; //Stretch it out to fill the area between the map edge and max map edge
            
            Transform maskRight = Instantiate(navmeshMaskPrefab, Vector3.right * (mapSize.x + maxMapSize.x) / 4 * tileSize, Quaternion.identity) as Transform;
            maskRight.parent = mapHolder;
            maskRight.localScale = new Vector3((maxMapSize.x - mapSize.x)/2, 1, mapSize.y) * tileSize;

            Transform maskTop = Instantiate(navmeshMaskPrefab, Vector3.forward * (mapSize.y + maxMapSize.y) / 4 * tileSize, Quaternion.identity) as Transform;
            maskTop.parent = mapHolder;
            maskTop.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - mapSize.y)/2) * tileSize;

            Transform maskBottom = Instantiate(navmeshMaskPrefab, Vector3.back * (mapSize.y + maxMapSize.y) / 4 * tileSize, Quaternion.identity) as Transform;
            maskBottom.parent = mapHolder;
            maskBottom.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - mapSize.y)/2) * tileSize;

            //In this case, because the floor is rotated by 90 degrees, what we see as the Z axis is actually the objects Y axis
            navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize;
        }

        /**
        * Determines if the obstacles are blocking any paths in the map
        *
        * Uses a floodfill algorithm to count the number of empty tiles from the center of the map.
        * If the number of tiles the floodfill finds does not equal the actual number of tiles without
        * obstacles, then we return false.
        */
        bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
        {
            //Keeps track of what tiles we have already checked
            bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
            Queue<Coord> queue = new Queue<Coord>();

            queue.Enqueue(mapCenter);
            mapFlags[mapCenter.x, mapCenter.y] = true;

            int accessibleTileCount = 1;

            while (queue.Count > 0)
            {
                Coord tile = queue.Dequeue();
                //Go through all of the adjacent tiles
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        int neighbourX = tile.x + x;
                        int neighbourY = tile.y + y;
                        //Make sure we dont check the diagonals
                        if (x == 0 || y == 0)
                        {
                            //Make sure the coordinate is within our map
                            if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                            {
                                //Make sure we havent checked the tile and that its not an obstacle tile
                                if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                                {
                                    mapFlags[neighbourX, neighbourY] = true;
                                    queue.Enqueue(new Coord(neighbourX, neighbourY)); //Look at this tile's neighbours
                                    accessibleTileCount++;
                                }
                            }
                        }
                    }
                }
            }

            //How many tiles should there be?
            int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
            return targetAccessibleTileCount == accessibleTileCount;
        }

        //Converts a coordinate on the tile map to a position in 3d space
        Vector3 CoordToPosition(int x, int y)
        {
            //To calculate the leftmost edge, we do -mapSize.x / 2
            //This puts the tile at the center of that position. We actually want the edge to be at the leftmost edge,
            //so we shift by 0.5
            return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y) * tileSize;
        }

        //Gets a random coordinate by returning the next item in the random coord queue
        public Coord GetRandomCoord()
        {
            Coord randomCoord = shuffledTileCoords.Dequeue(); //Pop the random coord
            shuffledTileCoords.Enqueue(randomCoord); //Requeue  that coord to the end of the queue
            return randomCoord;
        }

        public struct Coord
        {
            public int x;
            public int y;

            public Coord(int _x, int _y)
            {
                x = _x;
                y = _y;
            }

            public static bool operator ==(Coord c1, Coord c2)
            {
                return c1.x == c2.x && c1.y == c2.y;
            }
            
            public static bool operator !=(Coord c1, Coord c2)
            {
                return !(c1==c2);
            }
        }
    }
}
