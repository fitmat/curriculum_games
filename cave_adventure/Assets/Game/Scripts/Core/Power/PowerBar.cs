using RPG.Control;
using RPG.Movement;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    public class PowerBar : MonoBehaviour
    {
        //required variables
        public Image powerBarI;

        [SerializeField] float damage = 20f;
        [SerializeField] GameObject[] Enemies;
        [SerializeField] GameObject powerOBJ;

        GameObject player;

        private float startPower = 0f;
        private float maxPower = 100f;
        public float currPower;

        public float increaseAmmount = 10;

        private float timer = 0f;

        private int i = 0;
        public int totalEnemies;

        private bool playerMoved = false;
        public bool powerAttack = false;

        // Start is called before the first frame update
        void Start()
        {
            currPower = startPower;
            totalEnemies = Enemies.Length;

            player = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            AttackPower();

            timer += Time.deltaTime;

            if (totalEnemies < 1 && !playerMoved)
            {
                playerMoved = true;
                MovePlayer();
            }
        }

        private void AttackPower()
        {
            if (totalEnemies <= 0)
            {
                currPower = startPower;
                powerBarI.fillAmount = currPower / maxPower;
                return;
            }

            if (timer > 0.5f)
            {
                if (Input.GetKey(KeyCode.V) || powerAttack)
                {
                    currPower = Mathf.Min(currPower + increaseAmmount, maxPower);
                    powerBarI.fillAmount = currPower / maxPower;

                    if (currPower == maxPower)
                    {
                        Enemies[i].GetComponent<EnemyHealth>().TakeDamage(damage);
                        currPower = startPower;
                    }
                }
                else
                {
                    currPower = Mathf.Max(currPower - increaseAmmount, startPower);
                    powerBarI.fillAmount = currPower / maxPower;
                }

                timer = 0f;
            }
        }

        public void ChangeEnemy()
        {
            i++;
            totalEnemies--;
        }

        private void MovePlayer()
        {
            powerOBJ.SetActive(false);
            player.GetComponent<PlayerAIController>().runToggle = true;
        }
    }
}
