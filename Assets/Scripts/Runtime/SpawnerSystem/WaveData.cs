﻿using System;
using System.Collections.Generic;
using Runtime.EnemySystem.View;
using UnityEngine;

namespace Runtime.SpawnerSystem
{
    [Serializable]
    public struct WaveData
    {
        public List<EnemyViewModel> Enemies;
        
        public List<int> SpawnCount;
        
        public List<float> SpawnInterval;
        public List<float> WaitingTimeBetweenEnemyTypes;

        [HideInInspector] 
        public string WaveTime;
        
        public float WaitingTimeBossSpawn;
        
        public EnemyViewModel Boss;
        
        public int WaitingTimeAfterWaveEnds;
    }
}