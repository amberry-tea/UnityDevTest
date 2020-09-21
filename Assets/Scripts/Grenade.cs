using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public GameObject explosionEffect;

    public float delay = 3f;
    public float radius = 5f;
    public float force = 700f;

    float countdown;
    bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        //We could do Invoke("Explode", delay); but why not make things complicated
        countdown -= Time.deltaTime; //reduce by time since last frame
        if(countdown <= 0 && !hasExploded){
            Explode();
            hasExploded = true;
        }
    }

    public void Explode(){
        //Debug.Log("Boom");
        //Show explosion effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //Get nesarby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider nearbyObject in colliders){
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            //Add force
            if(rb != null){
                rb.AddExplosionForce(force, transform.position, radius);
            }

            

            //Damage
            
        }
        
        //Remove grenade
        Destroy(gameObject);
    }
}
