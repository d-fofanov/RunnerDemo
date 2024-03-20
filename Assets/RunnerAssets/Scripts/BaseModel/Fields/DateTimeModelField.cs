using System;
using System.IO;

namespace BaseModel.Fields
{
    /**
     * Model field for DateTime values.
     */
    public class DateTimeModelField : BaseModelField<DateTime>
    {
        protected override void SerializeImpl(BinaryWriter writer, DateTime val)
        {
            writer.Write(val.ToBinary());
        }

        protected override DateTime DeserializeImpl(BinaryReader reader)
        {
            return DateTime.FromBinary(reader.ReadInt64());
        }
    }
}