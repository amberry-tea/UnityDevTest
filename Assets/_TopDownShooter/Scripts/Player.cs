using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    /**
    * For getting the players inputs
    */
    [RequireComponent (typeof(PlayerController))]
    public class Player : MonoBehaviour
    {

        public float moveSpeed = 5;
        PlayerController controller;
        Camera viewCamera;

        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<PlayerController>();
            viewCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")); //GetAxisRaw is good!
            Vector3 moveVelocity = moveInput.normalized * moveSpeed;
            controller.Move(moveVelocity);

            //Draw a ray from the camera where the mouse is
            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            //Create a plane where the ground is. Dont bother getting the plane from the game, too problematic.
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero); //First vector tells where the plane should point, second vector is a point the vector should intersect (for tilt)
            float rayDistance;

            if(groundPlane.Raycast(ray, out rayDistance)){ //If ray intersects with groundplane, return true and give out the distance the ray traveled.
                Vector3 point = ray.GetPoint(rayDistance); //Get the point where the ray intersected the plane
                //Debug.DrawLine(ray.origin, point, Color.red);
                controller.LookAt(point);
            }
        }
    }
}