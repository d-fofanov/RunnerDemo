using UnityEngine;

namespace Settings
{
    /**
     * Cheap way to expose settings in an editable way.
     * In the course of the project growth may be replaced with [remote] json storage for example.
     */
    public class StaticSettingsHost : MonoBehaviour
    {
        [SerializeField] private StaticSettings _settings;

        public StaticSettings Settings => _settings;
    }
}