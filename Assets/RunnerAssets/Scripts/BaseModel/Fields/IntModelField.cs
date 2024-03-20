using System.IO;

namespace BaseModel.Fields
{
    /**
     * Model field for int values.
     */
    public class IntModelField : BaseModelField<int>
    {
        protected override void SerializeImpl(BinaryWriter writer, int val)
        {
            writer.Write(val);
        }

        protected override int DeserializeImpl(BinaryReader reader)
        {
            return reader.ReadInt32();
        }
    }
}