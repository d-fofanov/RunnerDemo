using System.IO;

namespace BaseModel.Fields
{
    /**
    * Model field for long values.
    */
    public class LongModelField : BaseModelField<long>
    {
        protected override void SerializeImpl(BinaryWriter writer, long val)
        {
            writer.Write(val);
        }

        protected override long DeserializeImpl(BinaryReader reader)
        {
            return reader.ReadInt64();
        }
    }
}