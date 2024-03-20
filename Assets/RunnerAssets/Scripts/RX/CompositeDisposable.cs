using System;
using System.Collections.Generic;

namespace RX
{
    /**
     * A disposable that contains other disposables.
     */
    public class CompositeDisposable : IDisposable
    {
        private HashSet<IDisposable> _set = new HashSet<IDisposable>();

        public void Add(IDisposable d)
        {
            _set.Add(d);
        }

        public void Remove(IDisposable d)
        {
            _set.Remove(d);
        }

        public void AddTo(CompositeDisposable target)
        {
            target.Add(this);
        }
        
        public void Dispose()
        {
            if (_set == null)
                return;
            
            foreach (var d in _set)
            {
                d.Dispose();
            }

            _set.Clear();
            _set = null;
        }
    }
}