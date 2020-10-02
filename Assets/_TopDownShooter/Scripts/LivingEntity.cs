using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter{
    public class LivingEntity : MonoBehaviour, IDamageable
    {
        public float startingHealth;

        protected float health; //protected keeps it only avaliable to child classes
        protected bool dead;

        public event System.Action OnDeath;

        protected virtual void Start(){
            health = startingHealth;
        }

        public void TakeHit(float damage, RaycastHit hit){
            //Do stuff with hit variable, eg particle effects and knockback
            TakeDamage(damage);
        }

        public void TakeDamage(float damage){
            health -= damage;

            if(health <= 0 && !dead)
                Die();
        }

        [ContextMenu("Self Destruct")]
        protected void Die(){
            dead = true;
            if(OnDeath != null){
                OnDeath(); //Broadcast an event of death
            }
            GameObject.Destroy(gameObject);
        }
    }
}
