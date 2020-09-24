using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public GameObject pickupEffect;
    public float duration = 4f;
    //public int healthBoost = 20;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player")){
            StartCoroutine(Pickup(other));
        }
    }

    private IEnumerator Pickup(Collider player)
    {
        //Debug.Log("PowerUp Picked Up");
        
        //Spawn a cool effect
        Instantiate(pickupEffect, transform.position, transform.rotation);
        //~~~~~~~~~~~~~~~~~~~~~~
        //Apply effect to player
        //~~~~~~~~~~~~~~~~~~~~~~

        player.transform.localScale *= 1.1f; //Make the player bigger by 50%
        yield return new WaitForSeconds(.1f);
        player.transform.localScale /= 1.1f;
        yield return new WaitForSeconds(.1f);
        player.transform.localScale *= 1.2f;
        yield return new WaitForSeconds(.1f);
        player.transform.localScale /= 1.2f;
        yield return new WaitForSeconds(.1f);
        player.transform.localScale *= 1.3f;
        yield return new WaitForSeconds(.1f);
        player.transform.localScale /= 1.3f;
        yield return new WaitForSeconds(.1f);
        player.transform.localScale *= 1.5f;

        //PlayerStats stats = player.GetComponent<PlayerStats>();
        //stats.health += healthBoost;

        //Make the powerup invisible as it has been collected
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        //Wait a time
        yield return new WaitForSeconds(duration);

        //Reverse effect on player
        player.transform.localScale /= 1.2f; //Make the player smaller by 50%
        yield return new WaitForSeconds(.1f);
        player.transform.localScale *= 1.2f;
        yield return new WaitForSeconds(.1f);
        player.transform.localScale /= 1.3f;
        yield return new WaitForSeconds(.1f);
        player.transform.localScale *= 1.3f;
        yield return new WaitForSeconds(.1f);
        player.transform.localScale /= 1.4f;
        yield return new WaitForSeconds(.1f);
        player.transform.localScale *= 1.4f;
        yield return new WaitForSeconds(.1f);
        player.transform.localScale /= 1.5f;

        //Remove power up
        Destroy(gameObject);
    }
}
