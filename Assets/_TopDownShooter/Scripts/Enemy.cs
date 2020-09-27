using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TopDownShooter
{
    [RequireComponent (typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        NavMeshAgent pathfinder;
        Transform target;

        void Start()
        {
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
                pathfinder.SetDestination (targetPos);
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
