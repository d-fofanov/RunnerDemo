using System;
using System.Collections.Generic;
using RunnerModel;
using RX;
using UnityEngine;
using Utils;

namespace Controllers.Gameplay
{
    /**
     * Controller for coin-related gameplay logic.
     */
    public class CoinController : IDisposable
    {
        private readonly GameplayModel _game;
        private readonly MetaModel _meta;
        private readonly List<CoinModel> _coinBuffer = new();
        private CompositeDisposable _disposable = new();
        
        public CoinController(PersistenceController persistenceController)
        {
            _game = persistenceController.WritableGame;
            _meta = persistenceController.WritableMeta;
            
            _game.CharacterPosition.Subscribe(OnCharacterMoved).AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
        
        private void OnCharacterMoved(Vector3 position)
        {
            if (_game.GameState != GameplayModel.State.Running)
                return;

            _coinBuffer.Clear();
            _coinBuffer.AddRange(_game.Coins);
            foreach (var coin in _coinBuffer)
            {
                if (Vector3.Distance(coin.Position.ToWorldPosition(), position) > coin.Settings.PickupRange)
                    continue;
                
                _game.Coins.Remove(coin);
                _game.CharacterEffects.Add(new CharacterEffectModel(
                    coin.Settings.CharacterEffectSettings,
                    Time.time
                ));
                _meta.TotalPoints.Value += coin.Settings.PointsAdded;
            }
        }
    }
}