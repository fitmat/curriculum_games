using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerAIController : MonoBehaviour
    {
        // variables hierarchy
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 0f;
        [SerializeField] PetrolPath petrolPath = null;
        [SerializeField] float WaypointTolerance = 1f;
        [Range(0, 1)]
        [SerializeField] float petrolSpeedFraction = 0.2f;

        public bool runToggle = false;

        Fighter fighter;
        Health health;
        CreateMover mover;
        //GameObject enemy;
        //GameObject[] enemies;
        NavMeshAgent navMeshAgent;

        Vector3 guardPosition;
        float timeSinceLastSawEnemy = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        float traveledDistance = 0;
        Vector3 lastPosition;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            //enemies = GameObject.FindGameObjectsWithTag("EnemyOBJ");
            mover = GetComponent<CreateMover>(); ;

            guardPosition = transform.position;
            lastPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

           /* if (InAttackRangeOfEnemy() && fighter.CanAttack(enemy))
            {
                AttackBehaviour();
            } else */

            if (timeSinceLastSawEnemy < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PetrolBehaviour();
            }

            UpdateTimers();
            CalculateDistance();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawEnemy += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PetrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (petrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                navMeshAgent.isStopped = false;
                mover.StartMoveAction(nextPosition, petrolSpeedFraction);
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
                if (navMeshAgent.isActiveAndEnabled)
                {
                    navMeshAgent.isStopped = true;
                }
            }

            /*if (runToggle)
            {
                navMeshAgent.isStopped = false;
                mover.StartMoveAction(nextPosition, petrolSpeedFraction);
            }
            else if (!runToggle)
            {
                if (navMeshAgent.isActiveAndEnabled)
                {
                    navMeshAgent.isStopped = true;
                }
            } */
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < WaypointTolerance;
        }

        private void CycleWayPoint()
        {
            currentWaypointIndex = petrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return petrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawEnemy = 0;
            //fighter.Attack(enemy);
        }

        private void CalculateDistance ()
        {
            traveledDistance += Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;
            //Debug.Log("travelDist : " + (int)traveledDistance);
        }

        /*private bool InAttackRangeOfEnemy()
        {
            foreach (GameObject e in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, e.transform.position);

                if (distanceToEnemy < chaseDistance)
                {
                    enemy = e;
                    return true;
                }
            }

            return false;
        } */

        //called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
