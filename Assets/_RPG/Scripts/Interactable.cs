
using UnityEngine;

//Base class for all interactable objects
namespace RPG
{
    public class Interactable : MonoBehaviour
    {
        //How close to be to take action
        public float radius = 3f;
        public Transform interactionTransform;

        bool isFocus = false;
        Transform player;

        bool hasInteracted = false;

        public virtual void Interact()
        {
            Debug.Log("Interacting With" + transform.name);
        }

        void Update()
        {
            if (isFocus && !hasInteracted)
            {
                float distance = Vector3.Distance(player.position, interactionTransform.position);
                if (distance <= radius)
                {
                    Debug.Log("Interact");
                    hasInteracted = true;
                }
            }
        }

        public void OnFocused(Transform playerTransform)
        {
            isFocus = true;
            player = playerTransform;
            hasInteracted = false;
        }

        public void OnDefocused()
        {
            isFocus = false;
            player = null;
            hasInteracted = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionTransform.position, radius);
        }
    }
}
