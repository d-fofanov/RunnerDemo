using System.Collections.Generic;

namespace RX
{
    /**
     * A reactive set that can be observed for changes.
     */
    public class ReactiveSet<T> : ReadOnlyReactiveSet<T>
    {
        public void Add(T value)
        {
            if (_set.Add(value))
            {
                _onAdded.OnNext(value);
                _onAnyChange.OnNext();
            }
        }
        
        public void AddRange(IEnumerable<T> value)
        {
            foreach (var elem in value)
            {
                Add(elem);
            }
        }
        
        public void Remove(T value)
        {
            if (_set.Remove(value))
            {
                _onRemoved.OnNext(value);
                _onAnyChange.OnNext();
            }
        }

        public void Clear()
        {
            foreach (var elem in _set)
            {
                _onRemoved.OnNext(elem);
            }
            _set.Clear();
            _onAnyChange.OnNext();
        }
    }
}