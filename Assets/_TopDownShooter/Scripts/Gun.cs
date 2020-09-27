using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    public class Gun : MonoBehaviour
    {
        public Transform muzzle;
        public Projectile projectile;
        public float msBetweenShots = 100;
        public float muzzleVelocity = 35; //speed that bullet leaves the gun

        float nextShotTime;

        public void Shoot()
        {
            if (Time.time > nextShotTime) //Only shoot when the time passes current time + 100ms from last shot time
            { 
                nextShotTime = Time.time + msBetweenShots / 1000; //Divide by 1000 to get ms from s
                Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
                newProjectile.SetSpeed(muzzleVelocity);
            }
        }


    }
}
