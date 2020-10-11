using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(PlayerMotor))]
    public class PlayerController : MonoBehaviour
    {
        public LayerMask movementMask;

        Camera cam;
        PlayerMotor motor;

        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main;
            motor = GetComponent<PlayerMotor>();
        }

        // Update is called once per frame
        void Update()
        {
            //Move to a position with leftclick
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, movementMask)) //Raycast 100 units and only on the movementMask layer
                {
                    //Debug.Log("We hit " + hit.collider.name + " " + hit.point, hit.collider.gameObject);
                    //Move our player to what we hit
                    motor.MoveToPoint(hit.point);

                    //Stop focusing on any object/action
                }
            }

            //Focus on an object with rightclick
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, movementMask))
                {
                    //Debug.Log("We hit " + hit.collider.name + " " + hit.point, hit.collider.gameObject);
                    //Check if we hit an interactable

                    //If we did, set as focus
                }
            }
        }
    }
}
