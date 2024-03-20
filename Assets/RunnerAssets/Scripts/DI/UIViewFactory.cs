using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;
using Views.UI;

namespace DI
{
    /**
     * This class is responsible for creating UI views.
     * For a production project Resources usage may be replaced with Addressables.
     */
    public class UIViewFactory
    {
        private readonly Container _container;

        public UIViewFactory(Container container)
        {
            _container = container;
        }
        
        public T Create<T>() where T: BaseUIView
        {
            var prefab = Resources.Load<T>($"UI/{typeof(T).Name}");
            var view = Object.Instantiate(prefab);
            AttributeInjector.Inject(view, _container);
            view.Initialize();
            
            return view;
        }
    }
}