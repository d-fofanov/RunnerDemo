using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BaseModel.Fields;
using RX;
using UnityEngine;

namespace BaseModel
{
    /**
     * Base class for model storage, that handles serialization and deserialization of model fields.
     */
    public abstract class BaseModelStorage
    {
        private readonly Dictionary<int, IModelField> _allFields = new();
        private readonly Dictionary<int, IModelField> _allArrays = new();
        private readonly Dictionary<int, IModelField> _allSets = new();

        public CompositeDisposable SubscribeToAnyChange(Action action)
        {
            FillAllFields();

            var result = new CompositeDisposable();
            foreach (var field in _allFields.Values)
            {
                field.Subscribe(action).AddTo(result);
            }
            foreach (var field in _allArrays.Values)
            {
                field.Subscribe(action).AddTo(result);
            }
            foreach (var field in _allSets.Values)
            {
                field.Subscribe(action).AddTo(result);
            }

            return result;
        }
        
        public void Deserialize(byte[] fromData)
        {
            FillAllFields();

            var allFieldKeys = _allFields.Keys.ToHashSet();
            var allArrayKeys = _allArrays.Keys.ToHashSet();
            var allSetKeys = _allSets.Keys.ToHashSet();

            using var ms = new MemoryStream(fromData);
            using var br = new BinaryReader(ms);

            var meta = new MetaField();
            var currentFieldPos = 0;
            while (ms.Position + MetaField.OwnLength < ms.Length)
            {
                currentFieldPos += MetaField.OwnLength;
                meta.Deserialize(br);
                var nextFieldPos = currentFieldPos + meta.Length;
                
                if (_allFields.TryGetValue(meta.Index, out var field))
                {
                    field.Deserialize(br);
                    allFieldKeys.Remove(meta.Index);
                }
                else if (_allArrays.TryGetValue(meta.Index, out var array))
                {
                    array.Deserialize(br);
                    allArrayKeys.Remove(meta.Index);
                }
                else if (_allSets.TryGetValue(meta.Index, out var set))
                {
                    set.Deserialize(br);
                    allSetKeys.Remove(meta.Index);
                }
                else
                {
                    Debug.LogWarning($"[ModelStorage] Cannot deserialize unknown field/array index: {meta.Index}");
                }

                if (br.BaseStream.Position != nextFieldPos)
                {
                    Debug.LogWarning($"[ModelStorage] Incorrect position after field #{meta.Index} deserialization." +
                                     $" Expected position: {nextFieldPos}, actual: {br.BaseStream.Position}");
                    ms.Seek(nextFieldPos - br.BaseStream.Position, SeekOrigin.Current);
                }

                currentFieldPos = nextFieldPos;
            }
            
            foreach (var key in allFieldKeys)
            {
                Debug.LogWarning($"[ModelStorage] No input data for deserialization of field index: {key}");
            }
            foreach (var key in allArrayKeys)
            {
                Debug.LogWarning($"[ModelStorage] No input data for deserialization of array index: {key}");
            }
            foreach (var key in allSetKeys)
            {
                Debug.LogWarning($"[ModelStorage] No input data for deserialization of set index: {key}");
            }
            
            OnPostDeserialized();
        }
        
        public byte[] Serialize()
        {
            FillAllFields();
            
            var meta = new MetaField();

            using var outputStream = new MemoryStream();
            using var outputWriter = new BinaryWriter(outputStream);
            
            using var bufferStream = new MemoryStream();
            using var bufferWriter = new BinaryWriter(bufferStream);
            
            foreach (var (index, field) in _allFields)
            {
                SerializeElement(index, field);
            }
            foreach (var (index, array) in _allArrays)
            {
                SerializeElement(index, array);
            }
            foreach (var (index, set) in _allSets)
            {
                SerializeElement(index, set);
            }

            void SerializeElement(int index, IModelField elem)
            {
                bufferWriter.Seek(0, SeekOrigin.Begin);
                elem.Serialize(bufferWriter);
                var length = (int)bufferWriter.BaseStream.Position;
                
                meta.ResetTo(index, length);
                
                meta.Serialize(outputWriter);
                outputWriter.Write(bufferStream.GetBuffer(), 0, length);
            }
            
            return outputStream.ToArray();
        }
        
        protected ReactiveProperty<T> Get<T>(int index)
        {
            if (!_allFields.TryGetValue(index, out var field))
            {
                field = ModelFieldUtil.Create<T>();
                _allFields[index] = field;
            }

            if (field is BaseModelField<T> casted)
                return casted;
            
            Debug.LogError($"[ModelStorage] Wrong field type with index {index}: expected BaseModelField<{typeof(T).Name}>, got {field.GetType().Name}");
            return null;
        }
        
        protected ReactiveSet<T> GetSet<T>(int index)
        {
            if (!_allSets.TryGetValue(index, out var set))
            {
                set = new ModelSet<T>();
                _allSets[index] = set;
            }

            if (set is ModelSet<T> casted)
                return casted;
            
            Debug.LogError($"[ModelStorage] Wrong field type: expected ModelSet<{typeof(T).Name}>, got {set.GetType().Name}<{set.GetType().GetGenericArguments()[0].Name}>");
            return null;
        }

        protected virtual void OnPostDeserialized()
        {
            
        }

        private bool _isInitialized = false;
        private void FillAllFields()
        {
            if (_isInitialized)
                return;
            
            var type = GetType();
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            foreach (var property in properties)
            {
                var _ = property.GetValue(this);
            }

            _isInitialized = true;
        }
    }
}