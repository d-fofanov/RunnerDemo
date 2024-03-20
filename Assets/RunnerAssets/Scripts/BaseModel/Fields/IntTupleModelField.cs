using System.IO;

namespace BaseModel.Fields
{
    /**
     * Model field for int tuple values.
     */
    public class IntTupleModelField : BaseModelField<(int x, int y)>
    {
        protected override void SerializeImpl(BinaryWriter writer, (int x, int y) val)
        {
            writer.Write(val.x);
            writer.Write(val.y);
        }

        protected override (int x, int y) DeserializeImpl(BinaryReader reader)
        {
            return (reader.ReadInt32(), reader.ReadInt32());
        }
    }
}