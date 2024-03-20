using System;
using System.Collections.Generic;
using System.IO;
using BaseModel.Fields;
using RunnerModel;
using UnityEngine;
using Random = System.Random;

namespace BaseModel
{
    /**
     * Common facility for model fields, holds common buffers and factory.
     */
    public static class ModelFieldUtil
    {
        public static BaseModelField<T> Create<T>()
        {
            var t = typeof(T);
            if (t.IsEnum)
                return new EnumModelField<T>();
            
            if (!Constructors.TryGetValue(t, out var constructor))
                throw new Exception($"[BaseModelField] Factory is requested to create field for an unknown type {t.Name}");

            return constructor() as BaseModelField<T>;
        }

        public static void GenerateKey(byte[] target)
        {
            _random.NextBytes(target);
        }

        public static BinaryReader Decrypt(MemoryBuffer buf, ReadOnlySpan<byte> key)
        {
            _bufferStream.Seek(0, SeekOrigin.Begin);
            buf.ReadAndDecrypt(_bufferWriter, key);
            _bufferStream.Seek(0, SeekOrigin.Begin);
            return _bufferReader;
        }
        
        public static BinaryWriter GetWriter()
        {
            _bufferStream.Seek(0, SeekOrigin.Begin);
            return _bufferWriter;
        }
        
        public static ReadOnlySpan<byte> GetWritten()
        {
            return new ReadOnlySpan<byte>(_bufferStream.GetBuffer(), 0, (int) _bufferStream.Position);
        }

        private static readonly MemoryStream _bufferStream = new();
        private static readonly BinaryWriter _bufferWriter = new(_bufferStream);
        private static readonly BinaryReader _bufferReader = new(_bufferStream);
        private static readonly Random _random = new();
        private static readonly Dictionary<Type, Func<IModelField>> Constructors = new()
        {
            { typeof(bool), () => new BoolModelField() },
            { typeof(int), () => new IntModelField() },
            { typeof(long), () => new LongModelField() },
            { typeof(DateTime), () => new DateTimeModelField() },
            { typeof((int x, int y)), () => new IntTupleModelField() },
            { typeof(Vector3), () => new Vector3ModelField() },
            { typeof(MapModel), () => new RunnerMapField() },
        };
    }
}