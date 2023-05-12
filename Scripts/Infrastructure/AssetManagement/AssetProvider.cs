using Infrastructure.Factory;
using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Instantiate(string path, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(path);

            var pooledObject = ObjectPoolManager.SpawnObject(prefab, at, Quaternion.identity);

            if (pooledObject != null) return pooledObject;

            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);

            var pooledObject = ObjectPoolManager.SpawnObject(prefab);

            if (pooledObject != null) return pooledObject;

            return Object.Instantiate(prefab);
        }
    }
}