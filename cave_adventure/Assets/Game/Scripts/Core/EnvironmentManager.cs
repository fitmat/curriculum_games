using UnityEngine;

namespace RPG.Core
{
    public class EnvironmentManager : MonoBehaviour
    {
        // required variables
        [SerializeField] Transform spawnPos;

        private WorldBuilder wb;

        private void Start()
        {
            wb = WorldBuilder.Instance;

            SpawnEnvironment();
        }

        private void SpawnEnvironment()
        {
            wb.SpawnFromPool("enOne", spawnPos.position, spawnPos.rotation);
        }
    }
}
