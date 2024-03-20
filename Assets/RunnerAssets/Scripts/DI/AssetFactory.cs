using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;
using Views.Gameplay;
using Object = UnityEngine.Object;

namespace DI
{
    /**
     * Loads and instantiates gameplay assets.
     * Should hide the source of the assets and the loading mechanism.
     * May contain pooling logic if needed.
     */
    public class AssetFactory
    {
        private const string GameplayPath = "Gameplay";
        private readonly Container _container;

        public AssetFactory(Container container)
        {
            _container = container;
        }

        public async UniTask<T> InstantiateView<T>(Vector3 position, Quaternion rotation) where T: Object
        {
            try
            {
                var asset = await LoadView<T>();
                var view = Object.Instantiate(asset, position, rotation) as T;
                AttributeInjector.Inject(view, _container);
                return view;
            }
            catch (Exception e)
            {
                Debug.LogError($"Could not create view {nameof(T)}: {e}");
                return null;
            }
        }
        
        public async UniTask<T> InstantiateView<T>(Transform parent) where T: Object
        {
            try
            {
                var asset = await LoadView<T>();
                var view = Object.Instantiate(asset, parent) as T;
                AttributeInjector.Inject(view, _container);
                return view;
            }
            catch (Exception e)
            {
                Debug.LogError($"Could not create view {nameof(T)}: {e}");
                return null;
            }
        }

        public async UniTask<Transform> LoadBlockPrefab()
        {
            var request = Resources.LoadAsync<Transform>($"{GameplayPath}/Block");
            await request;
            return request.asset as Transform;
        }

        public async UniTask<GCoinView> LoadCoinPrefab(string prefabName)
        {
            var request = Resources.LoadAsync<GCoinView>($"{GameplayPath}/Coins/{prefabName}");
            await request;
            return request.asset as GCoinView;
        }

        public void ReturnInstance(Object instance)
        {
            Object.Destroy(instance);
        }

        private async UniTask<T> LoadView<T>() where T : Object
        {
            var request = Resources.LoadAsync<T>($"{GameplayPath}/{typeof(T).Name}");
            await request;
            return request.asset as T;
        }
    }
}