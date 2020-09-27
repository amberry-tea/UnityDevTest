﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    public class Projectile : MonoBehaviour
    {

        float speed = 10;
        float damage = 1;
        public LayerMask collisionMask; //Determine which layers the projectile can collide with

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        void Update()
        {
            float moveDistance = speed * Time.deltaTime; //How far it has moved since the last frame
            CheckCollisions(moveDistance);
            transform.Translate(Vector3.forward * moveDistance); //Move
        }

        /**
        * Check if the bullet hits anything
        */
        void CheckCollisions(float moveDistance)
        {
            Ray ray = new Ray(transform.position, transform.forward); //A ray infront of the bullet to calculate if it will hit anything
            RaycastHit hit;

            //QueryTriggerInteraction determines if the raycast should also check trigger objects
            //We're checking for triggers since we've set the enemies to triggers
            if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
            {
                OnHitObject(hit);
            }
        }

        void OnHitObject(RaycastHit hit)
        {
            IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
            if(damageableObject != null){
                damageableObject.TakeHit(damage, hit);
            }
            print(hit.collider.gameObject.name);
            GameObject.Destroy(gameObject);
        }
    }
}
