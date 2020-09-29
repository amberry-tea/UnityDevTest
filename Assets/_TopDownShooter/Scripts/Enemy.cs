using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TopDownShooter
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : LivingEntity
    {

        public enum State { Idle, Chasing, Attacking };
        State currentState;

        NavMeshAgent pathfinder;
        Transform target;
        LivingEntity targetEntity;
        Material skinMaterial;

        Color originalColor;

        float attackDistanceThreshold = .5f;
        float timeBetweenAttacks = 1;
        float damage = 1;

        float nextAttackTime;
        float myCollisionRadius; //how thicc this boi is (to keep him from walkin in the player)
        float targetCollisionRadius; //how thicc his target is

        bool hasTarget;

        protected override void Start()
        {
            base.Start();
            pathfinder = GetComponent<NavMeshAgent>();
            skinMaterial = GetComponent<Renderer>().material;
            originalColor = skinMaterial.color;

            //DONT assume the player exists in the scene: this enemy may spawn after the player has died!
            if (GameObject.FindGameObjectWithTag("Player") != null) //Set up chasing code
            {
                currentState = State.Chasing;
                hasTarget = true;

                target = GameObject.FindGameObjectWithTag("Player").transform;
                targetEntity = target.GetComponent<LivingEntity>();
                targetEntity.OnDeath += OnTargetDeath;

                myCollisionRadius = GetComponent<CapsuleCollider>().radius;
                targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

                StartCoroutine(UpdatePath());
            }
        }

        void OnTargetDeath()
        {
            hasTarget = false;
            currentState = State.Idle;
        }

        void Update()
        {
            if(hasTarget){
                if (Time.time > nextAttackTime)
                {
                    //CALCULATING DISTANCE BETWEEN TARGETS THE RIGHT WAY
                    float sqrDstToTarget = (target.position - transform.position).sqrMagnitude; //Square distance so we dont have to calculate a square root uneccesarily
                    //Square the attack distance to match with the squared distance to target
                    //Add in the length of both our collision radii to lengthen the attack distance. Equivelant of subtracting it from the variable.
                    //This makes us calculate the distance between the target from the edges of our colliders rather than the centers of our game objects.
                    if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                    {
                        nextAttackTime = Time.time + timeBetweenAttacks;
                        StartCoroutine(Attack());
                    }
                }
            }
        }

        IEnumerator Attack()
        {
            currentState = State.Attacking;
            pathfinder.enabled = false;

            Vector3 originalPosition = transform.position; //Point of start/end for animation
            Vector3 dirToTarget = (target.position - transform.position).normalized; //Direction of the target
            Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius); //Target position minus the length of how thicc we are (in the right direction)

            float percent = 0; //Percent of the animation complete
            float attackSpeed = 3;

            skinMaterial.color = Color.red;

            bool hasAppliedDamage = false;

            while (percent <= 1)
            {
                percent += Time.deltaTime * attackSpeed;

                if(percent >= .5  && !hasAppliedDamage)
                {
                    hasAppliedDamage = true;
                    targetEntity.TakeDamage(damage);
                }

                //Parabola equasion with percent as X. This makes interpolation (y) start at 0 and go to 1 and then back to 0.
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                //Vector3.Lerp provides a Vector3 value between two Vector3s, where the third paramter defines the percent of the way from point A to B
                transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

                yield return null;
            }
            skinMaterial.color = originalColor;
            currentState = State.Chasing;
            pathfinder.enabled = true;
        }

        IEnumerator UpdatePath()
        {
            float refreshRate = .25f;

            while (hasTarget)
            {
                if (currentState == State.Chasing)
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized; //Direction of the target
                    Vector3 targetPos = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2); //Target position minus the length of how thicc we are (in the right direction)
                    if (!dead)
                    { //Make sure code doesn't run after the enemy was killed. Important for coroutines to make sure the object state is still desirable.
                        pathfinder.SetDestination(targetPos);
                    }
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
