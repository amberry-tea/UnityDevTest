using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    /**
    * Interface for applying damage
    */
    public interface IDamageable
    {
        void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection);
        void TakeDamage(float damage);
    }
}