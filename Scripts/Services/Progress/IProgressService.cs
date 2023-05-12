using Data;

namespace Services.Progress
{
    public interface IProgressService : IService
    {
        PlayerProgress Progress { get; }
        void InitializeProgress();
    }
}