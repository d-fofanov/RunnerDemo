using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace Settings
{
    /**
     * Static settings for the whole game.
     */
    [Serializable]
    public class StaticSettings
    {
        [SerializeField] private int _mapWidth;
        [SerializeField] private int _mapHeight;
        [SerializeField] private int _maxStepRise;
        [SerializeField] private int _maxStepFall;
        [SerializeField] private int _maxHoleSize;
        [SerializeField] private int _minPlatformLength;
        [SerializeField] private int _maxPlatformLength;
        [SerializeField] private float _coinChancePerPlatform = 0.4f;
        [SerializeField] private List<CoinSettings> _coinTypes = new();

        public int MapWidth => _mapWidth;
        public int MapHeight => _mapHeight;
        public int MaxStepRise => _maxStepRise;
        public int MaxStepFall => _maxStepFall;
        public int MaxHoleSize => _maxHoleSize;
        public int MinPlatformLength => _minPlatformLength;
        public int MaxPlatformLength => _maxPlatformLength;
        public float CoinChancePerPlatform => _coinChancePerPlatform;
        public IReadOnlyList<CoinSettings> CoinTypes => _coinTypes;
    }
}