using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    public class Spawner : MonoBehaviour
    {
        public Wave[] waves;
        public Enemy enemy;

        Wave currentWave;
        int currentWaveNumber;

        int enemiesRemainingToSpawn;
        int enemiesRemainingAlive;
        float nextSpawnTime;

        void Start()
        {
            NextWave();
        }
        void Update()
        {
            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
                spawnedEnemy.OnDeath += OnEnemyDeath; //Set the event handler for enemy death
            }
        }

        void NextWave()
        {
            currentWaveNumber++;
            //print("Wave " + currentWaveNumber);
            if(currentWaveNumber - 1 < waves.Length){
                currentWave = waves[currentWaveNumber - 1];

                enemiesRemainingToSpawn = currentWave.enemyCount;
                enemiesRemainingAlive = enemiesRemainingToSpawn;
            }
        }

        void OnEnemyDeath(){
            //print("Enemy died");
            enemiesRemainingAlive--;

            if(enemiesRemainingAlive == 0)
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