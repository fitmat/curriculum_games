using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class EnemyMover : MonoBehaviour, IAction, ISaveable
    {
        // variables hierarchy
        [SerializeField] float maxSpeed = 10f;

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
            //navMeshAgent.enabled = !health.IsDead();

            JumpAbility();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed;
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
                //playerfollower.enabled = false;

                playerBody.isKinematic = false;
                playerBody.useGravity = true;
                //playerBody.AddForce(new Vector3(0f, 5f, 5f), ForceMode.Impulse);

                float zFactor = Mathf.Clamp01(transform.position.z);
                float xFactor = Mathf.Clamp01(transform.position.x);

                TriggerJump();
                playerBody.AddForce(new Vector3(xFactor, 5f, zFactor * 5f), ForceMode.Impulse);

                Invoke("EnableNavAgent", 1f);
            }
        }

        private void EnableNavAgent()
        {
            playerBody.isKinematic = true;
            playerBody.useGravity = false;

            //playerfollower.enabled = true;
            navMeshAgent.enabled = true;
        }

        private void TriggerJump()
        {
            GetComponent<Animator>().SetTrigger("jump");
            GetComponent<Animator>().ResetTrigger("stopJump");
        }

        private void TriggerStopJump()
        {
            GetComponent<Animator>().ResetTrigger("jump");
            GetComponent<Animator>().SetTrigger("stopJump");
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}