using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class ObjectPoolManager
    {
        public static List<PooledObjectInfo> ObjectPool = new();

        public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            var pool = ObjectPool.Find(pool => pool.PooledObjectName == objectToSpawn.name);

            if (pool == null)
            {
                pool = new PooledObjectInfo {PooledObjectName = objectToSpawn.name};
                ObjectPool.Add(pool);
            }

            var spawnableObj = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObj != null)
            {
                spawnableObj.transform.position = spawnPosition;
                spawnableObj.transform.rotation = spawnRotation;

                pool.InactiveObjects.Remove(spawnableObj);
                spawnableObj.SetActive(true);
            }

            return spawnableObj;
        }

        public static GameObject SpawnObject(GameObject objectToSpawn)
        {
            var pool = ObjectPool.Find(pool => pool.PooledObjectName == objectToSpawn.name);

            if (pool == null)
            {
                pool = new PooledObjectInfo {PooledObjectName = objectToSpawn.name};
                ObjectPool.Add(pool);
            }

            var spawnableObj = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObj != null)
            {
                pool.InactiveObjects.Remove(spawnableObj);
                spawnableObj.SetActive(true);
            }

            return spawnableObj;
        }

        public static void ReturnObjectToPool(GameObject obj)
        {
            var gameObjectName = obj.name.Substring(0, obj.name.Length - 7);
            var pool = ObjectPool.Find(p => p.PooledObjectName == gameObjectName);

            if (pool == null)
            {
                Debug.LogWarning("Trying to release an object that is not pooled: " + obj.name);
            }
            else
            {
                obj.SetActive(false);
                pool.InactiveObjects.Add(obj);
            }
        }
    }
}