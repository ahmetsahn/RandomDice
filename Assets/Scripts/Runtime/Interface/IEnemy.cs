using System;
using UnityEngine;

namespace Runtime.Interface
{
    public interface IEnemy
    {
        public Transform Transform { get; }
        public Action<int> OnTakeDamage { get; set; }
    }
}