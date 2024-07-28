using Runtime.Core.Pool;
using UnityEngine;

namespace Runtime.Misc.Particle
{
    public class ReturnParticleToPool : MonoBehaviour
    {
        private void OnParticleSystemStopped()
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}