using UnityEngine;

namespace portal
{
    public class PortalSpawnObject: MonoBehaviour
    {
        [SerializeField] private Portal _portalToSpawn;

        public Portal PortalToSpawn => _portalToSpawn;
    }
}