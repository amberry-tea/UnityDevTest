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
        public int projectilesPerMag;
        public float reloadTime = .3f;
        public float reloadAngle = 45;

        [Header("Recoil")]
        public Vector2 kickMinMax = new Vector2(0.05f, .2f); //x axis for min, y for max
        public Vector2 recoilAngleMinMax = new Vector2(3, 5); //x axis for min, y for max
        public float recoilMoveSettleTime = .1f; //the time it takes to return to baseline
        public float recoilRotationSettleTime = .1f; //the time it takes to return to baseline

        [Header("Effects")]
        public Transform shell;
        public Transform shellEjection; //Point to spawn the shells from
        Muzzleflash muzzleflash;

        float nextShotTime;
        bool triggerReleasedSinceLastShot;
        int shotsRemainingInBurst;
        int projectilesRemainingInMag;
        bool isReloading;

        Vector3 recoilSmoothDampVelocity; //The velocity from the SmoothDamp function used to animate recoil
        float recoilAngle;
        float recoilRotSmoothDampVelocity;

        private void Start()
        {
            muzzleflash = GetComponent<Muzzleflash>();
            shotsRemainingInBurst = burstCount;
            projectilesRemainingInMag = projectilesPerMag;
        }

        private void LateUpdate()
        { //Late update so that changes overwrite the gun aiming scripts
            //To animate recoil
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
            recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleTime);
            transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle; //v3.right for x axis negative

            if (!isReloading && projectilesRemainingInMag == 0)
            {
                Reload();
            }
        }

        void Shoot()
        {
            if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag > 0) //Only shoot when the time passes current time + 100ms from last shot time
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
                    if (projectilesRemainingInMag == 0)
                    {
                        break;
                    }
                    projectilesRemainingInMag--;

                    nextShotTime = Time.time + msBetweenShots / 1000; //Divide by 1000 to get ms from s
                    Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                    newProjectile.SetSpeed(muzzleVelocity);
                }
                Instantiate(shell, shellEjection.position, shellEjection.rotation);
                muzzleflash.Activate();

                transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y); //Gun kickback

                recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
                recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);
            }
        }

        public void Reload()
        {
            if (!isReloading && projectilesRemainingInMag != projectilesPerMag)
            {
                StartCoroutine("AnimateReload");
            }
        }

        IEnumerator AnimateReload()
        {
            isReloading = true;

            yield return new WaitForSeconds(0.2f);

            float reloadSpeed = 1f / reloadTime;
            float percent = 0;
            Vector3 initialRot = transform.localEulerAngles;
            float maxReloadAngle = reloadAngle;

            while (percent < 1)
            {
                percent += Time.deltaTime * reloadSpeed;

                //Animate the gun
                //Parabola equasion with percent as X. This makes interpolation (y) start at 0 and go to 1 and then back to 0.
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
                transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;

                yield return null;
            }

            isReloading = false;
            projectilesRemainingInMag = projectilesPerMag;
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

        public void Aim(Vector3 aimPoint)
        {
            if (!isReloading)
            {
                transform.LookAt(aimPoint);
            }
        }
    }
}
