using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RX
{
    /**
     * A subscription to a subject, event or property.
     */
    public class Subscription : IDisposable
    {
        public static readonly Subscription Empty = new Subscription(null, null);
        
        private Action _unsubscribe;

        public Subscription(Action subscribe, Action unsubscribe)
        {
            if (subscribe != null && unsubscribe != null)
            {
                _unsubscribe = unsubscribe;
                subscribe();
            }
        }

        public void AddTo(CompositeDisposable d)
        {
            d.Add(this);
        }

        public void Dispose()
        {
            _unsubscribe?.Invoke();
            _unsubscribe = null;
        }
    }
    
    public static class SubscriptionExtensions
    {
        public static Subscription Subscribe(this Button button, Action onButtonClick)
        {
            UnityAction action = () => onButtonClick();
            return new Subscription(
                () => button.onClick.AddListener(action),
                () => button.onClick.RemoveListener(action)
            );
        }
    }
}