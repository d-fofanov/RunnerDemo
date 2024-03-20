using System;

namespace RX
{
    /**
     * A subject that can be observed for changes.
     */
    public class Subject
    {
        private Action _onNext = delegate { };

        public Subscription Subscribe(Action onNext)
        {
            return new Subscription(() => _onNext += onNext, () => _onNext -= onNext);
        }

        public void OnNext()
        {
            _onNext();
        }

        public ReadOnlySubject ToReadOnly()
        {
            return new ReadOnlySubject(this);
        }
    }

    public class ReadOnlySubject
    {
        private Subject _value;

        public ReadOnlySubject(Subject value)
        {
            _value = value;
        }
        
        public Subscription Subscribe(Action onNext)
        {
            return _value.Subscribe(onNext);
        }
    }
    
    public class Subject<T>
    {
        private Action _emptyOnNext = delegate { };
        private Action<T> _onNext = delegate { };

        public Subscription Subscribe(Action emptyOnNext)
        {
            return new Subscription(() => _emptyOnNext += emptyOnNext, () => _emptyOnNext -= emptyOnNext);
        }
        
        public Subscription Subscribe(Action<T> onNext)
        {
            return new Subscription(() => _onNext += onNext, () => _onNext -= onNext);
        }

        public void OnNext(T val)
        {
            _emptyOnNext();
            _onNext(val);
        }
    }
    
    public class Subject<T1, T2>
    {
        private Action _emptyOnNext = delegate { };
        private Action<T1, T2> _onNext = delegate { };

        public Subscription Subscribe(Action emptyOnNext)
        {
            return new Subscription(() => _emptyOnNext += emptyOnNext, () => _emptyOnNext -= emptyOnNext);
        }
        
        public Subscription Subscribe(Action<T1, T2> onNext)
        {
            return new Subscription(() => _onNext += onNext, () => _onNext -= onNext);
        }

        public void OnNext(T1 val1, T2 val2)
        {
            _emptyOnNext();
            _onNext(val1, val2);
        }
    }
}