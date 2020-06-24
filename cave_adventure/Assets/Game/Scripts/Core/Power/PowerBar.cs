using RPG.Control;
using RPG.Movement;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    public class PowerBar : MonoBehaviour
    {
        //required variables
        public Image powerDefenceBarI;
        public Image powerAttackBarI;

        [SerializeField] float damage = 20f;
        [SerializeField] GameObject[] AttackEnemies;
        [SerializeField] GameObject[] DefenceEnemies;

        [SerializeField] Transform[] petrolPathsWaypoints;
        [SerializeField] Transform[] DefencePositions;

        [SerializeField] GameObject powerDefenceOBJ;
        [SerializeField] GameObject powerAttackOBJ;

        GameObject player;

        private float startPower = 0f;
        private float maxPower = 300f;
        public float currPowerDefence;
        public float currPowerAttack;

        public float increaseAmmount = 10;

        private float timer = 0f;

        private int enemyIndex = 0;
        private int defenceIndex = 0;
        private int movePositionIndex = 0;

        public int totalAttackEnemies;
        public int totalDefenceEnemies;

        private bool playerMoved = false;
        public bool powerAttack = false;

        public bool attackArea;

        private AIController aic;

        // Start is called before the first frame update
        void Start()
        {
            currPowerDefence = startPower;
            currPowerAttack = maxPower;

            totalAttackEnemies = AttackEnemies.Length;
            totalDefenceEnemies = DefenceEnemies.Length;

            powerDefenceOBJ.SetActive(false);

            aic = FindObjectOfType<AIController>();
            player = GameObject.FindWithTag("Player");

            player.GetComponent<PlayerAIController>().runToggle = false;

            attackArea = true;

            for (int j = 1; j < totalAttackEnemies; j++)
            {
                AttackEnemies[j].SetActive(false);
            }

            for (int k = 0; k < totalAttackEnemies; k++)
            {
                DefenceEnemies[k].SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            DefencePower();
            AttackPower();

            timer += Time.deltaTime;

            if (totalAttackEnemies < 1 && !playerMoved)
            {
                playerMoved = true;
                MovePlayer();
            }
        }

        private void AttackPower()
        {
            if (totalAttackEnemies <= 0)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                AttackEnemies[enemyIndex].GetComponent<EnemyHealth>().TakeDamage(damage);

                currPowerAttack = Mathf.Max(currPowerAttack - damage, 0);

                powerAttackBarI.fillAmount = currPowerAttack / maxPower;
            }
        }

        private void DefencePower()
        {
            if (totalDefenceEnemies <= 0)
            {
                currPowerDefence = startPower;
                powerDefenceBarI.fillAmount = currPowerDefence / maxPower;
                return;
            }

            if (timer > 0.5f)
            {
                if (Input.GetKey(KeyCode.V) || powerAttack)
                {
                    currPowerDefence = Mathf.Min(currPowerDefence + increaseAmmount, maxPower);
                    powerDefenceBarI.fillAmount = currPowerDefence / maxPower;

                    if (currPowerDefence == maxPower)
                    {
                        //DefenceEnemies[defenceIndex].GetComponent<EnemyHealth>().TakeDamage(damage);
                        moveEnemy(movePositionIndex);
                        movePositionIndex++;
                        currPowerDefence = startPower;
                    }
                }
                else
                {
                    currPowerDefence = Mathf.Max(currPowerDefence - increaseAmmount, startPower);
                    powerDefenceBarI.fillAmount = currPowerDefence / maxPower;
                }

                timer = 0f;
            }
        }

        public void ChangeEnemyAttack()
        {
            enemyIndex++;
            if (enemyIndex < AttackEnemies.Length)
            {
                AttackEnemies[enemyIndex].SetActive(true);
            }

            totalAttackEnemies--;
        }

        public void ChangeEnemyDefence()
        {
            defenceIndex++;
            if (defenceIndex < DefenceEnemies.Length)
            {
                DefenceEnemies[defenceIndex].SetActive(true);
            }

            totalDefenceEnemies--;
        }

        private void MovePlayer()
        {
            powerAttackOBJ.SetActive(false);
            powerDefenceOBJ.SetActive(true);

            player.GetComponent<PlayerAIController>().runToggle = true;
            DefenceEnemies[defenceIndex].SetActive(true);
        }

        private void moveEnemy(int positionIndex)
        {
            petrolPathsWaypoints[defenceIndex].position = DefencePositions[positionIndex].position;
            DefenceEnemies[defenceIndex].GetComponent<EnemyMover>().StartMoveAction(DefencePositions[positionIndex].position);
            ChangeEnemyDefence();
        }
    }
}
