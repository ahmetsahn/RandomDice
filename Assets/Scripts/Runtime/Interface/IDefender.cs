using System;
using Runtime.Enum;
using UnityEngine;

namespace Runtime.Interface
{
    public interface IDefender
    {
        public DefenderType DefenderType { get; }
     
        public Transform Transform { get; }
        
        public Vector3 InitialPosition { get; }
        
        public int Level { get; }
        
        public Action SetDefaultColorEvent { get; }
        public Action SetUnMergeableColorEvent { get; }
        
        public Action SetDefaultLevelEvent { get; }
        public Action<int> LevelUpEvent { get; }
        
        public Action<int> UpgradeDefenderEvent { get; }
        
        public Action<int> UpgradeNewDefenderEvent { get; }
        
    }
}