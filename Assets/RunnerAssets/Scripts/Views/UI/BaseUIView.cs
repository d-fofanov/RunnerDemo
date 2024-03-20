using UnityEngine;

namespace Views.UI
{
    /**
     * Base class for UI views.
     * Calls OnShown/OnHidded when the view is enabled/disabled but only after injection has been done.
     */
    public abstract class BaseUIView : MonoBehaviour
    {
        private bool _isInitialized;

        protected abstract void OnShown();
        protected abstract void OnHidden();

        public void Initialize()
        {
            _isInitialized = true;

            if (gameObject.activeInHierarchy)
                OnShown();
        }
        
        private void OnEnable()
        {
            if (_isInitialized)
            {
                OnShown();
            }
        }
        
        private void OnDisable()
        {
            if (_isInitialized)
            {
                OnHidden();
            }
        }
    }
}