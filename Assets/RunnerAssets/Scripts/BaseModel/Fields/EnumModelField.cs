using System;
using System.IO;

namespace BaseModel.Fields
{
    /**
     * Model field for enum values.
     */
    public class EnumModelField<T> : BaseModelField<T>
    {
        public EnumModelField()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException($"EnumModelField is used for non-enum type {typeof(T).Name}");
        }

        protected override void SerializeImpl(BinaryWriter writer, T val)
        {
            writer.Write(Convert.ToInt32(val));
        }

        protected override T DeserializeImpl(BinaryReader reader)
        {
            return (T) Enum.ToObject(typeof(T), reader.ReadInt32());
        }
    }
}