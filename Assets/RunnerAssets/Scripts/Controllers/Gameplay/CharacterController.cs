using System;
using RunnerModel;
using RX;
using UnityEngine;
using Utils;

namespace Controllers.Gameplay
{
    /**
     * Controller for character-related game logic.
     */
    public class CharacterController : IDisposable
    {
        private readonly TimeUtil _timeUtil;
        private readonly GameplayModel _game;
        private CompositeDisposable _disposable = new();
        private CompositeDisposable _ingameDisposable= new();

        public CharacterController(PersistenceController persistenceController, TimeUtil timeUtil)
        {
            _timeUtil = timeUtil;
            _game = persistenceController.WritableGame;

            _game.GameState.Subscribe(OnGameStateChanged).AddTo(_disposable);
        }

        private void OnGameStateChanged(GameplayModel.State newState)
        {
            if (newState == GameplayModel.State.Running)
            {
                _timeUtil.AddUpdateAction(OnUpdate).AddTo(_ingameDisposable);
            }
            else
            {
                _ingameDisposable.Dispose();
                _ingameDisposable = new();
            }
        }

        private void OnUpdate(float _)
        {
            _game.CharacterEffects.RemoveAll(e => e.StartTime + e.Settings.Duration < Time.time);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}