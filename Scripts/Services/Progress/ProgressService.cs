using Data;
using UnityEngine;

namespace Services.Progress
{
    public class ProgressService : IProgressService
    {
        public PlayerProgress Progress { get; set; }

        public void InitializeProgress()
        {
            PlayerPrefs.DeleteAll();
            Progress = new PlayerProgress(Constants.MainLevel);
        }
    }
}