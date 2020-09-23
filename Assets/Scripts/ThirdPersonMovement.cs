using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float gravity = 9.8f;
    public float jumpSpeed = 8;
    public float drag = 12f;

    private float turnSmoothVelocity;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        //A raw vector 3 for player movement:
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveDir = Vector3.zero;

        //~~~~~~~~~
        //MOVEMENT:
        //~~~~~~~~~

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
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //When we move the character, normalize his movement direction!
            //Then, multiply by speed and deltaTime
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        
        //~~~~~~~~
        //JUMPING:
        //~~~~~~~~

        float friction = 0;

        if(controller.isGrounded){
            velocity.y = 0; //no Y velocity on the ground
            friction = drag * 3;
            if (Input.GetKeyDown("space")){
                velocity.y = jumpSpeed; //jump
            }
        }

        //~~~~~~~~
        //PHYSICS:
        //~~~~~~~~

        //apply gravity acceleration to vertical speed
        velocity.y -= gravity * Time.deltaTime;

        //Apply drag to the player (This is a bit jank but it works I guess)
        velocity.x -= (drag + friction) * System.Math.Sign(velocity.x) * Time.deltaTime;
        velocity.z -= (drag + friction) * System.Math.Sign(velocity.z) * Time.deltaTime;
        //Prevent the drag from flipping inbetween positive and negative numbers
        if(System.Math.Abs(velocity.x) <= 1f) velocity.x = 0;
        if(System.Math.Abs(velocity.z) <= 1f) velocity.z = 0;

        //convert vel to displacement and Move the character
        controller.Move(velocity * Time.deltaTime);        
    }

    public void AddExplosionForce(Vector3 direction, float speed){
        velocity = direction * speed;
    }
}
