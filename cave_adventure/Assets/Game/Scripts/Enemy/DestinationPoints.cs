using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Enemy
{
    public class DestinationPoints : MonoBehaviour
    {
        //required variables
        [SerializeField] Transform[] destinations;

        public Transform getDestinationsOne()
        {
            return destinations[0];
        }

        public Transform getDestinationsTwo()
        {
            return destinations[1];
        }

        public Transform getDestinationsThree()
        {
            return destinations[2];
        }
    }
}