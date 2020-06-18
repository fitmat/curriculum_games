using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class Environment : MonoBehaviour, IPooledEnviron
    {
        public void onObjectSpawn()
        {
            gameObject.SetActive(true);
        }
    }
}
