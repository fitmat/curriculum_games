using Cinemachine;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class CreateMover : MonoBehaviour, IAction
    {
        // variables hierarchy
        [SerializeField] float maxSpeed = 10f;
        [SerializeField] CinemachineVirtualCamera playerfollower;

        NavMeshAgent navMeshAgent;
        Health health;
        Rigidbody playerBody;

        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            playerBody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();

            //JumpAbility();
            UpdateAnimator();
        }

        public void StartMoveAction (Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            //navMeshAgent.enabled = true;
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }

        private void JumpAbility()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                navMeshAgent.enabled = false;
                playerfollower.enabled = false;

                playerBody.isKinematic = false;
                playerBody.useGravity = true;
                playerBody.AddForce(new Vector3(0f, 5f, 5f), ForceMode.Impulse);

                Invoke("EnableNavAgent", 1f);
            }
        }

        private void EnableNavAgent()
        {
            playerBody.isKinematic = true;
            playerBody.useGravity = false;

            playerfollower.enabled = true;
            navMeshAgent.enabled = true;
        }
    }
}
