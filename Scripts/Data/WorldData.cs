using System;

namespace Data
{
    [Serializable]
    public class WorldData
    {
        public LootData LootData;

        public WorldData()
        {
            LootData = new LootData();
        }
    }
}