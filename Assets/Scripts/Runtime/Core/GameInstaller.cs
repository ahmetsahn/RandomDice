using System.Collections.Generic;
using Runtime.Core.Pool;
using Runtime.EnemySystem.View;
using Runtime.Signal;
using Runtime.SpawnerSystem;
using UnityEngine;
using Zenject;

namespace Runtime.Core
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSignals();
        }

        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<StartSpawnEnemySignal>();
            Container.DeclareSignal<SpawnDefenderRandomLocationSignal>();
            Container.DeclareSignal<DestroyHealthIconSignal>();
            Container.DeclareSignal<AddDefenderToListSignal>();
            Container.DeclareSignal<RemoveDefenderFromListSignal>();
            Container.DeclareSignal<ShowMergeableDefendersSignal>();
            Container.DeclareSignal<MergeDefendersSignal>();
            Container.DeclareSignal<SpawnMergedDefenderSignal>();
            Container.DeclareSignal<AddEnemyToListSignal>();
            Container.DeclareSignal<RemoveEnemyFromListSignal>();
            Container.DeclareSignal<StartDefenderAttackSignal>();
            Container.DeclareSignal<EnemyListEmptySignal>();
            Container.DeclareSignal<SetNewDefenderAttackTargetSignal>();
            Container.DeclareSignal<IncreaseCurrentEnergySignal>();
            Container.DeclareSignal<UpgradeDefenderSignal>();
            Container.DeclareSignal<UpdateNewDefenderUpgradeSignal>();
            Container.DeclareSignal<ReduceCurrentEnergySignal>();
            Container.DeclareSignal<UpdateUpgradeDefenderButtonStateSignal>();
        }
    }
}