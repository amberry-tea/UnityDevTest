using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public GameObject pickupEffect;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player")){
            Pickup();
        }
    }

    private void Pickup()
    {
        //Debug.Log("PowerUp Picked Up");
        
        //Spawn a cool effect
        Instantiate(pickupEffect, transform.position, transform.rotation);

        //Apply effect to player

        //Remove power up
        Destroy(gameObject);
    }
}
