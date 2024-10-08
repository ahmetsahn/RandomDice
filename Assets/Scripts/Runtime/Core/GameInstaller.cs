﻿using Runtime.Signal;
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
            Container.DeclareSignal<IncreaseCurrentEnergySignal>();
            Container.DeclareSignal<UpgradeDefenderSignal>();
            Container.DeclareSignal<UpdateNewDefenderUpgradeSignal>();
            Container.DeclareSignal<ReduceCurrentEnergySignal>();
            Container.DeclareSignal<UpdateUpgradeDefenderButtonStateSignal>();
            Container.DeclareSignal<IsDefenderSpawnSlotListFullSignal>();
            Container.DeclareSignal<UpdateSpawnDefenderButtonStateSignal>();
            Container.DeclareSignal<BossSequenceSignal>();
            Container.DeclareSignal<BossDeadSignal>();
            Container.DeclareSignal<IsEnemyMoveableSignal>();
            Container.DeclareSignal<SetDamagePopupTextSignal>();
            Container.DeclareSignal<IncreaseWaveSignal>();
            Container.DeclareSignal<ResumeTimerSignal>();
            Container.DeclareSignal<EnableBinSpriteSignal>();
            Container.DeclareSignal<AddToEmptyDefenderSpawnSlotListSignal>();
            Container.DeclareSignal<SetTargetSignal>();
            Container.DeclareSignal<SetTargetForNewDefenderSignal>();
        }
    }
}