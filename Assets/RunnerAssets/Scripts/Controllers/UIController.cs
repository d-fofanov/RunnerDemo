using System;
using System.Collections.Generic;
using DI;
using Reflex.Attributes;
using RX;
using UnityEngine;
using Views.UI;

namespace Controllers
{
    /**
     * Manages UI main views.
     * Assumes that only one instance of each view type is used at any given time.
     * Retains all the shown views in the memory.
     * For a bigger project some eviction policy should be put in place.
     */
    public class UIController
    {
        [Inject] private readonly UIViewFactory _viewFactory;
        private readonly Dictionary<Type, BaseUIView> _activeViews = new();
        private BaseUIView _foreground;
        private Canvas _canvas;
        private RectTransform _uiRoot;
        private Canvas _overlayCanvas;
        private RectTransform _overlayUiRoot;
        
        public UIController(Canvas mainCanvas, Canvas overlayCanvas, LoadingScreenView tempLoadingScreen)
        {
            _canvas = mainCanvas;
            _uiRoot = _canvas.GetComponent<RectTransform>();
            _overlayCanvas = overlayCanvas;
            _overlayUiRoot = _overlayCanvas.GetComponent<RectTransform>();
            
            _activeViews.Add(typeof(LoadingScreenView), tempLoadingScreen);
        }

        public T Show<T>() where T : BaseUIView
            => ShowView<T>(_uiRoot);

        public void ShowLoadingScreen(ReadOnlyReactiveProperty<float> progress)
        { 
            var loadingScreen = ShowView<LoadingScreenView>(_overlayUiRoot);   
            loadingScreen.SetProgress(progress);
        }

        public void HideLoadingScreen()
        {
            Hide<LoadingScreenView>();
        }

        public T ShowForeground<T>() where T : BaseUIView
        {
            if (_foreground != null)
            {
                Hide(_foreground.GetType());
            }

            var instance = Show<T>();
            _foreground = instance;
            return instance;
        }

        public void HideForeground()
        {
            if (_foreground == null)
                return;
            
            Hide(_foreground.GetType());
        }

        public void Hide<T>()
        {
            Hide(typeof(T));
        }

        public void Hide(Type viewType)
        {
            if (!_activeViews.TryGetValue(viewType, out var view))
                return;
            
            view.gameObject.SetActive(false);
            if (view == _foreground)
            {
                _foreground = null;
            }
        }
        
        private T ShowView<T>(RectTransform parent) where T: BaseUIView
        {
            if (_activeViews.TryGetValue(typeof(T), out var view))
            {
                if (view.transform.parent != parent)
                {
                    view.transform.SetParent(parent);
                }
                view.gameObject.SetActive(true);
                return (T) view;
            }
            
            view = _viewFactory.Create<T>();
            var rt = view.GetComponent<RectTransform>();
            rt.SetParent(parent);
            rt.localScale = Vector3.one;
            
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            rt.anchoredPosition = Vector2.zero;
            
            _activeViews.Add(typeof(T), view);
            return (T) view;
        }
    }
}