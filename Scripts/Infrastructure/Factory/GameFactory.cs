using System.Collections.Generic;
using Infrastructure.AssetManagement;
using Infrastructure.States;
using Logic.Hero;
using Logic.LevelPart;
using Logic.Loot;
using Services.Input;
using Services.Progress;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IInputService _inputService;
        private readonly IProgressService _progressService;
        private readonly GameStateMachine _stateMachine;

        public List<GameObject> LevelParts = new();

        public GameFactory(IAssetProvider assets, IProgressService progressService, IInputService inputService)
        {
            _assets = assets;
            _progressService = progressService;
            _inputService = inputService;
        }

        public GameObject CreateHero(GameObject at)
        {
            var player = InstantiateRegistered(AssetPath.PlayerPath, at.transform.position);

            player.GetComponent<LootCounter>().Construct(_progressService.Progress.WorldData);

            player.GetComponent<HeroMove>().Construct(_inputService);

            return player;
        }

        public void CreateInitialLevelPart()
        {
            var defaultSpawnPosition = GameObject.FindWithTag(Constants.LevelPartStartSpawnPosition).transform.position;
            var levelPartObj = InstantiateRegistered(AssetPath.LevelPartPath, defaultSpawnPosition);
            var levelPart = levelPartObj.GetComponent<LevelPart>();

            levelPart.id = 0;
            levelPart.lastLevelPartId = -1;

            levelPartObj.GetComponentInChildren<LevelPartCrossed>().Construct(this);

            LevelParts.Add(levelPartObj);
        }

        public void CreateLevelPart()
        {
            var lastLevelPart = LevelParts[LevelParts.Count - 1].GetComponent<LevelPart>();
            var spawnPoint = lastLevelPart.connecton;

            var levelPartObj = InstantiateRegistered(AssetPath.LevelPartPath, spawnPoint.position);

            var levelPart = levelPartObj.GetComponent<LevelPart>();
            levelPart.lastLevelPartId = lastLevelPart.id;
            levelPart.id = lastLevelPart.id + 1;

            levelPartObj.GetComponentInChildren<LevelPartCrossed>().Construct(this);

            CreateLoot(levelPart);

            LevelParts.Add(levelPartObj);
        }

        public void DestroyLevelPart(int destroyLevelPartId)
        {
            var levelPartObjToDestroy = LevelParts.Find(
                levelPart =>
                    levelPart.GetComponent<LevelPart>().id == destroyLevelPartId);

            if (levelPartObjToDestroy != null)
            {
                DestroyCollectables(levelPartObjToDestroy);

                LevelParts.Remove(levelPartObjToDestroy);
                ObjectPoolManager.ReturnObjectToPool(levelPartObjToDestroy);
            }

            if (LevelParts.Count < 5) CreateLevelPart();
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            var gameObject = _assets.Instantiate(prefabPath, at);

            return gameObject;
        }

        private void CreateLoot(LevelPart levelPart)
        {
            var initialCollectablesSpawnPosition = levelPart.initialCollectablesSpawnPosition.position;

            var visualLocalScale = levelPart.visual.localScale;
            var rows = (int) visualLocalScale.x;
            var column = (int) visualLocalScale.z;

            for (var i = 0; i < rows; i++)
            for (var j = 0; j < column; j++)
            {
                var spawnPosition = new Vector3(
                    initialCollectablesSpawnPosition.x + i,
                    initialCollectablesSpawnPosition.y,
                    initialCollectablesSpawnPosition.z + j);

                if (Random.Range(0, 2) == 1)
                {
                    var loot = InstantiateRegistered(AssetPath.Coin, spawnPosition);
                    var lootPiece = loot.GetComponent<LootPiece>();

                    lootPiece.Construct(_progressService.Progress.WorldData);
                    var lootValue = new Data.Loot();
                    lootValue.Value = 1;
                    lootPiece.Initialize(lootValue);

                    loot.transform.SetParent(levelPart.collectablesFolder);
                }
            }
        }

        private static void DestroyCollectables(GameObject levelPartObjToDestroy)
        {
            var collectablesFolder = levelPartObjToDestroy.GetComponent<LevelPart>().collectablesFolder;
            var collectablesFolderChildCount = collectablesFolder.childCount;

            for (var i = 0; i < collectablesFolderChildCount; i++)
                ObjectPoolManager.ReturnObjectToPool(collectablesFolder.GetChild(i).gameObject);
        }
    }
}