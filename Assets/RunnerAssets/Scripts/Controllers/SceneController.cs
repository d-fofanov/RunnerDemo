using Cysharp.Threading.Tasks;
using RX;
using UnityEngine.SceneManagement;
using Utils;

namespace Controllers
{
    /**
     * This class is responsible for loading scenes.
     * Production version of this class is expected to handle loading from Addressables.
     */
    public class SceneController
    {
        private const string GameplaySceneName = "GameplayScene";
        
        public async UniTask LoadGameplayScene(ReactiveProperty<float> progress)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(GameplaySceneName, LoadSceneMode.Additive);
            await asyncOperation.AwaitWithProgress(progress);
        }
        
        public async UniTask UnloadGameplayScene(ReactiveProperty<float> progress)
        {
            var asyncOperation = SceneManager.UnloadSceneAsync(GameplaySceneName);
            await asyncOperation.AwaitWithProgress(progress);
        }
    }
}