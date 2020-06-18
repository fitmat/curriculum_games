using UnityEngine;
using UnityEngine.UI;

namespace RPG.Unleash
{
    public class UnleashWeapon : MonoBehaviour
    {
        //required variables
        [SerializeField] Text Timetext;

        float timer = 0f;

        // Start is called before the first frame update
        void Start()
        {
            Timetext.text = "";
        }

        // Update is called once per frame
        void Update()
        {
            RotateWeapon();
            CalculateTime();
        }

        private void RotateWeapon()
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
        }

        private void CalculateTime()
        {
            timer += Time.deltaTime;
            Timetext.text = ((int)timer).ToString();
        }
    }
}
