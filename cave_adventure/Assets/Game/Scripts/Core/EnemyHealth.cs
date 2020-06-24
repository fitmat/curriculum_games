using RPG.Combat;
using RPG.Saving;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    public class EnemyHealth : MonoBehaviour
    {
        // variables hierarchy
        [SerializeField] float StartHealthPoints = 100f;
        [SerializeField] GameObject attackEffect;
        private float healthPoints;

        [Header("UI Stuff")]
        public Image healthBar;
        public Text damageAmmount;

        private bool isDead = false;

        private PowerBar pb;

        private void Start()
        {
            pb = FindObjectOfType<PowerBar>();

            healthPoints = StartHealthPoints;
            setDamagetextNull();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            Instantiate(attackEffect, transform.position, Quaternion.identity);

            healthPoints = Mathf.Max(healthPoints - damage, 0);

            healthBar.fillAmount = healthPoints / StartHealthPoints;
            damageAmmount.text = damage.ToString();
            Invoke("setDamagetextNull", 4f);

            if (healthPoints == 0)
            {
                pb.ChangeEnemyAttack();
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void setDamagetextNull()
        {
            damageAmmount.text = "";
        }

        public float getHealthPoints()
        {
            return healthPoints;
        }

        // saving system
        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints == 0)
            {
                Die();
            }
        }
    }
}
