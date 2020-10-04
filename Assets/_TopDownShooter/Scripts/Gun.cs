using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    [RequireComponent(typeof(Muzzleflash))]
    public class Gun : MonoBehaviour
    {

        public enum FireMode
        {
            Auto, Burst, Single
        };
        public FireMode fireMode;

        public Transform[] projectileSpawn;
        public Projectile projectile;
        public float msBetweenShots = 100;
        public float muzzleVelocity = 35; //speed that bullet leaves the gun
        public int burstCount;


        public Transform shell;
        public Transform shellEjection; //Point to spawn the shells from
        Muzzleflash muzzleflash;

        float nextShotTime;
        bool triggerReleasedSinceLastShot;
        int shotsRemainingInBurst;

        private void Start()
        {
            muzzleflash = GetComponent<Muzzleflash>();
            shotsRemainingInBurst = burstCount;
        }

        void Shoot()
        {
            if (Time.time > nextShotTime) //Only shoot when the time passes current time + 100ms from last shot time
            {

                if (fireMode == FireMode.Burst)
                {
                    if (shotsRemainingInBurst == 0)
                    {
                        return; //leave shoot method when no more shots in burst
                    }
                    shotsRemainingInBurst--;
                }
                else if (fireMode == FireMode.Single)
                {
                    if (!triggerReleasedSinceLastShot)
                    {
                        return; //leave the shoot method if trigger is not released, ie held down
                    }
                }

                for (int i = 0; i < projectileSpawn.Length; i++)
                {
                    nextShotTime = Time.time + msBetweenShots / 1000; //Divide by 1000 to get ms from s
                    Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                    newProjectile.SetSpeed(muzzleVelocity);
                }
                Instantiate(shell, shellEjection.position, shellEjection.rotation);
                muzzleflash.Activate();
            }
        }

        public void OnTriggerHold()
        {
            Shoot();
            triggerReleasedSinceLastShot = false;
        }

        public void OnTriggerRelease()
        {
            triggerReleasedSinceLastShot = true;
            shotsRemainingInBurst = burstCount;
        }
    }
}
