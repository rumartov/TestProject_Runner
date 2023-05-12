using Data;
using UnityEngine;

namespace Logic.Loot
{
    public class LootPiece : MonoBehaviour
    {
        private const string Hero = "Hero";
        [SerializeField] private GameObject _visual;

        private AudioSource _audioSource;

        private int _destroyDelay = 5;
        private Data.Loot _loot;
        private bool _picked;
        private WorldData _worldData;

        private void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _visual.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Hero))
                return;

            PickUp();
        }

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }

        public void Initialize(Data.Loot loot)
        {
            _loot = loot;
            _picked = false;
        }

        private void PickUp()
        {
            if (_picked)
                return;

            _picked = true;

            _worldData.LootData.Collect(_loot);

            HideModel();

            _audioSource.Play();
        }

        private void HideModel()
        {
            _visual.SetActive(false);
        }
    }
}