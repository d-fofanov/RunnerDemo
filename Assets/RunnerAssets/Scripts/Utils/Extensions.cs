using Cysharp.Threading.Tasks;
using RX;
using UnityEngine;

namespace Utils
{
    /**
     * General purpose extensions.
     */
    public static class Extensions
    {
        public static async UniTask AwaitWithProgress(this AsyncOperation asyncOperation, ReactiveProperty<float> progress)
        {
            if (asyncOperation == null)
                return;
            
            if (progress == null)
            {
                await asyncOperation;
                return;
            }
            
            while (!asyncOperation.isDone)
            {
                progress.Value = asyncOperation.progress;
                await UniTask.Yield();
            }
        }

        public static Vector3 ToWorldPosition(this Vector2Int vec)
        {
            return new Vector3(vec.x, vec.y);
        }
    }
}