using System;
using System.Collections;
using System.Collections.Generic;

namespace RX
{
    /**
     * A read-only reactive set that can be observed for changes.
     */
    public class ReadOnlyReactiveSet<T> : IEnumerable<T>
    {
        public int Count => _set.Count;
        
        protected readonly HashSet<T> _set = new();
        protected readonly Subject _onAnyChange = new();
        protected readonly Subject<T> _onAdded = new();
        protected readonly Subject<T> _onRemoved = new();
        
        public bool Contains(T value)
        {
            return _set.Contains(value);
        }
        

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }
        
        public Subscription Subscribe(Action action) => _onAnyChange.Subscribe(action);
        public Subscription SubscribeAdded(Action<T> action) => _onAdded.Subscribe(action);
        public Subscription SubscribeRemoved(Action<T> action) => _onRemoved.Subscribe(action);
    }
}