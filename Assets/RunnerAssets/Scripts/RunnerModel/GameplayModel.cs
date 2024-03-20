using System.Collections.Generic;
using BaseModel;
using RX;
using UnityEngine;

namespace RunnerModel
{
    /**
     * Read-only version of the gameplay model.
     */
    public class ROGameplayModel
    {
        public ReadOnlyReactiveProperty<GameplayModel.State> GameState => _model.GameState;
        public ReadOnlyReactiveProperty<Vector3> CharacterPosition => _model.CharacterPosition;
        public ReadOnlyReactiveSet<CoinModel> Coins => _model.Coins;
        public IReadOnlyList<CharacterEffectModel> CharacterEffects => _model.CharacterEffects;
        public RORunnerMap Map => _model.Map;
        
        private readonly GameplayModel _model;
        
        public ROGameplayModel(GameplayModel model)
        {
            _model = model;
        }
    }
    
    /**
     * Gameplay model, contains all the important gameplay data.
     */
    public class GameplayModel : BaseModelStorage
    {
        public enum State
        {
            None,
            Running,
            Victory,
            Defeat
        }
        
        public ReactiveProperty<State> GameState => Get<State>(0);
        public ReactiveProperty<MapModel> Map => Get<MapModel>(10);
        public ReactiveProperty<Vector3> CharacterPosition => Get<Vector3>(20);
        
        public ReactiveSet<CoinModel> Coins => GetSet<CoinModel>(1000);
        
        // Not persisted, not protected
        public readonly List<CharacterEffectModel> CharacterEffects = new();

        public static GameplayModel Create()
        {
            var result = new GameplayModel();

            return result;
        }
    }
}