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
        void TakeHit(float damage, RaycastHit hit);
        void TakeDamage(float damage);
    }
}