using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    /**
     * Settings for a character effect.
     */
    [Serializable]
    public class CharacterEffectSettings
    {
        [SerializeField] private float _duration = 3f;
        [SerializeField] private float _speedMultiplier = 1f;
        [SerializeField] private float _gravityMultiplier = 1f;
        [SerializeField] private float _jumpForceMultiplier = 1f;
        [SerializeField] private int _jumpsCountInFlight = 0;
        
        public float Duration => _duration;
        public float SpeedMultiplier => _speedMultiplier;
        public float GravityMultiplier => _gravityMultiplier;
        public float JumpForceMultiplier => _jumpForceMultiplier;
        public int JumpsCountInFlight => _jumpsCountInFlight;

        public static CharacterEffectSettings Aggregate(IEnumerable<CharacterEffectSettings> effects)
        {
            float speedMultiplier = 1f;
            float gravityMultiplier = 1f;
            float jumpForceMultiplier = 1f;
            int jumpsCountInFlight = 0;

            foreach (var effect in effects)
            {
                speedMultiplier *= effect.SpeedMultiplier;
                gravityMultiplier *= effect.GravityMultiplier;
                jumpForceMultiplier *= effect.JumpForceMultiplier;
                jumpsCountInFlight += effect.JumpsCountInFlight;
            }
            
            return new CharacterEffectSettings
            {
                _speedMultiplier = speedMultiplier,
                _gravityMultiplier = gravityMultiplier,
                _jumpForceMultiplier = jumpForceMultiplier,
                _jumpsCountInFlight = jumpsCountInFlight
            };
        }
    }
}