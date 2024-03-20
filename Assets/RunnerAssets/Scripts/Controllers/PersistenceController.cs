using System;
using RunnerModel;
using RX;
using Settings;
using UnityEngine;
using Utils;

namespace Controllers
{
    /**
     * This class is responsible for management and persistence of the root models.
     */
    public class PersistenceController : IDisposable
    {
        private const string MetaFilePath = "meta.dat";
        
        public ROModel FullModel { get; }
        public MetaModel WritableMeta { get; }

        public GameplayModel WritableGame { get; }

        private readonly StaticSettings _settings;

        private bool _modelIsDirty = false;
        private CompositeDisposable _disposable = new();

        public PersistenceController(TimeUtil timeUtil, StaticSettings settings)
        {
            _settings = settings;
            
            InitializeModels(out var meta, out var game);
            FullModel = new ROModel(_settings, meta, game);
            WritableMeta = meta;
            WritableGame = game;
            
            meta.SubscribeToAnyChange(OnAnyModelChange).AddTo(_disposable);
            
            timeUtil.AddUpdateAction(OnUpdate).AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
        
        private void OnUpdate(float _)
        {
            if (!_modelIsDirty)
                return;

            _modelIsDirty = false;
            TrySaveModel();
        }

        private void OnAnyModelChange()
        {
            _modelIsDirty = true;
        }

        private bool TrySaveModel()
        {
            var metaBytes = WritableMeta.Serialize();

            var errorGameplay = string.Empty;
            if (EncryptionUtils.EncryptAndWriteReliableFile(MetaFilePath, metaBytes, out var errorMeta))
                return true;

            Debug.LogError($"Could not save meta model data: {errorMeta} or gameplay model data: {errorGameplay}");
            return false;
        }

        private void InitializeModels(out MetaModel meta, out GameplayModel game)
        {
            byte[] metaBytes;
            while (!EncryptionUtils.ReadAndDecryptReliableFile(MetaFilePath, out metaBytes, out var error))
            {
                Debug.LogError($"Could not deserialize meta model from file {MetaFilePath}: {error}");
                
                if (!FileUtils.PurgeLatestReliableVersion(MetaFilePath, out error))
                {
                    Debug.LogError($"Could not purge latest reliable version of {MetaFilePath}: {error}");
                    break;
                }
            }

            if (metaBytes == null)
            {
                meta = MetaModel.Create();
            }
            else
            {
                meta = new MetaModel();
                meta.Deserialize(metaBytes);
            }
            
            game = GameplayModel.Create();
        }
    }
}