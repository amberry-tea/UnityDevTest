using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TopDownShooter
{
    [RequireComponent (typeof(NavMeshAgent))]
    public class Enemy : LivingEntity
    {
        NavMeshAgent pathfinder;
        Transform target;

        protected override void Start()
        {
            base.Start();
            pathfinder = GetComponent<NavMeshAgent>();
            target = GameObject.FindGameObjectWithTag("Player").transform;

            StartCoroutine(UpdatePath());
        }

        void Update()
        {
        }

        IEnumerator UpdatePath() {
            float refreshRate = .25f;

            while(target != null){
                Vector3 targetPos = new Vector3(target.position.x, 0, target.position.z);
                if(!dead){ //Make sure code doesn't run after the enemy was killed. Important for coroutines to make sure the object state is still desirable.
                    pathfinder.SetDestination (targetPos);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
