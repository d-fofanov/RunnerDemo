using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Settings
{
    /**
     * Settings for a gameplay coin.
     */
    [Serializable]
    public class CoinSettings
    {
        [SerializeField] private string _prefabName;
        [SerializeField] private float _pickupRange = 0.7f;
        [SerializeField] private int _pointsAdded = 10;
        [SerializeField] private CharacterEffectSettings _characterEffectSettings;
        
        public string PrefabName => _prefabName;
        public float PickupRange => _pickupRange;
        public int PointsAdded => _pointsAdded;
        public CharacterEffectSettings CharacterEffectSettings => _characterEffectSettings;
    }
}