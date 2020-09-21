using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;
    public Rigidbody rb;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float gravity = 9.8f;
    public float jumpSpeed = 8;

    private float turnSmoothVelocity;
    private float vSpeed = 0f;

    void Update()
    {
        //A raw vector 3 for player movement:
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //If the player is moving... (Deadzone of 0.1f)
        if(direction.magnitude >= 0.1f) 
        {
            //Atan2 gets the angle between 0 degrees and a coordinate point.
            //We multiply by Rad2Deg to turn Atan2 to degrees
            //Then, we add eulerAngle Y to adjust it from calculating the angle from 0 degrees to calculating from 90 degrees
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //Smooth damp angle is a great function to use. Look at documentation.
            //There are likely other smooth functions which will come in handy (eg. linear number systems)
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //Sets the rotation of the Y axis to the angle we calculated and smoothed.
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //moveDir calculates the angle to adjust our movemen at.
            //We set this angle to be adjusted along the Y axis by the target angle we calculated above.
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //When we move the character, normalize his movement direction!
            //Then, multiply by speed and deltaTime
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        
        //~~~~~~~~
        //JUMPING:
        //~~~~~~~~

        if(controller.isGrounded){
            vSpeed = 0; //no Y velocity on the ground
            if (Input.GetKeyDown("space")){
                vSpeed = jumpSpeed; //jump
            }
        }

        //apply gravity acceleration to vertical speed
        vSpeed -= gravity * Time.deltaTime;
        Vector3 vel = new Vector3(0, vSpeed, 0);

        //convert vel to displacement and Move the character
        controller.Move(vel * Time.deltaTime);
    }

    public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius){
        
    }
}
