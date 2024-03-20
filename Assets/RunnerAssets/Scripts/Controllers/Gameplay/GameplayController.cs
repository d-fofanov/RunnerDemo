using System;
using Cysharp.Threading.Tasks;
using DI;
using RunnerModel;
using RX;
using UnityEngine;
using Utils;
using Views.Gameplay;

namespace Controllers.Gameplay
{
    /**
     * Main gameplay controller.
     */
    public class GameplayController
    {
        private readonly AssetFactory _assetFactory;
        private readonly ROModel _model;
        private readonly MetaModel _meta;
        private readonly GameplayModel _game;

        private GameplayView _gameplayView;
        private CompositeDisposable _disposable = null;
        
        public GameplayController(AssetFactory assetFactory, ROModel model, PersistenceController persistenceController,
            CoinController coinController, CharacterController characterController)
        {
            _assetFactory = assetFactory;
            _model = model;
            _meta = persistenceController.WritableMeta;
            _game = persistenceController.WritableGame;
        }

        public async UniTask Start()
        {
            await Stop();
            
            var (map, coinsList) = MapGenerator.GenerateMap(_model.ROMeta.Level.Value, _model.Settings);
            _game.Map.Value = map;
            _game.Coins.Clear();
            _game.Coins.AddRange(coinsList);
            
            _gameplayView = await _assetFactory.InstantiateView<GameplayView>(null);
            await _gameplayView.Build();
            _disposable?.Dispose();
            _disposable = new ();
            _gameplayView.CharacterPosition.Subscribe(OnCharacterMoved).AddTo(_disposable);
            _game.GameState.Value = GameplayModel.State.Running;
        }

        public async UniTask Stop()
        {
            if (_gameplayView == null)
                return;
            
            _gameplayView.Clear();
            _assetFactory.ReturnInstance(_gameplayView);
            _gameplayView = null;
            _disposable?.Dispose();
            _disposable = null;

            ResetGameplayModel();
        }

        private void ResetGameplayModel()
        {
            _game.GameState.Value = GameplayModel.State.None;
            _game.Coins.Clear();
            _game.CharacterEffects.Clear();
        }

        private void OnCharacterMoved(Vector3 position)
        {
            _game.CharacterPosition.Value = position;
            
            if (position.x < 0 || position.y < -1)
                Defeat();
            else if (position.x >= _model.ROGame.Map.Width)
                Victory();
        }

        private void Defeat()
        {
            _game.GameState.Value = GameplayModel.State.Defeat;
            RestartIn(2).Forget(Debug.LogException);
        }

        private void Victory()
        {
            _game.GameState.Value = GameplayModel.State.Victory;
            _meta.Level.Value++;
            RestartIn(3).Forget(Debug.LogException);
        }

        private async UniTask RestartIn(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            
            await Start();
        }
    }
}