﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    public class Spawner : MonoBehaviour
    {
        public Wave[] waves;
        public Enemy enemy;

        LivingEntity playerEntity;
        Transform playerT;

        Wave currentWave;
        int currentWaveNumber;

        int enemiesRemainingToSpawn;
        int enemiesRemainingAlive;
        float nextSpawnTime;

        MapGenerator map;

        float timeBetweenCampingChecks = 2; //Ammount of time to move campThresholdDistance to not be camping
        float campThresholdDistance = 1.5f; //Distance that needs to be moved to not be camping
        float nextCampCheckTime;
        Vector3 campPositionOld;
        bool isCamping;

        bool isDisabled;

        void Start()
        {
            playerEntity = FindObjectOfType<Player>();
            playerT = playerEntity.transform;

            nextCampCheckTime = timeBetweenCampingChecks + Time.time;
            campPositionOld = playerT.position;
            playerEntity.OnDeath += OnPlayerDeath;

            map = FindObjectOfType<MapGenerator>();
            NextWave();
        }
        void Update()
        {
            if (!isDisabled)
            {
                //Check if player is camping
                if (Time.time > nextCampCheckTime)
                {
                    nextCampCheckTime = Time.time + timeBetweenCampingChecks;

                    //The player is deemed camping if he hasnt moved from the campPosition stored last time
                    isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campThresholdDistance);

                    campPositionOld = playerT.position;
                }

                if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
                {
                    enemiesRemainingToSpawn--;
                    nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                    StartCoroutine(SpawnEnemy());
                }
            }
        }

        IEnumerator SpawnEnemy()
        {
            float spawnDelay = 1; //How long tile flashes for before spawning
            float tileFlashSpeed = 4; //flashes per second

            Transform spawnTile = map.GetRandomOpenTile();
            if (isCamping)
            {
                spawnTile = map.GetTileFromPosition(playerT.position);
            }
            Material tileMat = spawnTile.GetComponent<Renderer>().material;
            Color initialColour = tileMat.color;
            Color flashColour = Color.red;
            float spawnTimer = 0; //How much time has passed since start of coroutine

            //Make the spawner wait while the tile flashes
            while (spawnTimer < spawnDelay)
            {
                //Mathf.PingPong(): For incrementing value in param 1, return value will bounce between 0 and param 2
                tileMat.color = Color.Lerp(initialColour, flashColour, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

                spawnTimer += Time.deltaTime;
                yield return null;
            }

            Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath; //Set the event handler for enemy death
        }

        void NextWave()
        {
            currentWaveNumber++;
            //print("Wave " + currentWaveNumber);
            if (currentWaveNumber - 1 < waves.Length)
            {
                currentWave = waves[currentWaveNumber - 1];

                enemiesRemainingToSpawn = currentWave.enemyCount;
                enemiesRemainingAlive = enemiesRemainingToSpawn;
            }
        }

        void OnPlayerDeath()
        {
            isDisabled = true;
        }

        void OnEnemyDeath()
        {
            //print("Enemy died");
            enemiesRemainingAlive--;

            if (enemiesRemainingAlive == 0)
            {
                NextWave();
            }
        }

        [System.Serializable]
        public class Wave
        {
            public int enemyCount;
            public float timeBetweenSpawns;
        }
    }
}