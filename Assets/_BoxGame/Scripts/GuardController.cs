using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BoxGame
{
    public class GuardController : MonoBehaviour
    {
        Transform player;
        NavMeshAgent agent;
        private int health = 100;

        public LayerMask whatIsGround, whatIsPlayer;
        public GameObject projectile;

        //Patrolling
        public Vector3 walkPoint;
        bool walkPointSet;
        public float walkPointRange;

        //Attacking
        public float timeBetweenAttacks;
        bool alreadyAttacked;

        //States
        public float sightRange, attackRange;
        [SerializeField]
        bool playerInSightRange, playerInAttackRange;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            player = PlayerManager.instance.player.transform;
        }

        void Update()
        {
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patrolling();
            else if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            else if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }

        //~~~~~~
        //STATES
        //~~~~~~
        private void Patrolling()
        {
            //Search for a walk point until we find a suitable point to walk to
            if (!walkPointSet)
            {
                SearchWalkPoint();
            }
            else if (walkPointSet)
            {
                agent.SetDestination(walkPoint);

                Vector3 distanceToWalkPoint = transform.position - walkPoint;
                //Debug.Log(distanceToWalkPoint.magnitude.ToString());

                //Walkpoint reached. We assume the AI is stuck if it stops for any reason.
                //Should be fixed if you want pauses durring patrol.
                if (distanceToWalkPoint.magnitude <= 1f || agent.velocity == Vector3.zero)
                    walkPointSet = false;
            }

        }

        private void SearchWalkPoint()
        {
            //Calculate a random point in the walk range
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            //Make sure that the walk point is actually on the ground (not outside the map)
            NavMeshHit hit;
            if (NavMesh.SamplePosition(walkPoint, out hit, 1f, NavMesh.AllAreas))
            {
                walkPoint = hit.position;
                walkPointSet = true;
            }
        }

        private void ChasePlayer()
        {
            agent.SetDestination(player.position);
        }

        private void AttackPlayer()
        {
            //Make sure enemy doesn't move
            agent.SetDestination(transform.position);

            transform.LookAt(player);

            if (!alreadyAttacked)
            {
                //Attack code here, ie shoot projectile
                Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
                rb.AddForce(transform.up * 2f, ForceMode.Impulse);

                alreadyAttacked = true;
                Invoke("ResetAttack", timeBetweenAttacks);
            }
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        public void TakeDamage(int damage)
        {
            //Code for when the enemy is hit
            health -= damage;
            if (health <= 0) Invoke("DestroyEnemy", 0.1f);
        }

        private void destroyEnemy()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }
    }
}