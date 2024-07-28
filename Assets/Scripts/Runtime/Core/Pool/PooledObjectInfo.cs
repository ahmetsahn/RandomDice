using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Core.Pool
{
    public class PooledObjectInfo
    {
        public string LookupString;
        public readonly List<GameObject> InactiveObjects = new();
    }
}