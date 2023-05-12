using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class PooledObjectInfo
    {
        public List<GameObject> InactiveObjects = new();
        public string PooledObjectName;
    }
}