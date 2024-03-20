using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Cysharp.Threading.Tasks;
using DI;
using Reflex.Attributes;
using RunnerModel;
using RX;
using StarterAssets;
using UnityEngine;

namespace Views.Gameplay
{
    /**
     * Overall gameplay view.
     * Responsible for building the map, character, coins and camera.
     */
    public class GameplayView : MonoBehaviour
    {
        public ReadOnlyReactiveProperty<Vector3> CharacterPosition => _characterPosition;
        [Inject] private ROModel _model;
        [Inject] private AssetFactory _assetFactory;
        [Inject] private GCameraView _cameraView;
        
        private readonly List<GameObject> _blocks = new();
        private readonly Dictionary<CoinModel, GCoinView> _coins = new();
        private readonly ReactiveProperty<Vector3> _characterPosition = new();
        private readonly Dictionary<string, GCoinView> _coinPrefabs = new();
        private readonly List<CoinModel> _coinBuffer = new();
        private GCharacterView _characterView;
        private Transform _blockPrefab;
        private CompositeDisposable _disposable = null;

        public async UniTask Build()
        {
            if (_blockPrefab == null)
                _blockPrefab = await _assetFactory.LoadBlockPrefab();

            var firstBlockY = await BuildMap();

            _characterView = await _assetFactory.InstantiateView<GCharacterView>(
                new Vector3(0, firstBlockY + 2, 0),
                Quaternion.LookRotation(Vector3.right, Vector3.up)
            );
            
            _cameraView.SetTarget(_characterView.transform);
            
            _disposable?.Dispose();
            _disposable = new ();
            _model.ROGame.GameState.Subscribe(OnGameStateChanged).AddTo(_disposable);
            _model.ROGame.Coins.Subscribe(OnCoinsChanged).AddTo(_disposable);
        }

        public void Clear()
        {
            Unsubscribe();
            _assetFactory.ReturnInstance(_characterView.gameObject);
            
            _characterView = null;
            
            foreach (var block in _blocks)
            {
                Destroy(block);
            }
            _blocks.Clear();
            
            foreach (var coin in _coins.Values)
            {
                _assetFactory.ReturnInstance(coin.gameObject);
            }
            _coins.Clear();
            _coinPrefabs.Clear();
        }

        private void Update()
        {
            if (_characterView == null)
                return;
            
            _characterPosition.Value = _characterView.transform.position;
        }

        private async UniTask<int> BuildMap()
        {
            var map = _model.ROGame.Map;
            var firstBlockY = 0;
            for (int w=0; w<map.Width; w++)
            {
                for (int h=0; h<map.Height; h++)
                {
                    if (!map[w, h])
                        continue;
                    
                    if (w == 0 && h > firstBlockY)
                        firstBlockY = h;
                    
                    var block = Instantiate(_blockPrefab, new Vector3(w, h, 0), Quaternion.identity);
                    block.SetParent(transform);
                    _blocks.Add(block.gameObject);
                }
            }

            foreach (var coinModel in _model.ROGame.Coins)
            {
                await AddCoin(coinModel);
            }

            return firstBlockY;
        }

        private void OnGameStateChanged(GameplayModel.State newState)
        {
            _characterView.SetFrozen(newState != GameplayModel.State.Running);
        }
        
        private void OnCoinsChanged()
        {
            _coinBuffer.Clear();
            _coinBuffer.AddRange(_model.ROGame.Coins.Except(_coins.Keys));
            foreach (var added in _coinBuffer)
            {
                AddCoin(added).Forget(Debug.LogException);
            }

            _coinBuffer.Clear();
            _coinBuffer.AddRange(_coins.Keys.Except(_model.ROGame.Coins));
            foreach (var removed in _coinBuffer)
            {
                RemoveCoin(removed);
            }
        }

        private async UniTask AddCoin(CoinModel coinModel)
        {
            var prefabName = coinModel.Settings.PrefabName;
            if (!_coinPrefabs.TryGetValue(prefabName, out var prefab))
            {
                prefab = await _assetFactory.LoadCoinPrefab(prefabName);
                _coinPrefabs[prefabName] = prefab;
            }

            var coinView = Instantiate(prefab, new Vector3(coinModel.Position.x, coinModel.Position.y, 0),
                Quaternion.identity);
            coinView.transform.SetParent(transform);
            _coins[coinModel] = coinView;
        }

        private void RemoveCoin(CoinModel coinModel)
        {
            if (!_coins.TryGetValue(coinModel, out var coinView))
                return;
            
            _assetFactory.ReturnInstance(coinView.gameObject);
            _coins.Remove(coinModel);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}