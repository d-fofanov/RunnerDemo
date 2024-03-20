using System.IO;

namespace BaseModel.Fields
{
    /**
     * Model field for boolean values.
     */
    public class BoolModelField : BaseModelField<bool>
    {
        protected override void SerializeImpl(BinaryWriter writer, bool val)
        {
            writer.Write(val);
        }

        protected override bool DeserializeImpl(BinaryReader reader)
        {
            return reader.ReadBoolean();
        }
    }
}