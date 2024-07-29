using System;
using System.Collections.Generic;
using Runtime.EnemySystem.View;

namespace Runtime.SpawnerSystem
{
    [Serializable]
    public struct WaveData
    {
        public List<EnemyViewModel> Enemies;
        
        public List<float> SpawnInterval;
        public List<float> WaitingTimeBetweenEnemyTypes;
        
        public List<int> SpawnCount;
        
        public int WaitingTimeAfterWaveEnds;

        public EnemyViewModel Boss;
    }
}