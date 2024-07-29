using System;
using Cysharp.Threading.Tasks;
using Runtime.Signal;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.EnemySystem.Controller
{
    public class BossDestroyedController : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnDisable()
        {
            _signalBus.Fire(new BossDeadSignal());
        }
    }
}