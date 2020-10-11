using UnityEngine;

namespace BoxGame
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody rb;
        public float movementSpeed = 50f;

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Input.GetKey("w"))
            {
                rb.AddForce(movementSpeed * Time.deltaTime, 0, 0, ForceMode.Impulse);
            }
            if (Input.GetKey("a"))
            {
                rb.AddForce(0, 0, movementSpeed * Time.deltaTime, ForceMode.Impulse);
            }
            if (Input.GetKey("s"))
            {
                rb.AddForce(-movementSpeed * Time.deltaTime, 0, 0, ForceMode.Impulse);
            }
            if (Input.GetKey("d"))
            {
                rb.AddForce(0, 0, -movementSpeed * Time.deltaTime, ForceMode.Impulse);
            }
            if (Input.GetKey("space"))
            {
                rb.AddForce(0, movementSpeed * Time.deltaTime, 0, ForceMode.Impulse);
            }
        }
    }
}
