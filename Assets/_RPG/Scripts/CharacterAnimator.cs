using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG
{
    public class CharacterAnimator : MonoBehaviour
    {

        const float locomotionAnimationSmoothTime = .1f;
        Animator animator;
        NavMeshAgent agent;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            //The speed percent is the agents current speed divided by the maximum speed
            float speedPercent = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("speedPercent", speedPercent, .1f, Time.deltaTime); //Set the sped percent to our speed percent var, and dampen it so it takes .1 seconds of smoothing between values
        }
    }
}
