using UnityEngine;

namespace Logic.LevelPart
{
    public class LevelPart : MonoBehaviour
    {
        public Transform connecton;
        public int id;
        public int lastLevelPartId;
        public Transform initialCollectablesSpawnPosition;
        public Transform visual;
        public Transform collectablesFolder;
    }
}