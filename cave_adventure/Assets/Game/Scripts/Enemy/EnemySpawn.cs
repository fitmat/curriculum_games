using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Enemy
{
    public class EnemySpawn : MonoBehaviour
    {
        // required variables
        [SerializeField] Transform[] destinationPoints;
        [SerializeField] GameObject EnemyPrefabs;

        GameObject player;
        GameObject enemy;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            
        }
    }
}
