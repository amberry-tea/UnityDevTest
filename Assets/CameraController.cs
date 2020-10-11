using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;

        public Vector3 offset;
        public float zoomSpeed = 4f;
        public float minZoom = 5f;
        public float maxZoom = 15f;

        public float pitch = 2f;
        public float yawSpeed = 100f;

        private float currentZoom = 10f;
        private float currentYaw = 0f;

        private void Update() {
            //Inverted by default, so we apply the -= operator
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            //Move the camera around with A and D
            currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        }

        private void LateUpdate()
        {
            //Move/aim the camera
            transform.position = target.position - offset * currentZoom;
            transform.LookAt(target.position + Vector3.up * pitch); //The players position is actually at his feet, so we move it up

            //Rotate the camera left and right
            transform.RotateAround(target.position, Vector3.up, currentYaw);
        }
    }
}
