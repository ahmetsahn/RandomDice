using System;
using Runtime.Enum;
using UnityEngine;

namespace Runtime.Interface
{
    public interface IEnemy
    {
        public Transform Transform { get; }
        public Action<int> TakeDamageEvent { get; set; }
    }
}