using System.IO;
using RX;

namespace BaseModel.Fields
{
    /**
     * Base class for model fields. It handles encryption and serialization.
     */
    public abstract class BaseModelField<T> : ReactiveProperty<T>, IModelField
    {
        private const int KeyLength = 16;

        private readonly byte[] _key;
        private readonly MemoryBuffer _encryptedValue = new ();

        public override T Value
        {
            get
            {
                if (_encryptedValue.IsEmpty)
                    return default;
                
                // decrypt and return
                var reader = ModelFieldUtil.Decrypt(_encryptedValue, _key);
                return DeserializeImpl(reader);
            }
            set
            {
                var oldVal = Value;
                if (Equals(oldVal, value))
                    return;
                
                // encrypt and store
                var writer = ModelFieldUtil.GetWriter();
                SerializeImpl(writer, value);
                _encryptedValue.ReplaceAndEncrypt(ModelFieldUtil.GetWritten(), _key);
                
                OnValueChange(oldVal, value);
            }
        }

        protected BaseModelField()
        {
            _key = new byte[KeyLength];
            ModelFieldUtil.GenerateKey(_key);
        }

        protected abstract void SerializeImpl(BinaryWriter writer, T val);
        protected abstract T DeserializeImpl(BinaryReader reader);
        
        public void Serialize(BinaryWriter writer)
        {
            SerializeImpl(writer, Value);
        }

        public void Deserialize(BinaryReader reader)
        {
            Value = DeserializeImpl(reader);
        }
    }
}