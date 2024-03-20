using System;

namespace RX
{
    /**
     * A read-only reactive property that can be observed for changes.
     */
    public class ReadOnlyReactiveProperty<T>
    {
        public delegate void DeltaEventDelegate(T oldValue, T newValue);
        
        protected T _val;
        protected Action _onChangeEmptyEvent = delegate{};
        protected Action<T> _onChangeEvent = delegate{};
        protected DeltaEventDelegate _onDeltaEvent = delegate{};

        public T Value
        {
            get => _val;
        }

        public ReadOnlyReactiveProperty()
        {
        }

        public ReadOnlyReactiveProperty(T val)
        {
            _val = val;
        }
        
        public Subscription Subscribe(Action onChange)
        {
            if (onChange == null)
                return Subscription.Empty;
            
            onChange.Invoke();
            return new Subscription(() => _onChangeEmptyEvent += onChange, () => _onChangeEmptyEvent -= onChange);
        }

        public Subscription Subscribe(Action<T> onChange)
        {
            onChange?.Invoke(_val);
            return new Subscription(() => _onChangeEvent += onChange, () => _onChangeEvent -= onChange);
        }

        public Subscription Subscribe(DeltaEventDelegate onDelta)
        {
            onDelta?.Invoke(_val, _val);
            return new Subscription(() => _onDeltaEvent += onDelta, () => _onDeltaEvent -= onDelta);
        }
        
        public static implicit operator T(ReadOnlyReactiveProperty<T> field)
        {
            return field.Value;
        }
    }
}