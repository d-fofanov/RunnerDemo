using System.IO;
using RunnerModel;

namespace BaseModel.Fields
{
    /**
     * Model field for RunnerMap values.
     */
    public class RunnerMapField : BaseModelField<MapModel>
    {
        protected override void SerializeImpl(BinaryWriter writer, MapModel val)
        {
            writer.Write(val.Width);
            writer.Write(val.Height);
            for (int i = 0; i < val.Width; i++)
                writer.Write(val.GetColumn(i));
        }

        protected override MapModel DeserializeImpl(BinaryReader reader)
        {
            var width = reader.ReadInt32();
            var height = reader.ReadInt32();
            var map = new uint[width];
            for (int i = 0; i < width; i++)
                map[i] = reader.ReadUInt32();
            
            return new MapModel(map, width, height);
        }
    }
}