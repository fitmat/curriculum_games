using Cinemachine;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class CreateMover : MonoBehaviour, IAction, ISaveable
    {
        // variables hierarchy
        [SerializeField] float maxSpeed = 10f;
        [SerializeField] GameObject character;

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

        public void StartMoveAction (Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            navMeshAgent.enabled = true;
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

                playerBody.isKinematic = false;
                playerBody.useGravity = true;

                TriggerJump();
                Vector3 jumpDir = new Vector3(0f, 1f, 1f) * 5f;

                playerBody.AddRelativeForce(jumpDir, ForceMode.Impulse);

                Invoke("EnableNavAgent", 1f);
            }
        }

        public void JumpClick()
        {
            navMeshAgent.enabled = false;

            playerBody.isKinematic = false;
            playerBody.useGravity = true;

            Vector3 jumpDir = new Vector3(0f, 1f, 1f) * 5f;

            TriggerJump();
            playerBody.AddRelativeForce(jumpDir, ForceMode.Impulse);

            Invoke("EnableNavAgent", 1f);
        }

        private void EnableNavAgent()
        {
            playerBody.isKinematic = true;
            playerBody.useGravity = false;
            navMeshAgent.enabled = true;
            navMeshAgent.ResetPath();
        }

        private void TriggerJump()
        {
            GetComponent<Animator>().SetTrigger("jump");
            GetComponent<Animator>().ResetTrigger("stopJump");
            //GetComponent<Animator>().SetTrigger("animation_4");
        }

        private void TriggerStopJump()
        {
            GetComponent<Animator>().ResetTrigger("jump");
            //GetComponent<Animator>().ResetTrigger("animation_4");
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

        /* Dictionary approch
        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);

            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;

            GetComponent<NavMeshAgent>().enabled = false;

            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();

            GetComponent<NavMeshAgent>().enabled = true;
        } */

        /* normal approch
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
        } */

        /* struct approach
        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);

            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;

            GetComponent<NavMeshAgent>().enabled = false;

            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();

            GetComponent<NavMeshAgent>().enabled = true;
        } */
    }
}
