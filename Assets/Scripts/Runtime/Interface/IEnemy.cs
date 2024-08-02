using System;
using Runtime.Enum;
using UnityEngine;

namespace Runtime.Interface
{
    public interface IEnemy
    {
        public int Health { get; set; }
        public Transform Transform { get; }
        public Action<int> TakeDamageEvent { get; set; }
        
        public EnemyType EnemyType { get; set; }
    }
}