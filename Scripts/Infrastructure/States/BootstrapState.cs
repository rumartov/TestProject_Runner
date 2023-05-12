using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Services;
using Services.Input;
using Services.Progress;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string MainLevel = "Main";

        private readonly ICoroutineRunner _coroutineRunner;
        private readonly SceneLoader _sceneLoader;

        private readonly AllServices _services;

        private readonly GameStateMachine _stateMachine;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services,
            ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _coroutineRunner = coroutineRunner;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(Constants.InitialScene, EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void RegisterServices()
        {
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IInputService>(new InputService());

            InitializeProgressService();

            _services.RegisterSingle<IGameFactory>(
                new GameFactory(_services.Single<IAssetProvider>(),
                    _services.Single<IProgressService>(),
                    _services.Single<IInputService>()));
        }

        private void InitializeProgressService()
        {
            var progressService = new ProgressService();
            progressService.InitializeProgress();
            _services.RegisterSingle<IProgressService>(progressService);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState, string>(MainLevel);
        }
    }
}