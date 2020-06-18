using RPG.Saving;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        // variables hierarchy
        [SerializeField] float healthPoints = 100f;

        private bool isDead = false;

        public bool IsDead ()
        {
            return isDead;
        }

        public void TakeDamage (float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0)
            {
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
