using Settings;

namespace RunnerModel
{
    /**
     * Model for a character effect during gameplay.
     */
    public class CharacterEffectModel
    {
        public CharacterEffectSettings Settings { get; }
        public float StartTime { get; }
        
        public CharacterEffectModel(CharacterEffectSettings settings, float startTime)
        {
            Settings = settings;
            StartTime = startTime;
        }
    }
}