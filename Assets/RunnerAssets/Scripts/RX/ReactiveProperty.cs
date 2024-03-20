using System;

namespace RX
{
    /**
     * A reactive property that can be observed for changes.
     */
    [Serializable]
    public class ReactiveProperty<T> : ReadOnlyReactiveProperty<T>
    {
        public new virtual T Value
        {
            get => _val;
            set
            {
                var oldVal = _val;
                if (!Equals(value, oldVal))
                {
                    OnValueChange(oldVal, value);
                }
            }
        }

        protected virtual void OnValueChange(T from, T to)
        {
            _val = to;
            _onChangeEvent.Invoke(to);
            _onDeltaEvent.Invoke(from, to);
            _onChangeEmptyEvent.Invoke();
        }

        public ReactiveProperty()
        {
        }

        public ReactiveProperty(T val)
        {
            _val = val;
        }
        
        public static implicit operator T(ReactiveProperty<T> field)
        {
            return field.Value;
        }
    }
}