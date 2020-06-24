using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class battleModeChanger : MonoBehaviour
    {
        // required variables
        private PowerBar pb;

        private void Start()
        {
            pb = FindObjectOfType<PowerBar>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                pb.attackArea = false;
            }
        }
    }
}
