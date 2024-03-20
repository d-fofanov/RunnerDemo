using Settings;
using UnityEngine;

namespace RunnerModel
{
    /**
     * Model for a gameplay coin.
     */
    public class CoinModel
    {
        public CoinSettings Settings { get; }
        public Vector2Int Position { get; }
        
        public CoinModel(CoinSettings settings, Vector2Int position)
        {
            Settings = settings;
            Position = position;
        }
    }
}