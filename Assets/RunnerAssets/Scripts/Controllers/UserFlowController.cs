using Controllers.Gameplay;
using Cysharp.Threading.Tasks;
using RX;
using UnityEngine;
using Utils;
using Views.UI;

namespace Controllers
{
    /**
     * This class is responsible for controlling the user flow, i.e. transitions between application states.
     * For a more complex state graph, a state machine or a state pattern could be used.
     */
    public class UserFlowController
    {
        private readonly UIController _uiController;
        private readonly SceneController _sceneController;
        private readonly GameplayController _gameplayController;

        public UserFlowController(UIController uiController, SceneController sceneController, GameplayController gameplayController)
        {
            _uiController = uiController;
            _sceneController = sceneController;
            _gameplayController = gameplayController;
        }

        public void InitializeFlow()
        {
            _uiController.ShowForeground<MainMenuView>();
            
            _uiController.HideLoadingScreen();
        }

        public void GameplayRequested()
        {
            StartGameplay().Forget(Debug.LogException);
        }

        public void MainMenuRequested()
        {
            StopGameplay().Forget(Debug.LogException);
        }

        private async UniTask StartGameplay()
        {
            var progress = new ReactiveProperty<float>();
            _uiController.ShowLoadingScreen(progress);
            
            await _sceneController.LoadGameplayScene(progress);
            _uiController.ShowForeground<GameplayUIView>();
            await _gameplayController.Start();
            
            _uiController.HideLoadingScreen();
        }

        private async UniTask StopGameplay()
        {
            var progress = new ReactiveProperty<float>();
            _uiController.ShowLoadingScreen(progress);
            
            await _gameplayController.Stop();
            
            await _sceneController.UnloadGameplayScene(progress);
            _uiController.ShowForeground<MainMenuView>();
            _uiController.HideLoadingScreen();
        }
        
    }
}