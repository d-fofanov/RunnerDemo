using System;
using System.IO;
using UnityEngine;

namespace BaseModel
{
    /**
     * Memory buffer for field's [de]serialization & encryption/decryption.
     */
    public class MemoryBuffer
    {
        public bool IsEmpty => _data.Length == 0;
        
        private byte[] _data = Array.Empty<byte>();
        private int _length = 0;
        
        public ReadOnlySpan<byte> Read()
        {
            return new ReadOnlySpan<byte>(_data, 0, _length);
        }

        public void Replace(ReadOnlySpan<byte> withData)
        {
            if (withData.Length >= _data.Length)
            {
                _data = new byte[Mathf.Max(_data.Length * 2, withData.Length)];
            }
            
            withData.CopyTo(_data);
            _length = withData.Length;
        }

        public void ReplaceAndEncrypt(ReadOnlySpan<byte> withData, ReadOnlySpan<byte> key)
        {
            Replace(withData);
            
            // NOTE can be significantly optimized
            for (var i = 0; i < _length; i++)
            {
                _data[i] ^= key[i % key.Length];
            }
        }

        public void ReadAndDecrypt(BinaryWriter target, ReadOnlySpan<byte> key)
        {
            // NOTE can be significantly optimized
            for (var i = 0; i < _length; i++)
            {
                target.Write((byte) (_data[i] ^ key[i % key.Length]));
            }
        }
    }
}