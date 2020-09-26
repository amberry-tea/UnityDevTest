using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Grenade : MonoBehaviour
{

    public GameObject explosionEffect;
    private CinemachineImpulseSource cinemachineImpulseSource;

    public float delay = 3f;
    public float radius = 5f;
    public float force = 700f;

    float countdown;
    bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //We could do Invoke("Explode", delay); but why not make things complicated
        countdown -= Time.deltaTime; //reduce by time since last frame
        if(countdown <= 0 && !hasExploded){
            Explode();
            cinemachineImpulseSource.GenerateImpulse();
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
            Rigidbody rb;

            //Add force
            if(nearbyObject.tag == "Player")
            {
                //Math here should be reviewed to make sure its all right
                //Computation should be localized to ThirdPersonMovement
                Vector3 dir = nearbyObject.transform.position - transform.position;
                float speed = ((radius - dir.magnitude) / radius) * force;
                Debug.Log("Player was launched at speed of:" + speed);
                nearbyObject.GetComponent<ThirdPersonMovement>().AddExplosionForce(dir.normalized, speed/50);
            } 
            else if((rb = nearbyObject.GetComponent<Rigidbody>()) != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            

            //Damage
            
        }
        
        //Remove grenade
        Destroy(gameObject);
    }
}
