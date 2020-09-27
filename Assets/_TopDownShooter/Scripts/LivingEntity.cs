using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter{
    public class LivingEntity : MonoBehaviour, IDamageable
    {
        public float startingHealth;

        protected float health; //protected keeps it only avaliable to child classes
        protected bool dead;

        protected virtual void Start(){
            health = startingHealth;
        }

        public void TakeHit(float damage, RaycastHit hit){
            health -= damage;

            if(health <= 0 && !dead)
                Die();
        }

        protected void Die(){
            dead = true;
            GameObject.Destroy(gameObject);
        }
    }
}
