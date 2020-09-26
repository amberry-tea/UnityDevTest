using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Camera shaking effects can be done using Cinnemachine.
* This code offers a janky alternative which doesn't work
* as well but it was fun coroutine practice.
*
* To work, camera must be under an empty game object so that
* camera movement is relative to that game objects space. This
* is because the x and y values are not adjusted for the camera
* positioning.
*/
public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition; //local position for a position relative to parent

        float elapsed = 0.0f;

        while (elapsed < duration) //while time is less than our duration
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null; //wait until next frame
        }

        transform.localPosition = originalPos; //put camera back
    }
}
