using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Utils
{
    /**
     * Utilities for encryption and decryption.
     */
    public static class EncryptionUtils
    {
        private static byte[] _data;
        
        public static void Initialize(byte[] data)
        {
            _data = data;
        }

        public static byte[] Generate(int length = 32)
        {
            var key = new byte[length];
            var rnd = new Random();
            for (int i = 0; i < key.Length; i++)
            {
                key[i] = (byte) rnd.Next(byte.MinValue, byte.MaxValue);
            }

            return key;
        }
        
        public static bool TryDeserializeReliableFile<T>(string path, out T result, out string error)
        {
            result = default;
            
            try
            {
                if (!ReadAndDecryptReliableFile(path, out var resultBytes, out error))
                    return false;

                var json = Encoding.UTF8.GetString(resultBytes);
                result = JsonUtility.FromJson<T>(json);
                return true;
            }
            catch (Exception e)
            {
                error = $"deserialization error: {e}";
                return false;
            }
        }
        
        public static bool ReadAndDecryptReliableFile(string path, out byte[] result, out string error)
        {
            result = default;
            
            var readBytes = FileUtils.ReadReliableFile(path, out error);
            if (readBytes == null || readBytes.Length == 0)
                return false;
            
            try
            {
                using var aes = Aes.Create();
                
                aes.Key = _data;
                var iv = new byte[aes.IV.Length];
                Array.Copy(readBytes, 3, iv, 0, iv.Length);
                aes.IV = iv;

                using var decr = aes.CreateDecryptor();
                
                result = decr.TransformFinalBlock(readBytes, 3 + iv.Length, readBytes.Length - 3 - iv.Length);

                return true;
            }
            catch (Exception e)
            {
                error = $"decr error: {e}";
                return false;
            }
        }

        public static bool TrySerializeReliableFile<T>(string path, T data, out string error)
        {
            var json = JsonUtility.ToJson(data);
            var jsonBytes = Encoding.UTF8.GetBytes(json);

            return EncryptAndWriteReliableFile(path, jsonBytes, out error);
        }

        public static bool EncryptAndWriteReliableFile(string path, byte[] data, out string error)
        {
            byte[] towrite;
            try
            {
                using var aes = Aes.Create();
                
                aes.Key = _data;
                using var encryptor = aes.CreateEncryptor();
                
                var result = encryptor.TransformFinalBlock(data, 0, data.Length);

                towrite = new byte[result.Length + aes.IV.Length + 3];
                towrite[0] = GetRandomByte();
                towrite[1] = GetRandomByte();
                towrite[2] = GetRandomByte();
                Array.Copy(aes.IV, 0, towrite, 3, aes.IV.Length);
                Array.Copy(result, 0, towrite, 3 + aes.IV.Length, result.Length);
            }
            catch (CryptographicException e)
            {
                error = $"encr error: {e}";
                return false;
            }

            return FileUtils.ReliablyWriteFile(path, towrite, out error);
        }

        private static readonly Random _rnd = new ();
        private static byte GetRandomByte()
        {
            return (byte)_rnd.Next(byte.MinValue, byte.MaxValue);
        }
    }
}