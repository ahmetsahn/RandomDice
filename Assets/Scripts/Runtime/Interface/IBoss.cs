using System;
using UnityEngine;

namespace Runtime.Interface
{
    public interface IBoss
    {
        public Transform Transform { get; }
        
        public Action<int> SetHealthEvent { get; }
    }
}