using Services;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(GameObject at);
        void CreateLevelPart();
        void DestroyLevelPart(int destroyLevelPartId);
        void CreateInitialLevelPart();
    }
}