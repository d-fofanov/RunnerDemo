using System;
using RX;
using TMPro;
using UnityEngine;

namespace Views.UI
{
    /**
     * Loading screen view.
     * Is contained in the initial scene.
     */
    public class LoadingScreenView : BaseUIView
    {
        [SerializeField] private TextMeshProUGUI _progressText;
        
        private CompositeDisposable _disposable = null;
        
        public void SetProgress(ReadOnlyReactiveProperty<float> progress)
        {
            _disposable?.Dispose();
            _disposable = new();
            
            progress?.Subscribe(OnProgressChanged).AddTo(_disposable);
        }
        
        private void OnProgressChanged(float progress)
        {
            _progressText.text = $"{(int)(progress * 100)}%";
        }

        protected override void OnShown()
        {
        }

        protected override void OnHidden()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}