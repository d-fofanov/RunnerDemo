using System;
using UnityEngine;

namespace RX
{
    /**
     * A reactive property that saves its value to PlayerPrefs.
     */
    public class PrefsReactiveProperty<T> : ReactiveProperty<T>
    {
        private string _prefsName;
        private Container _container = new Container();

        public PrefsReactiveProperty(string prefsName)
        {
            _prefsName = prefsName;
            
            if (TryLoadFromPrefs(out var loaded))
                Value = loaded;
        }

        public PrefsReactiveProperty(string prefsName, T val)
        {
            _prefsName = prefsName;

            Value = TryLoadFromPrefs(out var loaded) ? loaded : val;
        }

        protected override void OnValueChange(T from, T to)
        {
            base.OnValueChange(from, to);
            
            SaveToPrefs(to);
        }

        private bool TryLoadFromPrefs(out T result)
        {
            result = default;
            
            var serialized = PlayerPrefs.GetString(_prefsName, "");
            if (string.IsNullOrWhiteSpace(serialized))
                return false;

            try
            {
                JsonUtility.FromJsonOverwrite(serialized, _container);
                result = _container.Val;
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Could not deserialize {typeof(T)} from '{serialized}': {e}");
                return false;
            }
        }

        private void SaveToPrefs(T val)
        {
            _container.Val = val;
            var serialized = JsonUtility.ToJson(_container);
            PlayerPrefs.SetString(_prefsName, serialized);
            PlayerPrefs.Save();
        }

        private class Container
        {
            public T Val;
        }
    }
}