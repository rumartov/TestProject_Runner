using Infrastructure.Factory;
using UnityEngine;

namespace Logic.LevelPart
{
    public class LevelPartCrossed : MonoBehaviour
    {
        private const string Hero = "Hero";

        private IGameFactory _gameFactory;
        private LevelPart _levelPart;

        private void Start()
        {
            _levelPart = GetComponentInParent<LevelPart>();
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!collider.CompareTag(Hero))
                return;

            _gameFactory.DestroyLevelPart(_levelPart.lastLevelPartId);
        }

        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }
    }
}