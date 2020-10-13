using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMotor : MonoBehaviour
    {
        Transform target; //target to follow
        NavMeshAgent agent;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if(target != null)
            {
                FaceTarget(); //Point towards the target cause the navmesh too janky to do it on its own
                //agent.SetDestination(target.position); //MAZUI, dont update navmesh dir 60 times a second
            }
        }

        public void MoveToPoint(Vector3 point)
        {
            agent.SetDestination(point);
        }

        public void FollowTarget(Interactable newTarget)
        {
            agent.stoppingDistance = newTarget.radius * .8f;
            agent.updateRotation = false; //We are handling rotation ourselves cause it turns itself off when in stopping distance on default

            target = newTarget.interactionTransform;
            StartCoroutine("UpdatePath");
        }

        public void StopFollowingTarget()
        {
            agent.stoppingDistance = 0f;
            agent.updateRotation = true; //Re-enable navagent controlling the rotation (see FollowTarget() for reason)

            target = null;
        }

        IEnumerator UpdatePath()
        {
            float refreshRate = .25f;

            while (target != null)
            {
                //Vector3 dirToTarget = (target.position - transform.position).normalized; //Direction of the target
                //Vector3 targetPos = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2); //Target position minus the length of how thicc we are (in the right direction)
                //if (!dead)
                //{ //Make sure code doesn't run after the enemy was killed. Important for coroutines to make sure the object state is still desirable.
                agent.SetDestination(target.position);
                //}
                yield return new WaitForSeconds(refreshRate);
            }

        }

        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized; //Get the direction of the target
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)); //Get the rotation to the target (not including y axis)
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5); //Smoothly rotate towards the target using Slerp
        }
    }
}
