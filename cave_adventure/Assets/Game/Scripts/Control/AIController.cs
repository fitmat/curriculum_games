//using RPG.Combat;
using RPG.Combat;
using RPG.Core;
using RPG.Enemy;
using RPG.Movement;
using System;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        // variables hierarchy
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float timeBetAttacks = 10f;

        [SerializeField] PetrolPath petrolPath = null;

        [SerializeField] float WaypointTolerance = 1f;
        [SerializeField] float WaypointSwellTime = 3f;
        [Range(0, 1)]
        [SerializeField] float petrolSpeedFraction = 0.2f;

        //Fighter fighter;
        EnemyHealth health;
        EnemyMover mover;
        GameObject player;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            //fighter = GetComponent<Fighter>();
            health = GetComponent<EnemyHealth>();
            player = GameObject.FindWithTag("Player");
            mover = GetComponent<EnemyMover>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (health.IsDead()) return;

            // AttackBehaviour();

            PetrolBehaviour();

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PetrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (petrolPath != null)
            {
                if (AtWaypoint())
                {
                    AttackBehavouir();

                    timeSinceArrivedAtWaypoint = 0;
                    CycleWayPoint();
                        
                    // do when enemy is in front of the player
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > WaypointSwellTime)
            {
                mover.StartMoveAction(nextPosition);
            }
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

        private void AttackBehavouir()
        {
            transform.LookAt(player.transform);
            if (timeSinceLastAttack > timeBetAttacks)
            {
                // this will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool InAttackRangeOfPlayer ()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }

        //called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        private void TriggerStopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}
